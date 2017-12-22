using App.Core.Utils;
using App.Domain.Entities.Payments;
using App.Domain.Interfaces.Repository;
using App.Infra.Data.Common;
using App.Infra.Data.DbFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace App.Infra.Data.Repository.PaymentMethodes
{
    public class PaymentMethodRepository : RepositoryBase<PaymentMethod>, IPaymentMethodRepository, IRepositoryBase<PaymentMethod>
	{
		public PaymentMethodRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}

		public PaymentMethod GetById(int id)
		{
			PaymentMethod province = this.FindBy((PaymentMethod x) => x.Id == id, false).FirstOrDefault<PaymentMethod>();
			return province;
		}

		protected override IOrderedQueryable<PaymentMethod> GetDefaultOrder(IQueryable<PaymentMethod> query)
		{
			IOrderedQueryable<PaymentMethod> PaymentMethod = 
				from p in query
				orderby p.Id
				select p;
			return PaymentMethod;
		}

		public IEnumerable<PaymentMethod> PagedList(Paging page)
		{
			return this.GetAllPagedList(page).ToList<PaymentMethod>();
		}

		public IEnumerable<PaymentMethod> PagedSearchList(SortingPagingBuilder sortBuider, Paging page)
		{
			Expression<Func<PaymentMethod, bool>> expression = PredicateBuilder.True<PaymentMethod>();
			if (!string.IsNullOrEmpty(sortBuider.Keywords))
			{
				expression = expression.And<PaymentMethod>((PaymentMethod x) => x.PaymentMethodSystemName.ToLower().Contains(sortBuider.Keywords.ToLower()));
			}
			return this.FindAndSort(expression, sortBuider.Sorts, page);
		}
	}
}