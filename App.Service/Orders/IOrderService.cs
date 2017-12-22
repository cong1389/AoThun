using App.Core.Utils;
using App.Domain.Interfaces.Services;
using App.Domain.Orders;
using System.Collections.Generic;

namespace App.Service.Orders
{
    public interface IOrderService : IBaseService<Order>, IService
	{
		Order GetById(int Id);

		IEnumerable<Order> PagedList(SortingPagingBuilder sortBuider, Paging page);
	}
}