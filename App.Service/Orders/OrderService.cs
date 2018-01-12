using App.Core.Caching;
using App.Core.Utils;
using App.Domain.Interfaces.Services;
using App.Domain.Orders;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Orderes;
using App.Infra.Data.UOW.Interfaces;
using System.Collections.Generic;
using System.Text;

namespace App.Service.Orders
{
    public class OrderService : BaseService<Order>, IOrderService, IBaseService<Order>, IService
	{
        private const string CACHE_ORDER_KEY = "db.Order.{0}";
        private readonly ICacheManager _cacheManager;

        private readonly IOrderRepository _orderRepository;

		private readonly IUnitOfWork _unitOfWork;

		public OrderService(IUnitOfWork unitOfWork, IOrderRepository orderRepository, ICacheManager cacheManager) : base(unitOfWork, orderRepository)
		{
			this._unitOfWork = unitOfWork;
			this._orderRepository = orderRepository;
            _cacheManager = cacheManager;

        }

		public Order GetById(int id, bool isCache = true)
        {
            Order order;
            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_ORDER_KEY, "GetById");
                sbKey.Append(id);

                string key = sbKey.ToString();
                order = _cacheManager.Get<Order>(key);
                if (order == null)
                {
                    order = _orderRepository.GetById(id);
                    _cacheManager.Put(key, order);
                }
            }
            else
            {
                order = _orderRepository.GetById(id);
            }

            //StringBuilder sbKey = new StringBuilder();
            //sbKey.AppendFormat(CACHE_ORDER_KEY, "GetById");
            //sbKey.Append(id);

            //string key = sbKey.ToString();
            //Order order = _cacheManager.Get<Order>(key);
            //if (order == null)
            //{
            //    order = _orderRepository.GetById(id);
            //    _cacheManager.Put(key, order);
            //}

            return order;
		}

		public IEnumerable<Order> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
		{
			return this._orderRepository.PagedSearchList(sortbuBuilder, page);
		}
	}
}