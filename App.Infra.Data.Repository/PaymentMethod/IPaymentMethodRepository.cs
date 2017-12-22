using App.Core.Utils;
using App.Domain.Entities.Payments;
using App.Domain.Interfaces.Repository;
using System.Collections.Generic;

namespace App.Infra.Data.Repository.PaymentMethodes
{
    public interface IPaymentMethodRepository : IRepositoryBase<PaymentMethod>
	{
		PaymentMethod GetById(int id);

		IEnumerable<PaymentMethod> PagedList(Paging page);

		IEnumerable<PaymentMethod> PagedSearchList(SortingPagingBuilder sortBuider, Paging page);
	}
}