using App.Core.Utils;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace App.Service.SystemApp
{
    public interface ISystemSettingService : IBaseService<SystemSetting>, IService
    {
        SystemSetting GetById(int id, bool isCache = true);

        IEnumerable<SystemSetting> PagedList(SortingPagingBuilder sortBuider, Paging page);
    }
}
