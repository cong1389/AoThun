using App.Aplication.Filters;
using App.Core.Extensions;
using App.Domain.Entities.Data;
using App.Domain.Entities.Orders;
using App.Extensions;
using App.FakeEntity.Common;
using App.Front.Models.Checkout;
using App.Front.Models.ShoppingCart;
using App.Service.Addresses;
using App.Service.Common;
using App.Service.Customers;
using App.Service.GenericAttribute;
using App.Service.Orders;
using App.Service.PaymentMethodes;
using App.Service.Post;
using App.Service.ShippingMethodes;
using App.Aplication;
using App.Aplication.MVCHelper;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace App.Front.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IWorkContext _workContext;

        private readonly ICustomerService _customerService;

        private readonly IAddressService _addressService;

        private readonly IPostService _postService;

        public readonly IShoppingCartItemService _shoppingCartItemService;

        public readonly IPaymentMethodService _paymentMethodService;

        public readonly IShippingMethodService _shippingMethodService;

        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IOrderService _orderService;
        private readonly IOrderProcessingService _orderProcessingService;

        public CheckoutController(IWorkContext workContext, ICustomerService customerService, IAddressService addressService, IPostService postService
            , IShoppingCartItemService shoppingCartItemService
            , IPaymentMethodService paymentMethodService
            , IGenericAttributeService genericAttributeService
            , IShippingMethodService shippingMethodService
            , IOrderService orderService
            , IOrderProcessingService orderProcessingService)
        {
            _workContext = workContext;
            _customerService = customerService;
            _addressService = addressService;
            _postService = postService;
            _shoppingCartItemService = shoppingCartItemService;
            _paymentMethodService = paymentMethodService;
            _genericAttributeService = genericAttributeService;
            _shippingMethodService = shippingMethodService;
            _orderService = orderService;
            _orderProcessingService = orderProcessingService;
        }

        #region Billing Address
        
        public ActionResult BillingAddress()
        {
            var cart = _workContext.CurrentCustomer.GetCartItems();

            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Home");
            }

            var model = PrepareBillingAddressModel();

            return View(model);
        }

        [NonAction]
        protected CheckoutBillingAddressModel PrepareBillingAddressModel()
        {
            var model = new CheckoutBillingAddressModel();
            try
            {
                model.NewAddress = new AddressViewModel();

                var billAddress = _workContext.CurrentCustomer.Addresses;
                if (billAddress.Count != 0 && billAddress.Any())
                {
                    foreach (var address in billAddress)
                    {
                        model.NewAddress = Mapper.Map<AddressViewModel>(address);
                    }
                }
            }
            catch
            {                
            }

            return model;
        }
        
        [HttpPost, ActionName("BillingAddress")]
        [FormValueRequired("nextstep")]
        public ActionResult NewBillingAddress(CheckoutBillingAddressModel model)
        {
            var addresses = _workContext.CurrentCustomer.Addresses;

            if (addresses.Count == 0 || !addresses.Any())
            {
                if (ModelState.IsValid)
                {
                    var address = model.NewAddress.ToEntity();

                    //some validation
                    if (address.CountryId == 0)
                        address.CountryId = null;

                    if (address.StateProvinceId == 0)
                        address.StateProvinceId = null;

                    _workContext.CurrentCustomer.Addresses.Add(address);
                    _workContext.CurrentCustomer.BillingAddress = address;

                    _customerService.Update(_workContext.CurrentCustomer);

                    return RedirectToAction("PaymentMethod");
                }
            }
            else
            {
                var objAddress = addresses.FirstOrDefault();

                objAddress.FirstName = model.NewAddress.FirstName;
                objAddress.Email = model.NewAddress.Email;
                objAddress.PhoneNumber = model.NewAddress.PhoneNumber;
                objAddress.Address1 = model.NewAddress.Address1;

                _addressService.Update(objAddress);

                return RedirectToAction("PaymentMethod");
            }

            //If we got this far, something failed, redisplay form
            model = PrepareBillingAddressModel();

            return View(model);
        }

        #endregion

        #region Payment method
        
        public ActionResult PaymentMethod()
        {
            var cart = _workContext.CurrentCustomer.GetCartItems();

            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Home");
            }

            var model = PreparePaymentMethodModel(cart);

            return View(model);
        }

        [NonAction]
        protected CheckoutPaymentMethodModel PreparePaymentMethodModel(IOrderedEnumerable<ShoppingCartItem> cart)
        {
            var model = new CheckoutPaymentMethodModel();
            var customer = _workContext.CurrentCustomer;

            // var paymentTypes = new PaymentMethodType[] { PaymentMethodType.Standard, PaymentMethodType.Redirection, PaymentMethodType.StandardAndRedirection };

            var payment = _paymentMethodService.GetAll();

            if (payment.Any())
            {
                model.PaymentMethods = (from x in payment
                                        select new CheckoutPaymentMethodModel.PaymentMethodModel()
                                        {
                                            Id = x.Id,
                                            PaymentMethodSystemName = x.PaymentMethodSystemName,
                                            FullDescription = x.FullDescription
                                        }).ToList();

            }

            var selectedPaymentMethodSystemName = _workContext.CurrentCustomer.GetAttribute("Customer", Contains.SelectedPaymentMethod, _genericAttributeService);

            bool selected = false;
            if (selectedPaymentMethodSystemName.HasValue())
            {
                var paymentMethodToSelect = model.PaymentMethods.Find(pm => pm.PaymentMethodSystemName.IsCaseInsensitiveEqual(selectedPaymentMethodSystemName));
                if (paymentMethodToSelect != null)
                {
                    paymentMethodToSelect.Selected = true;
                    selected = true;
                }
            }

            // if no option has been selected, let's do it for the first one
            if (!selected)
            {
                var paymentMethodToSelect = model.PaymentMethods.FirstOrDefault();
                if (paymentMethodToSelect != null)
                    paymentMethodToSelect.Selected = true;
            }

            return model;
        }

        [HttpPost, ActionName("PaymentMethod")]
        [FormValueRequired("nextstep")]
        public ActionResult SelectPaymentMethod(string paymentmethod, CheckoutPaymentMethodModel model, FormCollection form)
        {
            var customer = _workContext.CurrentCustomer;
            var cart = customer.GetCartItems();

            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Home");
            }

            //Save payment method
            string paymentMethodKey = form.AllKeys.FirstOrDefault(m => m.StartsWith("payment_method_id", StringComparison.InvariantCultureIgnoreCase));
            if (paymentMethodKey.HasValue())
            {
                var id = paymentMethodKey.Replace("payment_method_id", "").Trim();
                string paymentMethodName = form[paymentMethodKey];
                _genericAttributeService.SaveGenericAttribute(customer.Id, "Customer", Contains.SelectedPaymentMethod, paymentMethodName);
            }

            //Save shipping method
            string shippingMethodKey = form.AllKeys.FirstOrDefault(m => m.StartsWith("shipping_rate_id", StringComparison.InvariantCultureIgnoreCase));
            if (shippingMethodKey.HasValue())
            {
                var id = shippingMethodKey.Replace("shipping_rate_id", "").Trim();
                string shippingMethodName = form[shippingMethodKey];
                _genericAttributeService.SaveGenericAttribute(customer.Id, "Customer", Contains.SelectedShippingOption, shippingMethodName);
            }

            return RedirectToAction("Confirm");
        }

        #endregion

        #region Shipping method

        [ChildActionOnly]
        public ActionResult ShippingMethod()
        {
            var cart = _workContext.CurrentCustomer.GetCartItems();

            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Home");
            }

            var model = PrepareShippingMethodModel(cart);

            return PartialView(model);
        }

        [NonAction]
        protected CheckoutShippingMethodModel PrepareShippingMethodModel(IOrderedEnumerable<ShoppingCartItem> cart)
        {
            var model = new CheckoutShippingMethodModel();
            //var customer = _workContext.CurrentCustomer;
            var shipping = _shippingMethodService.GetAll();

            if (shipping.Any())
            {
                model.ShippingMethods = (from x in shipping
                                         select new CheckoutShippingMethodModel.ShippingMethodModel()
                                         {
                                             Id = x.Id,
                                             Name = x.Name,
                                             Description = x.Description
                                         }).ToList();

            }

            var selectedShippingOption = _workContext.CurrentCustomer.GetAttribute("Customer", Contains.SelectedShippingOption, _genericAttributeService);

            bool selected = false;
            if (selectedShippingOption.HasValue())
            {
                var shippingOptionToSelect = model.ShippingMethods.ToList()
                         .Find(so => !String.IsNullOrEmpty(so.Name) && so.Name.Equals(selectedShippingOption, StringComparison.InvariantCultureIgnoreCase));

                if (shippingOptionToSelect != null)
                {
                    shippingOptionToSelect.Selected = true;
                    selected = true;
                }
            }

            // if no option has been selected, let's do it for the first one
            if (!selected)
            {
                var shippingOptionToSelect = model.ShippingMethods.FirstOrDefault();
                if (shippingOptionToSelect != null)
                    shippingOptionToSelect.Selected = true;
            }

            return model;
        }

        #endregion

        #region Cart checkout

        /// <summary>
        /// Get Cart theo khách hàng
        /// </summary>
        /// <returns></returns>
        private MiniShoppingCartModel GetCartByCustomer()
        {
            //Get Cart by customer
            var cart = _workContext.CurrentCustomer.GetCartItems();
            List<Post> lstPost = new List<Post>();

            if (cart.Any())
            {
                foreach (var item in cart)
                {
                    Post objPost = _postService.GetById(item.PostId);
                    lstPost.Add(objPost);
                }
            }
            var miniShopping = new MiniShoppingCartModel
            {
                Items = lstPost,
                ShoppingCarts = cart,
                SubTotal = _shoppingCartItemService.GetCurrentCartSubTotal(cart)
            };

            HttpContext.Session["OrderPaymentInfo"] = miniShopping;

            return miniShopping;
        }

        /// <summary>
        /// Ajax show sản phẩm bên trái khi checkout
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> CartByCustomer()
        {
            var miniCart = GetCartByCustomer();

            JsonResult jsonResult = Json(new { success = true, list = this.RenderRazorViewToString("_Checkout.Cart", miniCart) }, JsonRequestBehavior.AllowGet);

            return jsonResult;

        }

        #endregion

        #region Confirm

        public ActionResult Confirm()
        {
            var cart = _workContext.CurrentCustomer.GetCartItems();

            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Home");
            }

            var model = PrepareConfirmOrderModel(cart);

            return View(model);
        }

        private CheckoutConfirmModel PrepareConfirmOrderModel(IOrderedEnumerable<ShoppingCartItem> cart)
        {
            var model = new CheckoutConfirmModel();

            return model;
        }

        [HttpPost, ActionName("Confirm")]
        [ValidateInput(false)]
        public ActionResult ConfirmOrder(FormCollection form)
        {
            var storeId = 0;
            var customer = _workContext.CurrentCustomer;
            var cart = _workContext.CurrentCustomer.GetCartItems();

            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Home");
            }
           
            MiniShoppingCartModel miniShoppingCartModelm = HttpContext.Session["OrderPaymentInfo"] as MiniShoppingCartModel;

            PlaceOrderResult placeOrderResult = null;

            var processPaymentRequest = new ProcessPaymentRequest();
            processPaymentRequest.StoreId = storeId;
            processPaymentRequest.CustomerId = customer.Id;
            processPaymentRequest.PaymentMethodSystemName = customer.GetAttribute("Customer", Contains.SelectedPaymentMethod, _genericAttributeService);

            var placeOrderExtraData = new Dictionary<string, string>();

            placeOrderResult = _orderProcessingService.PlaceOrder(processPaymentRequest, placeOrderExtraData);

            return RedirectToAction("Index", "Home");
        }

        #endregion

    }
}