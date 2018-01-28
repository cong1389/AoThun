using App.Core.Utils;
using App.Domain.Interfaces.Services;
using App.Domain.Orders;
using System.Collections.Generic;

namespace App.Service.Orders
{
    public interface IOrderService : IBaseService<Order>, IService
	{
		Order GetById(int id, bool isCache = true);

        IEnumerable<Order> GetByCustomerId(int customerId, bool isCache = true);

        IEnumerable<Order> PagedList(SortingPagingBuilder sortBuider, Paging page);
	}
}