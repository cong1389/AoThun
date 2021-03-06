using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.Language;
using App.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace App.Service.LocaleStringResource
{
    public interface ILocaleStringResourceService : IBaseService<App.Domain.Entities.Language.LocaleStringResource>, IService
    {
        void CreateLocaleStringResource(App.Domain.Entities.Language.LocaleStringResource LocaleStringResource);

        App.Domain.Entities.Language.LocaleStringResource GetByName(int languageId, string resourceName, bool isCache = true);

        IEnumerable<App.Domain.Entities.Language.LocaleStringResource> PagedList(SortingPagingBuilder sortBuider, Paging page);

        int SaveLocaleStringResource();

        App.Domain.Entities.Language.LocaleStringResource GetById(int id, bool isCache = true);

        IEnumerable<App.Domain.Entities.Language.LocaleStringResource> GetByLanguageId(int languageId, bool isCache = true);

        IQueryable<Domain.Entities.Language.LocaleStringResource> GetAll(int languageId);

        string GetResource(string resourceKey, int languageId = 0, bool logIfNotFound = true, string defaultValue = "", bool returnEmptyIfNotFound = false, bool isCache = true);

    }
}