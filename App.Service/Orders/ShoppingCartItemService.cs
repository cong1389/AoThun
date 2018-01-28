using App.Core.Caching;
using App.Core.Utils;
using App.Domain.Entities.Orders;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Orders;
using App.Infra.Data.UOW.Interfaces;
using App.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Service.Orders
{
    public class ShoppingCartItemService : BaseService<ShoppingCartItem>, IShoppingCartItemService, IBaseService<ShoppingCartItem>, IService
    {
        private const string CACHE_SHOPPINGCARTITEM_KEY = "db.ShoppingCartItem.{0}";
        private readonly ICacheManager _cacheManager;

        private readonly IShoppingCartItemRepository _shoppingCartItemRepository;

        private readonly IUnitOfWork _unitOfWork;

        private readonly IWorkContext _workContext;

        private readonly IOrderTotalCalculationService _orderTotalCalculationService;

        public ShoppingCartItemService(IUnitOfWork unitOfWork, IShoppingCartItemRepository shoppingCartItemRepository
            , IWorkContext workContext
             , IOrderTotalCalculationService orderTotalCalculationService
            , ICacheManager cacheManager)
            : base(unitOfWork, shoppingCartItemRepository)
        {
            this._unitOfWork = unitOfWork;
            this._shoppingCartItemRepository = shoppingCartItemRepository;
            _workContext = workContext;
            _orderTotalCalculationService = orderTotalCalculationService;
            _cacheManager = cacheManager;
        }

        public void AddToCart(AddToCartContext ctx)
        {
            try
            {
                var customer = ctx.Customers ?? _workContext.CurrentCustomer;
                int storeId = 1;

                IEnumerable<ShoppingCartItem> ieShoppingCart = _workContext.CurrentCustomer.ShoppingCartItems;

                ShoppingCartItem objNew = new ShoppingCartItem
                {
                    PostId = ctx.Post.Id,
                    CustomerId = customer.Id,
                    CustomerEnteredPrice = ctx.Price,
                    StoreId = storeId
                };

                //Customer chua co trong ShoppingCartItem
                if (ieShoppingCart == null || ieShoppingCart.Count() == 0)
                {
                    objNew.Quantity = ctx.Quantity <= 0 ? 1 : ctx.Quantity;
                    objNew.CustomerEnteredPrice = ctx.Price;

                    //Create cart
                    this.Create(objNew);
                }
                //Customer da co trong ShoppingCartItem
                else
                {
                    //Kiem tra PostId cua customer nay co trong ShoppingCartItem chua
                    ShoppingCartItem objOld = ieShoppingCart.Where(x => x.PostId == ctx.Post.Id).FirstOrDefault();

                    // int quantityOld = ieShoppingCart.Where(x => x.PostId == ctx.Post.Id && x.CustomerId == customer.Id).FirstOrDefault().Quantity;
                    if (objOld != null)
                    {
                        //Update và cộng dồn số lượng
                        objOld.Quantity = ctx.Quantity;
                        objOld.CustomerEnteredPrice = ctx.Price;

                        //objOld.Quantity = objOld.Quantity + ctx.Quantity;
                        this.Update(objOld);
                    }
                    //Cutomer, postId da co trong ShoppingCartItem
                    else
                    {
                        objNew.Quantity = ctx.Quantity <= 0 ? 1 : ctx.Quantity;
                        objNew.CustomerEnteredPrice = ctx.Price;

                        //Create cart
                        this.Create(objNew);
                    }
                }
            }
            catch 
            {
                
            }
        }

        public ShoppingCartItem GetById(int id, bool isCache = true)
        {
            ShoppingCartItem shoppingCartItem;
            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_SHOPPINGCARTITEM_KEY, "GetById");
                sbKey.Append(id);

                string key = sbKey.ToString();
                shoppingCartItem = _cacheManager.Get<ShoppingCartItem>(key);
                if (shoppingCartItem == null)
                {
                    shoppingCartItem = _shoppingCartItemRepository.GetById(id);
                    _cacheManager.Put(key, shoppingCartItem);
                }
            }
            else
            {
                shoppingCartItem = _shoppingCartItemRepository.GetById(id);
            }

            //StringBuilder sbKey = new StringBuilder();
            //sbKey.AppendFormat(CACHE_SHOPPINGCARTITEM_KEY, "GetById");
            //sbKey.Append(id);

            //string key = sbKey.ToString();
            //ShoppingCartItem shoppingCartItem = _cacheManager.Get<ShoppingCartItem>(key);
            //if (shoppingCartItem == null)
            //{
            //    shoppingCartItem = _shoppingCartItemRepository.GetById(id);
            //    _cacheManager.Put(key, shoppingCartItem);
            //}

            return shoppingCartItem;
        }

        public IEnumerable<ShoppingCartItem> GetByPostId(int postId, bool isCache = true)
        {
            IEnumerable<ShoppingCartItem> shoppingCartItem;
            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_SHOPPINGCARTITEM_KEY, "GetByPostId");
                sbKey.Append(postId);

                string key = sbKey.ToString();
                shoppingCartItem = _cacheManager.GetCollection<ShoppingCartItem>(key);
                if (shoppingCartItem == null)
                {
                    shoppingCartItem = _shoppingCartItemRepository.FindBy((ShoppingCartItem x) => x.PostId == postId, false);
                    _cacheManager.Put(key, shoppingCartItem);
                }
            }
            else
            {
                shoppingCartItem = _shoppingCartItemRepository.FindBy((ShoppingCartItem x) => x.PostId == postId, false);
            }

            //StringBuilder sbKey = new StringBuilder();
            //sbKey.AppendFormat(CACHE_SHOPPINGCARTITEM_KEY, "GetByPostId");

            //sbKey.AppendFormat("-{0}", postId);

            //string key = sbKey.ToString();
            //IEnumerable<ShoppingCartItem> shoppingCartItem = _cacheManager.GetCollection<ShoppingCartItem>(key);
            //if (shoppingCartItem == null)
            //{
            //    shoppingCartItem = _shoppingCartItemRepository.FindBy((ShoppingCartItem x) => x.PostId == postId, false);
            //    _cacheManager.Put(key, shoppingCartItem);
            //}

            //IEnumerable<ShoppingCartItem> posts = this._shoppingCartItemRepository.FindBy((ShoppingCartItem x) => x.PostId == postId, false);
            return shoppingCartItem;
        }

        public IEnumerable<ShoppingCartItem> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
        {
            return this._shoppingCartItemRepository.PagedSearchList(sortbuBuilder, page);
        }

        public string GetCurrentCartSubTotal(IOrderedEnumerable<ShoppingCartItem> cart)
        {
            return _orderTotalCalculationService.GetCurrentCartSubTotal(cart).ToString();
        }

        public virtual void DeleteShoppingCartItem(ShoppingCartItem shoppingCartItem, bool resetCheckoutData = true,
            bool ensureOnlyActiveCheckoutAttributes = false, bool deleteChildCartItems = true)
        {
            if (shoppingCartItem == null)
                throw new ArgumentNullException("shoppingCartItem");

            this.Delete(shoppingCartItem);
        }
    }
}