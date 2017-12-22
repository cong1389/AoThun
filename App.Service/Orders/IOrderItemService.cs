using App.Core.Utils;
using App.Domain.Interfaces.Services;
using App.Domain.Orders;
using System.Collections.Generic;

namespace App.Service.Orders
{
    public interface IOrderItemService : IBaseService<OrderItem>, IService
	{
		OrderItem GetById(int Id);

		IEnumerable<OrderItem> PagedList(SortingPagingBuilder sortBuider, Paging page);
	}
}