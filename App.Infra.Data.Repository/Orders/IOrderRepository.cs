using App.Core.Utils;
using App.Domain.Interfaces.Repository;
using App.Domain.Orders;
using System.Collections.Generic;

namespace App.Infra.Data.Repository.Orderes
{
    public interface IOrderRepository : IRepositoryBase<Order>
	{
		Order GetById(int id);

        IEnumerable<Order> GetByCustomerId(int customerId);

        IEnumerable<Order> PagedList(Paging page);

		IEnumerable<Order> PagedSearchList(SortingPagingBuilder sortBuider, Paging page);
	}
}