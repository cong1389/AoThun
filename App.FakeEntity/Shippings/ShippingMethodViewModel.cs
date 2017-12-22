using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.FakeEntity.Shippings
{
    public class ShippingMethodViewModel
    {
        public string Name
        {
            get; set;
        }
        public string Description
        {
            get; set;
        }
        public int DisplayOrder
        {
            get; set;
        }
        public bool IgnoreCharges
        {
            get; set;
        }

        public ShippingMethodViewModel()
        {
        }
    }
}
