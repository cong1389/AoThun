using App.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Orders
{
    public interface IOrderTotalCalculationService
    {
        decimal GetCurrentCartSubTotal(IOrderedEnumerable<ShoppingCartItem> cart);
    }
}
