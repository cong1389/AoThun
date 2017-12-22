using App.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Post
{
    public class PriceCalculationService : IPriceCalculationService
    {
        public virtual decimal GetSubTotal(ShoppingCartItem shoppingCartItem, bool includeDiscounts)
        {
            return shoppingCartItem.CustomerEnteredPrice * shoppingCartItem.Quantity;
        }
    }
}
