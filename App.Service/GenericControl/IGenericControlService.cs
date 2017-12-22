using App.Core.Utils;
using App.Domain.Entities.GenericControl;
using App.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace App.Service.GenericControl
{
    public interface IGenericControlService : IBaseService<App.Domain.Entities.GenericControl.GenericControl>, IService
    {
        Domain.Entities.GenericControl.GenericControl GetById(int id, bool isCache = true);

        IEnumerable<App.Domain.Entities.GenericControl.GenericControl> GetByMenuId(int menuId, bool isCache = true);

        IEnumerable<Domain.Entities.GenericControl.GenericControl> PagedList(SortingPagingBuilder sortBuider, Paging page);
    }
}