using App.Core.Common;
using App.Domain.Entities.Data;
using App.Domain.Entities.GlobalSetting;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace App.Domain.Entities.Payments
{
	public class PaymentMethod : AuditableEntity<int>
	{
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

        public PaymentMethod()
		{
		}
	}
}