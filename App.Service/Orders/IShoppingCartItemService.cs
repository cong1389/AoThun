using App.Core.Utils;
using App.Domain.Entities.Orders;
using App.Domain.Interfaces.Services;
using System.Collections.Generic;
using System.Linq;

namespace App.Service.Orders
{
    public interface IShoppingCartItemService : IBaseService<ShoppingCartItem>, IService
    {
        void AddToCart(AddToCartContext ctx);

        ShoppingCartItem GetById(int id, bool isCache = true);

        IEnumerable<ShoppingCartItem> GetByPostId(int postId, bool isCache = true);

        IEnumerable<ShoppingCartItem> PagedList(SortingPagingBuilder sortBuider, Paging page);

        string GetCurrentCartSubTotal(IOrderedEnumerable<ShoppingCartItem> cart);

        void DeleteShoppingCartItem(ShoppingCartItem shoppingCartItem, bool resetCheckoutData = true,
         bool ensureOnlyActiveCheckoutAttributes = false, bool deleteChildCartItems = true);
    }
}