using App.Core.Utils;
using App.Domain.Interfaces.Services;
using App.Domain.Shippings;
using System.Collections.Generic;

namespace App.Service.ShippingMethodes
{
    public interface IShippingMethodService : IBaseService<ShippingMethod>, IService
	{
		ShippingMethod GetById(int Id);

		IEnumerable<ShippingMethod> PagedList(SortingPagingBuilder sortBuider, Paging page);
	}
}