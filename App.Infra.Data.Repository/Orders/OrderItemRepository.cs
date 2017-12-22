using App.Core.Utils;
using App.Domain.Interfaces.Repository;
using App.Domain.Orders;
using App.Infra.Data.Common;
using App.Infra.Data.DbFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace App.Infra.Data.Repository.Orders
{
    public class OrderItemRepository : RepositoryBase<OrderItem>, IOrderItemRepository, IRepositoryBase<OrderItem>
	{
		public OrderItemRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}

		public OrderItem GetById(int id)
		{
			OrderItem province = this.FindBy((OrderItem x) => x.Id == id, false).FirstOrDefault<OrderItem>();
			return province;
		}

		protected override IOrderedQueryable<OrderItem> GetDefaultOrder(IQueryable<OrderItem> query)
		{
			IOrderedQueryable<OrderItem> OrderItem = 
				from p in query
				orderby p.Id
				select p;
			return OrderItem;
		}

		public IEnumerable<OrderItem> PagedList(Paging page)
		{
			return this.GetAllPagedList(page).ToList<OrderItem>();
		}

		public IEnumerable<OrderItem> PagedSearchList(SortingPagingBuilder sortBuider, Paging page)
		{
			Expression<Func<OrderItem, bool>> expression = PredicateBuilder.True<OrderItem>();
			if (!string.IsNullOrEmpty(sortBuider.Keywords))
			{
                expression = expression.And((OrderItem x) => x.Id.ToString().ToLower().Contains(sortBuider.Keywords.ToLower()));
            }
			return this.FindAndSort(expression, sortBuider.Sorts, page);
		}
	}
}