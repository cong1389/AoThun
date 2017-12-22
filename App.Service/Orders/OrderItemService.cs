using App.Core.Utils;
using App.Domain.Interfaces.Services;
using App.Domain.Orders;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Orders;
using App.Infra.Data.UOW.Interfaces;
using System.Collections.Generic;

namespace App.Service.Orders
{
    public class OrderItemService : BaseService<OrderItem>, IOrderItemService, IBaseService<OrderItem>, IService
	{
		private readonly IOrderItemRepository _OrderItemRepository;

		private readonly IUnitOfWork _unitOfWork;

		public OrderItemService(IUnitOfWork unitOfWork, IOrderItemRepository OrderItemRepository) : base(unitOfWork, OrderItemRepository)
		{
			this._unitOfWork = unitOfWork;
			this._OrderItemRepository = OrderItemRepository;
		}

		public OrderItem GetById(int Id)
		{
			return this._OrderItemRepository.GetById(Id);
		}

		public IEnumerable<OrderItem> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
		{
			return this._OrderItemRepository.PagedSearchList(sortbuBuilder, page);
		}
	}
}