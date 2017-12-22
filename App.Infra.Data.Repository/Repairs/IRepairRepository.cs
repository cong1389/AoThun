using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Repository;
using System.Collections.Generic;

namespace App.Infra.Data.Repository.Repairs
{
	public interface IRepairRepository : IRepositoryBase<Repair>
	{
		IEnumerable<App.Domain.Entities.Data.Repair> PagedList(Paging page);

		IEnumerable<App.Domain.Entities.Data.Repair> PagedSearchList(SortingPagingBuilder sortBuider, Paging page);
	}
}