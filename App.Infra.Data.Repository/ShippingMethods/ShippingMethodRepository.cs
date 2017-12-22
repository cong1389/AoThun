using App.Core.Utils;
using App.Domain.Interfaces.Repository;
using App.Domain.Shippings;
using App.Infra.Data.Common;
using App.Infra.Data.DbFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace App.Infra.Data.Repository.ShippingMethods
{
    public class ShippingMethodRepository : RepositoryBase<ShippingMethod>, IShippingMethodRepository, IRepositoryBase<ShippingMethod>
	{
		public ShippingMethodRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}

		public ShippingMethod GetById(int id)
		{
			ShippingMethod province = this.FindBy((ShippingMethod x) => x.Id == id, false).FirstOrDefault<ShippingMethod>();
			return province;
		}

		protected override IOrderedQueryable<ShippingMethod> GetDefaultOrder(IQueryable<ShippingMethod> query)
		{
			IOrderedQueryable<ShippingMethod> ShippingMethod = 
				from p in query
				orderby p.Id
				select p;
			return ShippingMethod;
		}

		public IEnumerable<ShippingMethod> PagedList(Paging page)
		{
			return this.GetAllPagedList(page).ToList<ShippingMethod>();
		}

		public IEnumerable<ShippingMethod> PagedSearchList(SortingPagingBuilder sortBuider, Paging page)
		{
			Expression<Func<ShippingMethod, bool>> expression = PredicateBuilder.True<ShippingMethod>();
			if (!string.IsNullOrEmpty(sortBuider.Keywords))
			{
				expression = expression.And<ShippingMethod>((ShippingMethod x) => x.Name.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.Description.ToLower().Contains(sortBuider.Keywords.ToLower()));
			}
			return this.FindAndSort(expression, sortBuider.Sorts, page);
		}
	}
}