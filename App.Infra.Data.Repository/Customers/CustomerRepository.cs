using App.Core.Utils;
using App.Domain.Interfaces.Repository;
using App.Infra.Data.Common;
using App.Infra.Data.DbFactory;
using Domain.Entities.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace App.Infra.Data.Repository.Customers
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository, IRepositoryBase<Customer>
	{
		public CustomerRepository(IDbFactory dbFactory) : base(dbFactory)
		{
		}

		public Customer GetById(int id)
		{
			Customer province = this.FindBy((Customer x) => x.Id == id, false).FirstOrDefault<Customer>();
			return province;
		}

		protected override IOrderedQueryable<Customer> GetDefaultOrder(IQueryable<Customer> query)
		{
			IOrderedQueryable<Customer> Customer = 
				from p in query
				orderby p.Id
				select p;
			return Customer;
		}

		public IEnumerable<Customer> PagedList(Paging page)
		{
			return this.GetAllPagedList(page).ToList<Customer>();
		}

		public IEnumerable<Customer> PagedSearchList(SortingPagingBuilder sortBuider, Paging page)
		{
			Expression<Func<Customer, bool>> expression = PredicateBuilder.True<Customer>();
			if (!string.IsNullOrEmpty(sortBuider.Keywords))
			{
				expression = expression.And<Customer>((Customer x) => x.Username.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.Email.ToLower().Contains(sortBuider.Keywords.ToLower()));
			}
			return this.FindAndSort(expression, sortBuider.Sorts, page);
		}
	}
}