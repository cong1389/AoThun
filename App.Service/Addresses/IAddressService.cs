using App.Core.Utils;
using App.Domain.Common;
using App.Domain.Interfaces.Services;
using System.Collections.Generic;

namespace App.Service.Addresses
{
    public interface IAddressService : IBaseService<Address>, IService
	{
		Address GetById(int Id, bool isCache = true);

		IEnumerable<Address> PagedList(SortingPagingBuilder sortBuider, Paging page);
	}
}