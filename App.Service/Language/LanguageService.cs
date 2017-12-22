using App.Core.Caching;
using App.Core.Utils;
using App.Domain.Entities.Language;
using App.Domain.Interfaces.Repository;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Language;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Service.Language
{
    public class LanguageService : BaseService<App.Domain.Entities.Language.Language>, ILanguageService, IBaseService<App.Domain.Entities.Language.Language>, IService
    {
        private const string CACHE_LANGUAGE_KEY = "db.Language.{0}";
        private readonly ICacheManager _cacheManager;

        private readonly ILanguageRepository _languageRepository;

        private readonly IUnitOfWork _unitOfWork;

        public LanguageService(IUnitOfWork unitOfWork
            , ILanguageRepository languageRepository
            , ICacheManager cacheManager) : base(unitOfWork, languageRepository)
        {
            this._unitOfWork = unitOfWork;
            this._languageRepository = languageRepository;
            _cacheManager = cacheManager;
        }

        public void CreateLanguage(App.Domain.Entities.Language.Language language)
        {
            this._languageRepository.Add(language);
        }

        public App.Domain.Entities.Language.Language GetById(int id, bool isCache = true)
        {
            Domain.Entities.Language.Language language;

            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_LANGUAGE_KEY, "GetById");
                sbKey.Append(id);

                string key = sbKey.ToString();
                language = _cacheManager.Get<Domain.Entities.Language.Language>(key);
                if (language == null)
                {
                    language = _languageRepository.GetById(id);
                    _cacheManager.Put(key, language);
                }
            }
            else
            {
                language = _languageRepository.GetById(id);
            }


            // return this._languageRepository.GetById(id);
            return language;
        }

        public IEnumerable<App.Domain.Entities.Language.Language> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
        {
            return this._languageRepository.PagedSearchList(sortbuBuilder, page);
        }

        public int SaveLanguage()
        {
            return this._unitOfWork.Commit();
        }
    }
}