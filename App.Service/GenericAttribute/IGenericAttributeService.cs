using App.Core.Utils;
using App.Domain.Interfaces.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Service.GenericAttribute
{
    public interface IGenericAttributeService : IBaseService<App.Domain.Entities.Data.GenericAttribute>, IService
    {
        void CreateGenericAttribute(App.Domain.Entities.Data.GenericAttribute attribute);

        App.Domain.Entities.Data.GenericAttribute GetById(int id, bool isCache = true);

        App.Domain.Entities.Data.GenericAttribute GetByKey(int entityId, string keyGroup, string key, bool isCache = true);

        IEnumerable<App.Domain.Entities.Data.GenericAttribute> PagedList(SortingPagingBuilder sortBuider, Paging page);

        void SaveGenericAttribute(int entityId, string keyGroup, string key, string value, int storeId = 0);
    }
}