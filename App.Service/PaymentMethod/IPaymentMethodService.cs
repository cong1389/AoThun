using App.Core.Utils;
using App.Domain.Entities.Payments;
using App.Domain.Interfaces.Services;
using System.Collections.Generic;

namespace App.Service.PaymentMethodes
{
    public interface IPaymentMethodService : IBaseService<PaymentMethod>, IService
	{
		PaymentMethod GetById(int Id);

        PaymentMethod GetBySystemName(string systemName);

        IEnumerable<PaymentMethod> PagedList(SortingPagingBuilder sortBuider, Paging page);
	}
}