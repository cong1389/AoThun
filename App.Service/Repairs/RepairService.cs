using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Repairs;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;

namespace App.Service.Repairs
{
    public class RepairService : BaseService<App.Domain.Entities.Data.Repair>, IRepairService, IBaseService<App.Domain.Entities.Data.Repair>, IService
    {
        private readonly IRepairRepository _orderRepository;

        private readonly IUnitOfWork _unitOfWork;

        public RepairService(IUnitOfWork unitOfWork, IRepairRepository orderRepository) : base(unitOfWork, orderRepository)
        {
            this._unitOfWork = unitOfWork;
            this._orderRepository = orderRepository;
        }

        public IEnumerable<App.Domain.Entities.Data.Repair> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
        {
            return this._orderRepository.PagedSearchList(sortbuBuilder, page);
        }
    }
}