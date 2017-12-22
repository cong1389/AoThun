using App.Domain.Entities.Data;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Orders;
using App.Infra.Data.Repository.Repairs;
using App.Infra.Data.UOW.Interfaces;
using System;

namespace App.Service.Repairs
{
	public class RepairItemService : BaseService<RepairItem>, IRepairItemService, IBaseService<RepairItem>, IService
	{
		private readonly IRepairItemRepository _orderItemRepository;

		private readonly IUnitOfWork _unitOfWork;

		public RepairItemService(IUnitOfWork unitOfWork, IRepairItemRepository orderItemRepository) : base(unitOfWork, orderItemRepository)
		{
			this._unitOfWork = unitOfWork;
			this._orderItemRepository = orderItemRepository;
		}
	}
}