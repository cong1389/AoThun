using App.Core.Utils;
using App.Domain.Entities.Brandes;
using App.Domain.Interfaces.Repository;
using Domain.Entities.Customers;
using System;
using System.Collections.Generic;

namespace App.Infra.Data.Repository.Customers
{
	public interface ICustomerRepository : IRepositoryBase<Customer>
	{
		Customer GetById(int id);

		IEnumerable<Customer> PagedList(Paging page);

		IEnumerable<Customer> PagedSearchList(SortingPagingBuilder sortBuider, Paging page);
	}
}