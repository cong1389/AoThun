using App.Core.Common;
using App.Domain.Entities.Data;
using App.Domain.Entities.GlobalSetting;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace App.FakeEntity.Payments
{
    public class PaymentMethodViewModel
    {
        public int Id
        {
            get;
            set;
        }

        public string PaymentMethodSystemName
        {
            get;
            set;
        }

        public string FullDescription
        {
            get;
            set;
        }

        public PaymentMethodViewModel()
        {
        }
    }
}