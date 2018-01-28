using App.Domain.Common;
using App.Domain.Orders;
using App.Service.Customers;
using App.Service.GenericAttribute;
using App.Service.Post;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Service.Orders
{
    public class OrderProcessingService : IOrderProcessingService
    {
        private readonly IOrderService _orderService;
        public readonly IShoppingCartItemService _shoppingCartItemService;
        public readonly ICustomerService _customerService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IPriceCalculationService _priceCalculationService;

        public OrderProcessingService(IOrderService orderService
            , IShoppingCartItemService shoppingCartItemService
            , ICustomerService customerService
            , IGenericAttributeService genericAttributeService
            , IPriceCalculationService priceCalculationService)
        {
            _orderService = orderService;
            _shoppingCartItemService = shoppingCartItemService;
            _customerService = customerService;
            _genericAttributeService = genericAttributeService;
            _priceCalculationService = priceCalculationService;
        }

        public virtual PlaceOrderResult PlaceOrder(
           ProcessPaymentRequest processPaymentRequest,
           Dictionary<string, string> extraData)
        {
            PlaceOrderResult result = new PlaceOrderResult();
            var utcNow = DateTime.UtcNow;

            if (processPaymentRequest == null)
                throw new AggregateException("processPaymentRequest");

            if (processPaymentRequest.OrderGuid == Guid.Empty)
                processPaymentRequest.OrderGuid = Guid.NewGuid();

            try
            {
                var customer = _customerService.GetById(processPaymentRequest.CustomerId, isCache: false);
                var cart = customer.GetCartItems();

                //Order total 
                decimal? orderTotal = null;
                string subTotal = _shoppingCartItemService.GetCurrentCartSubTotal(cart);
                if (subTotal != null)
                    orderTotal = decimal.Parse(subTotal);

                //BillingAddress  
                Address billingAddress = null;// customer.Addresses;
                billingAddress = customer.BillingAddress;

                //Shiping method
                string shippingMethod = customer.GetAttribute("Customer", App.Aplication.Contains.SelectedShippingOption, _genericAttributeService);

                var order = new Order()
                {
                    StoreId = processPaymentRequest.StoreId,
                    OrderGuid = processPaymentRequest.OrderGuid,
                    CustomerId = customer.Id,
                    CustomerLanguageId = 1, // Draft code
                    CustomerTaxDisplayTypeId = 0, //Draft code
                    CustomerIp = "172.0.0.0", //Draft code
                    OrderSubtotalInclTax = 0, //Draft code
                    OrderSubtotalExclTax = 0, //Draft code
                    OrderSubTotalDiscountInclTax = decimal.Zero, //Draft code
                    OrderSubTotalDiscountExclTax = decimal.Zero,//Draft code
                    OrderShippingInclTax = decimal.Zero,//Draft code
                    OrderShippingExclTax = decimal.Zero,//Draft code
                    OrderShippingTaxRate = decimal.Zero,//Draft code
                    PaymentMethodAdditionalFeeInclTax = decimal.Zero,//Draft code
                    PaymentMethodAdditionalFeeExclTax = decimal.Zero,//Draft code
                    PaymentMethodAdditionalFeeTaxRate = decimal.Zero,//Draft code
                    TaxRates = string.Empty,//Draft code
                    OrderTax = decimal.Zero,//Draft code
                    BillingAddress = billingAddress,
                    PaidDateUtc = utcNow,
                    CreatedOnUtc = utcNow,
                    UpdatedOnUtc = utcNow,
                    PaymentMethodSystemName = processPaymentRequest.PaymentMethodSystemName,
                    CustomerCurrencyCode = "VNĐ",
                    CurrencyRate = decimal.Zero,
                    OrderTotal = orderTotal.Value,
                    ShippingMethod = shippingMethod,
                    ShippingStatus = ShippingStatus.ShippingNotRequired,

                    OrderStatus = OrderStatus.Pending,
                    PaymentStatus = PaymentStatus.Pending

                };

                _orderService.Create(order);

                result.PlacedOrder = order;

                //Insert OrderItem
                foreach (var sc in cart)
                {
                    decimal scSubTotal = _priceCalculationService.GetSubTotal(sc, true);
                    decimal scSubTotalInclTax = scSubTotal;

                    var orderItem = new OrderItem()
                    {
                        OrderItemGuid = Guid.NewGuid(),
                        Order = order,
                        PostId = sc.PostId,
                        Quantity = sc.Quantity,
                        UnitPriceInclTax = sc.CustomerEnteredPrice,
                        PriceInclTax = scSubTotalInclTax
                    };

                    order.OrderItems.Add(orderItem);
                    _orderService.Update(order);
                }

                if (result.Success)
                {
                    //Delete
                    cart.ToList().ForEach(cr => _shoppingCartItemService.DeleteShoppingCartItem(cr, false));

                    _customerService.ResetCheckoutData(customer: customer, storeId: 1, clearCouponCodes: false, clearCheckoutAttributes: true, clearRewardPoints: true, clearShippingMethod: true, clearPaymentMethod: true);

                }
            }
            catch
            {
                
            }

            return result;
        }
    }
}
