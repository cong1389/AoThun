using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Services;
using System.Collections.Generic;

namespace App.Service.Repairs
{
    public interface IRepairService : IBaseService<App.Domain.Entities.Data.Repair>, IService
    {
        IEnumerable<App.Domain.Entities.Data.Repair> PagedList(SortingPagingBuilder sortBuider, Paging page);
    }
}