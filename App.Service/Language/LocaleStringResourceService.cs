using App.Core.Caching;
using App.Core.Extensions;
using App.Core.Utils;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.LocaleStringResource;
using App.Infra.Data.UOW.Interfaces;
using App.Service.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Service.LocaleStringResource
{
    public class LocaleStringResourceService : BaseService<App.Domain.Entities.Language.LocaleStringResource>, ILocaleStringResourceService, IBaseService<App.Domain.Entities.Language.LocaleStringResource>, IService
    {
        private const string CACHE_LOCALESTRINGRESOURCE_KEY = "db.LocaleStringResource.{0}";

        private readonly IWorkContext _workContext;

        private readonly ILocaleStringResourceRepository _LocaleStringResourceRepository;

        private readonly IUnitOfWork _unitOfWork;

        private readonly ICacheManager _cacheManager;

        public LocaleStringResourceService(IUnitOfWork unitOfWork, ILocaleStringResourceRepository LocaleStringResourceRepository
            , IWorkContext workContext
            , ICacheManager cacheManager) : base(unitOfWork, LocaleStringResourceRepository)
        {
            this._unitOfWork = unitOfWork;
            this._LocaleStringResourceRepository = LocaleStringResourceRepository;
            this._workContext = workContext;
            _cacheManager = cacheManager;
        }

        public void CreateLocaleStringResource(App.Domain.Entities.Language.LocaleStringResource LocaleStringResource)
        {
            this._LocaleStringResourceRepository.Add(LocaleStringResource);
        }

        public App.Domain.Entities.Language.LocaleStringResource GetByName(int languageId, string resourceName, bool isCache = true)
        {
            App.Domain.Entities.Language.LocaleStringResource locale;

            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_LOCALESTRINGRESOURCE_KEY, "GetByName");
                sbKey.AppendFormat("-{0}", languageId);

                if (resourceName.HasValue())
                    sbKey.AppendFormat("-{0}", resourceName);

                string key = sbKey.ToString();
                locale = _cacheManager.Get<App.Domain.Entities.Language.LocaleStringResource>(key);
                if (locale == null)
                {
                    locale = _LocaleStringResourceRepository.Get((Domain.Entities.Language.LocaleStringResource x)
                    => x.LanguageId == languageId && x.ResourceName == resourceName);
                    _cacheManager.Put(key, locale);
                }
            }
            else
            {
                locale = _LocaleStringResourceRepository.Get((Domain.Entities.Language.LocaleStringResource x)
                   => x.LanguageId == languageId && x.ResourceName == resourceName);
            }

            return locale;
        }

        public IEnumerable<App.Domain.Entities.Language.LocaleStringResource> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
        {
            return this._LocaleStringResourceRepository.PagedSearchList(sortbuBuilder, page);
        }

        public int SaveLocaleStringResource()
        {
            return this._unitOfWork.Commit();
        }

        public App.Domain.Entities.Language.LocaleStringResource GetById(int id, bool isCache = true)
        {
            App.Domain.Entities.Language.LocaleStringResource locale;

            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_LOCALESTRINGRESOURCE_KEY, "GetById");
                sbKey.AppendFormat("-{0}", id);

                string key = sbKey.ToString();
                locale = _cacheManager.Get<App.Domain.Entities.Language.LocaleStringResource>(key);
                if (locale == null)
                {
                    locale = _LocaleStringResourceRepository.GetLocaleStringResourceById(id);
                    _cacheManager.Put(key, locale);
                }
            }
            else
            {
                locale = _LocaleStringResourceRepository.GetLocaleStringResourceById(id);
            }

            //App.Domain.Entities.Language.LocaleStringResource locale = _LocaleStringResourceRepository.GetLocaleStringResourceById(id);

            return locale;
        }

        public IEnumerable<App.Domain.Entities.Language.LocaleStringResource> GetByLanguageId(int languageId, bool isCache = true)
        {
            IEnumerable<App.Domain.Entities.Language.LocaleStringResource> locale;

            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_LOCALESTRINGRESOURCE_KEY, "GetByLanguageId");
                sbKey.AppendFormat("-{0}", languageId);

                string key = sbKey.ToString();
                locale = _cacheManager.GetCollection<App.Domain.Entities.Language.LocaleStringResource>(key);
                if (locale == null)
                {
                    locale = _LocaleStringResourceRepository.FindBy(
                    (App.Domain.Entities.Language.LocaleStringResource x) => x.LanguageId == languageId, false);
                    _cacheManager.Put(key, locale);
                }
            }
            else
            {
                locale = _LocaleStringResourceRepository.FindBy(
                    (App.Domain.Entities.Language.LocaleStringResource x) => x.LanguageId == languageId, false);
            }

            //var locale = _LocaleStringResourceRepository.FindBy(
            //    (App.Domain.Entities.Language.LocaleStringResource x) => x.LanguageId == languageId, false);

            return locale;
        }

        public virtual IQueryable<App.Domain.Entities.Language.LocaleStringResource> GetAll(int languageId)
        {
            var query = from lsr in _LocaleStringResourceRepository.Table
                        orderby lsr.ResourceName
                        where lsr.LanguageId == languageId
                        select lsr;

            return query;
        }

        public virtual string GetResource(
          string resourceKey,
          int languageId = 0,
          bool logIfNotFound = true,
          string defaultValue = "",
          bool returnEmptyIfNotFound = false
            ,bool isCache=true)
        {
            App.Domain.Entities.Language.LocaleStringResource locale;
            string result = string.Empty;

            if (languageId <= 0)
            {
                if (_workContext.WorkingLanguage == null)
                {
                    return defaultValue;
                }

                languageId = _workContext.WorkingLanguage.Id;
            }

            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_LOCALESTRINGRESOURCE_KEY, "GetResource");
                sbKey.AppendFormat("-{0}", languageId);

                if (resourceKey.HasValue())
                    sbKey.AppendFormat("-{0}", resourceKey);

                string key = sbKey.ToString();
                locale = _cacheManager.Get<Domain.Entities.Language.LocaleStringResource>(key);
                if (locale == null)
                {
                    locale = this.GetByName(languageId, resourceKey);
                    _cacheManager.Put(key, locale);
                }
                if (locale != null)
                {
                    result = locale.ResourceValue;
                }
            }
            else
            {
                locale = this.GetByName(languageId, resourceKey);
            }
          

            return result;
        }
    }
}