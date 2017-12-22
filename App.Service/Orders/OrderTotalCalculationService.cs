using App.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Orders
{
    public class OrderTotalCalculationService : IOrderTotalCalculationService
    {
        public decimal GetCurrentCartSubTotal(IOrderedEnumerable<ShoppingCartItem> cart)
        {
            decimal subTotal = decimal.Zero;

            if (cart == null || cart.Count() == 0)
            {
                return subTotal;
            }

            subTotal = (from mul in cart
                        select mul.Quantity * mul.CustomerEnteredPrice).Sum();

            return subTotal;
        }
    }
}
