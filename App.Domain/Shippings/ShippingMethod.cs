using App.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Shippings
{
    public class ShippingMethod: AuditableEntity<int>
    {
        public string Name
        {
            get;set;
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

        public ShippingMethod()
        {
        }
    }
}
