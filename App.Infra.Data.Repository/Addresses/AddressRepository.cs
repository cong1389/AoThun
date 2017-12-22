using App.Core.Caching;
using App.Core.Utils;
using App.Domain.Common;
using App.Domain.Interfaces.Repository;
using App.Infra.Data.Common;
using App.Infra.Data.DbFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace App.Infra.Data.Repository.Addresses
{
    public class AddressRepository : RepositoryBase<Address>, IAddressRepository, IRepositoryBase<Address>
	{
        public AddressRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

		public Address GetById(int id)
		{
            Address address = this.FindBy((Address x) => x.Id == id, false).FirstOrDefault<Address>();

            return address;
		}

		protected override IOrderedQueryable<Address> GetDefaultOrder(IQueryable<Address> query)
		{
			IOrderedQueryable<Address> Address = 
				from p in query
				orderby p.Id
				select p;
			return Address;
		}

		public IEnumerable<Address> PagedList(Paging page)
		{
			return this.GetAllPagedList(page).ToList<Address>();
		}

		public IEnumerable<Address> PagedSearchList(SortingPagingBuilder sortBuider, Paging page)
		{
			Expression<Func<Address, bool>> expression = PredicateBuilder.True<Address>();
			if (!string.IsNullOrEmpty(sortBuider.Keywords))
			{
				expression = expression.And<Address>((Address x) => x.Title.ToLower().Contains(sortBuider.Keywords.ToLower()) || x.Address1.ToLower().Contains(sortBuider.Keywords.ToLower()));
			}
			return this.FindAndSort(expression, sortBuider.Sorts, page);
		}
	}
}