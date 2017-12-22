using App.Core.Caching;
using App.Core.Common;
using App.Core.Extensions;
using App.Core.Utils;
using App.Domain.Entities.Language;
using App.Domain.Interfaces.Repository;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Language;
using App.Infra.Data.UOW.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace App.Service.LocalizedProperty
{
    public class LocalizedPropertyService : BaseService<App.Domain.Entities.Language.LocalizedProperty>, ILocalizedPropertyService, IBaseService<App.Domain.Entities.Language.LocalizedProperty>, IService
    {
        private const string CACHE_LOCALIZEDPROPERTY_KEY = "db.LocalizedProperty.{0}";

        private readonly ICacheManager _cacheManager;

        private readonly ILocalizedPropertyRepository _localizedPropertyRepository;

        private readonly IUnitOfWork _unitOfWork;

        public LocalizedPropertyService(IUnitOfWork unitOfWork, ILocalizedPropertyRepository LocalizedPropertyRepository, ICacheManager cacheManager) : base(unitOfWork, LocalizedPropertyRepository)
        {
            this._unitOfWork = unitOfWork;
            this._localizedPropertyRepository = LocalizedPropertyRepository;
            _cacheManager = cacheManager;
        }

        public void CreateLocalizedProperty(App.Domain.Entities.Language.LocalizedProperty LocalizedProperty)
        {
            this._localizedPropertyRepository.Add(LocalizedProperty);
        }

        public App.Domain.Entities.Language.LocalizedProperty GetById(int id, bool isCache = true)
        {
            Domain.Entities.Language.LocalizedProperty localizedProperty;

            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_LOCALIZEDPROPERTY_KEY, "GetById");
                sbKey.Append(id);

                string key = sbKey.ToString();
                localizedProperty = _cacheManager.Get<Domain.Entities.Language.LocalizedProperty>(key);
                if (localizedProperty == null)
                {
                    localizedProperty = _localizedPropertyRepository.GetId(id);
                    _cacheManager.Put(key, localizedProperty);
                }
            }
            else
            {
                localizedProperty = _localizedPropertyRepository.GetId(id);
            }

            return localizedProperty;
        }

        public IEnumerable<App.Domain.Entities.Language.LocalizedProperty> GetByEntityId(int entityId, bool isCache = true)
        {
            IEnumerable<Domain.Entities.Language.LocalizedProperty> localizedProperty;

            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_LOCALIZEDPROPERTY_KEY, "GetByEntityId");
                sbKey.Append(entityId);

                string key = sbKey.ToString();
                localizedProperty = _cacheManager.GetCollection<Domain.Entities.Language.LocalizedProperty>(key);
                if (localizedProperty == null)
                {
                    localizedProperty = this._localizedPropertyRepository.FindBy((App.Domain.Entities.Language.LocalizedProperty x) => x.EntityId == entityId, false);
                    _cacheManager.Put(key, localizedProperty);
                }
            }
            else
            {
                localizedProperty = this._localizedPropertyRepository.FindBy((App.Domain.Entities.Language.LocalizedProperty x) => x.EntityId == entityId, false);
            }


            return localizedProperty;
        }

        public App.Domain.Entities.Language.LocalizedProperty GetByKey(int languageId, int entityId, string localeKeyGroup, string localeKey, bool isCache = true)
        {
            Domain.Entities.Language.LocalizedProperty attr;

            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_LOCALIZEDPROPERTY_KEY, "GetByKey");
                sbKey.Append(languageId);
                sbKey.Append(entityId);

                if (localeKeyGroup.HasValue())
                    sbKey.AppendFormat("-{0}", localeKeyGroup);
                if (localeKey.HasValue())
                    sbKey.AppendFormat("-{0}", localeKey);

                string key = sbKey.ToString();
                attr = _cacheManager.Get<App.Domain.Entities.Language.LocalizedProperty>(key);
                if (attr == null)
                {
                    attr = Get((App.Domain.Entities.Language.LocalizedProperty x) =>
                      x.LanguageId.Equals(languageId)
                      && x.EntityId.Equals(entityId)
                       && x.LocaleKeyGroup.Equals(localeKeyGroup)
                       && x.LocaleKey.Equals(localeKey)
                      , false);
                    _cacheManager.Put(key, attr);
                }
            }
            else
            {
                attr = Get((App.Domain.Entities.Language.LocalizedProperty x) =>
                      x.LanguageId.Equals(languageId)
                      && x.EntityId.Equals(entityId)
                       && x.LocaleKeyGroup.Equals(localeKeyGroup)
                       && x.LocaleKey.Equals(localeKey)
                      , false);
            }

            return attr;
        }

        public IEnumerable<App.Domain.Entities.Language.LocalizedProperty> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
        {
            return this._localizedPropertyRepository.PagedSearchList(sortbuBuilder, page);
        }

        public int SaveLocalizedProperty()
        {
            return this._unitOfWork.Commit();
        }

        public virtual void SaveLocalizedValue<T>(
            T entity,
            Expression<Func<T, string>> keySelector,
            string localeValue,
            int languageId) where T : AuditableEntity<int>
        {
            SaveLocalizedValueItem<T, string>(entity, keySelector, localeValue, languageId);
        }

        public virtual void SaveLocalizedValueItem<T, TPropType>(
           T entity,
           Expression<Func<T, TPropType>> keySelector,
           string localeValue,
           int languageId) where T : AuditableEntity<int>
        {
            var member = keySelector.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException($"Expression '{keySelector}' refers to a method, not a property.");
            }

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException($"Expression '{keySelector}' refers to a field, not a property.");
            }

            var keyGroup = typeof(T).Name;
            var key = propInfo.Name;

            App.Domain.Entities.Language.LocalizedProperty obj = this.GetByKey(languageId, entity.Id
           , keyGroup, key);

            if (obj == null)
            {
                if (!string.IsNullOrEmpty(localeValue))
                {
                    obj = new App.Domain.Entities.Language.LocalizedProperty
                    {
                        EntityId = entity.Id,
                        LanguageId = languageId,
                        LocaleKey = key,
                        LocaleKeyGroup = keyGroup,
                        LocaleValue = localeValue
                    };
                    this.Create(obj);
                }
            }
            else
            {
                obj.Id = obj.Id;
                obj.EntityId = entity.Id;
                obj.LanguageId = languageId;
                obj.LocaleKey = key;
                obj.LocaleValue = localeValue;
                this.Update(obj);
            }
        }

    }
}