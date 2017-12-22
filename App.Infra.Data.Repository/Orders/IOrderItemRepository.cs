using App.Core.Utils;
using App.Domain.Interfaces.Repository;
using App.Domain.Orders;
using System.Collections.Generic;

namespace App.Infra.Data.Repository.Orders
{
    public interface IOrderItemRepository : IRepositoryBase<OrderItem>
	{
		OrderItem GetById(int id);

		IEnumerable<OrderItem> PagedList(Paging page);

		IEnumerable<OrderItem> PagedSearchList(SortingPagingBuilder sortBuider, Paging page);
	}
}