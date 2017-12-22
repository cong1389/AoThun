using App.Core.Utils;
using App.Domain.Interfaces.Services;
using App.Domain.Shippings;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.ShippingMethods;
using App.Infra.Data.UOW.Interfaces;
using System.Collections.Generic;

namespace App.Service.ShippingMethodes
{
    public class ShippingMethodService : BaseService<ShippingMethod>, IShippingMethodService, IBaseService<ShippingMethod>, IService
	{
		private readonly IShippingMethodRepository _ShippingMethodRepository;

		private readonly IUnitOfWork _unitOfWork;

		public ShippingMethodService(IUnitOfWork unitOfWork, IShippingMethodRepository ShippingMethodRepository) : base(unitOfWork, ShippingMethodRepository)
		{
			this._unitOfWork = unitOfWork;
			this._ShippingMethodRepository = ShippingMethodRepository;
		}

		public ShippingMethod GetById(int Id)
		{
			return this._ShippingMethodRepository.GetById(Id);
		}

		public IEnumerable<ShippingMethod> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
		{
			return this._ShippingMethodRepository.PagedSearchList(sortbuBuilder, page);
		}
	}
}