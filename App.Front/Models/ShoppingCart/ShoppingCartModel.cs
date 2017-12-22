using App.Domain.Entities.Data;
using App.Domain.Entities.Orders;
using App.FakeEntity.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Front.Models.ShoppingCart
{
    public class ShoppingCartModel
    {
        public ShoppingCartModel()
        {
            Items = new List<Post>();
            ShoppingCarts = new List<ShoppingCartItem>();
            OrderReviewData = new OrderReviewDataModel();
        }

        public IEnumerable<Post> Items { get; set; }

        public IEnumerable<ShoppingCartItem> ShoppingCarts { get; set; }

        public OrderReviewDataModel OrderReviewData { get; set; }
    }

    public partial class OrderReviewDataModel 
    {
        public OrderReviewDataModel()
        {
            this.BillingAddress = new AddressViewModel();
            this.ShippingAddress = new AddressViewModel();
        }
        public bool Display { get; set; }        

        public AddressViewModel BillingAddress { get; set; }

        public bool IsShippable { get; set; }
        public AddressViewModel ShippingAddress { get; set; }
        public string ShippingMethod { get; set; }

        public string PaymentMethod { get; set; }
        public string PaymentSummary { get; set; }

        public bool IsPaymentSelectionSkipped { get; set; }
    }
}