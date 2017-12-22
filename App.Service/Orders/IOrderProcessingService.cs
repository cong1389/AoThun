using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Orders
{
   public interface IOrderProcessingService
    {
        PlaceOrderResult PlaceOrder(ProcessPaymentRequest processPaymentRequest, Dictionary<string, string> extraData);
    }
}
