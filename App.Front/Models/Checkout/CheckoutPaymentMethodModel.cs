using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Front.Models.Checkout
{
    public partial class CheckoutPaymentMethodModel 
    {
        public CheckoutPaymentMethodModel()
        {
            PaymentMethods = new List<PaymentMethodModel>();
        }

        public List<PaymentMethodModel> PaymentMethods { get; set; }

        public bool DisplayRewardPoints { get; set; }
        public int RewardPointsBalance { get; set; }
        public string RewardPointsAmount { get; set; }
        public bool UseRewardPoints { get; set; }
        public bool SkippedSelectShipping { get; set; }

        #region Nested classes

        public partial class PaymentMethodModel 
        {
            public int Id { get; set; }
            public string PaymentMethodSystemName { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string FullDescription { get; set; }
            public string BrandUrl { get; set; }
            public string Fee { get; set; }
            public bool Selected { get; set; }
           
            public bool RequiresInteraction { get; set; }
        }

        #endregion
    }
}