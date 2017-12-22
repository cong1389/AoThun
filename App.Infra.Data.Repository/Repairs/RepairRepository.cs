using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Repository;
using App.Infra.Data.Common;
using App.Infra.Data.DbFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace App.Infra.Data.Repository.Repairs
{
	public class RepairRepository : RepositoryBase<Repair>, IRepairRepository, IRepositoryBase<Repair>
	{
		public RepairRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}

		protected override IOrderedQueryable<Repair> GetDefaultOrder(IQueryable<Repair> query)
		{
			IOrderedQueryable<Repair> orders = 
				from p in query
				orderby p.Id
				select p;
			return orders;
		}

		public IEnumerable<App.Domain.Entities.Data.Repair> PagedList(Paging page)
		{
			return this.GetAllPagedList(page).ToList<App.Domain.Entities.Data.Repair>();
		}

		public IEnumerable<App.Domain.Entities.Data.Repair> PagedSearchList(SortingPagingBuilder sortBuider, Paging page)
		{
			Expression<Func<App.Domain.Entities.Data.Repair, bool>> expression = PredicateBuilder.True<App.Domain.Entities.Data.Repair>();
			if (!string.IsNullOrEmpty(sortBuider.Keywords))
			{
				expression = expression.And<App.Domain.Entities.Data.Repair>((App.Domain.Entities.Data.Repair x) => x.RepairCode.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.CustomerCode.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.CustomerName.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.CreatedDate.ToString("dd/MM/yyyy").ToLower().Contains(sortBuider.Keywords.ToLower()));
			}
			return this.FindAndSort(expression, sortBuider.Sorts, page);
		}
	}
}