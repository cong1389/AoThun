using App.Domain.Common;
using App.Domain.Entities.Orders;
using App.FakeEntity.Common;
using App.Front.Models.ShoppingCart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Front.Models.Checkout
{
    public class CheckoutBillingAddressModel
    {
        public CheckoutBillingAddressModel()
        {
            ExistingAddresses = new List<AddressViewModel>();
            NewAddress = new FakeEntity.Common.AddressViewModel();
        }

        public IList<FakeEntity.Common.AddressViewModel> ExistingAddresses { get; set; }

        public AddressViewModel NewAddress { get; set; }

        public MiniShoppingCartModel MiniShoppingCart { get; set; }

        /// <summary>
        /// Used on one-page checkout page
        /// </summary>
        public bool NewAddressPreselected { get; set; }
    }
}