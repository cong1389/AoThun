using App.Domain.Entities.Orders;
using Domain.Entities.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Orders
{
    public class AddToCartContext
    {
        public AddToCartContext()
        {
        }

        public App.Domain.Entities.Data.Post Post { get; set; }

        public Customer Customers { get; set; }

        public ShoppingCartItem ShoppingCart { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

    }
}
