using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Entities.Orders;
using App.Domain.Interfaces.Repository;
using System.Collections.Generic;

namespace App.Infra.Data.Repository.Orders
{
	public interface IShoppingCartItemRepository : IRepositoryBase<ShoppingCartItem>
	{
        ShoppingCartItem GetById(int id);

        IEnumerable<ShoppingCartItem> PagedList(Paging page);

		IEnumerable<ShoppingCartItem> PagedSearchList(SortingPagingBuilder sortBuider, Paging page);
	}
}