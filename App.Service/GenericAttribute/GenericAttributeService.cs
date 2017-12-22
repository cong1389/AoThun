using App.Core.Caching;
using App.Core.Utils;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.GenericAttribute;
using App.Infra.Data.UOW.Interfaces;
using App.Service.GenericAttribute;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.GenericAttribute
{
    public class GenericAttributeService : BaseService<App.Domain.Entities.Data.GenericAttribute>, IGenericAttributeService, IBaseService<App.Domain.Entities.Data.GenericAttribute>, IService
    {
        private const string CACHE_GENERICATTRIBUTE_KEY = "db.GenericAttribute.{0}";
        private readonly ICacheManager _cacheManager;
        private readonly IGenericAttributeRepository _genericAttributeRepository;

        private readonly IUnitOfWork _unitOfWork;

        public GenericAttributeService(IUnitOfWork unitOfWork, IGenericAttributeRepository genericAttributeRepository, ICacheManager cacheManager) : base(unitOfWork, genericAttributeRepository)
        {
            this._unitOfWork = unitOfWork;
            this._genericAttributeRepository = genericAttributeRepository;
            _cacheManager = cacheManager;
        }

        public void CreateGenericAttribute(App.Domain.Entities.Data.GenericAttribute genericAttribute)
        {
            this._genericAttributeRepository.Add(genericAttribute);
        }

        public App.Domain.Entities.Data.GenericAttribute GetById(int id, bool isCache = true)
        {
            Domain.Entities.Data.GenericAttribute genericAttribute;

            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_GENERICATTRIBUTE_KEY, "GetById");
                sbKey.Append(id);

                string key = sbKey.ToString();
                genericAttribute = _cacheManager.Get<Domain.Entities.Data.GenericAttribute>(key);
                if (genericAttribute == null)
                {
                    genericAttribute = _genericAttributeRepository.GetAttributeById(id);
                    _cacheManager.Put(key, genericAttribute);
                }
            }
            else
            {
                genericAttribute = _genericAttributeRepository.GetAttributeById(id);
            }

            return this._genericAttributeRepository.GetAttributeById(id);
        }

        public App.Domain.Entities.Data.GenericAttribute GetByKey(int entityId, string keyGroup, string key, bool isCache = true)
        {
            App.Domain.Entities.Data.GenericAttribute attr;

            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_GENERICATTRIBUTE_KEY, "GetByKey");
                sbKey.Append(entityId);

                if (!string.IsNullOrWhiteSpace(keyGroup))                
                    sbKey.Append(keyGroup);

                if (!string.IsNullOrWhiteSpace(key))
                    sbKey.Append(key);

                string keyCachare = sbKey.ToString();
                attr = _cacheManager.Get<App.Domain.Entities.Data.GenericAttribute>(keyCachare);
                if (attr == null)
                {
                    attr = this._genericAttributeRepository
                 .Get((App.Domain.Entities.Data.GenericAttribute x) =>
                 x.EntityId.Equals(entityId)
                  && x.KeyGroup.Equals(keyGroup)
                  && x.Key.Equals(key)
                 , false);

                    _cacheManager.Put(keyCachare, attr);
                }
            }
            else
            {
                attr = this._genericAttributeRepository
               .Get((App.Domain.Entities.Data.GenericAttribute x) =>
               x.EntityId.Equals(entityId)
                && x.KeyGroup.Equals(keyGroup)
                && x.Key.Equals(key)
               , false);
            }

            //App.Domain.Entities.Data.GenericAttribute attr = this._genericAttributeRepository
            //    .Get((App.Domain.Entities.Data.GenericAttribute x) =>
            //    x.EntityId.Equals(entityId)
            //     && x.KeyGroup.Equals(keyGroup)
            //     && x.Key.Equals(key)
            //    , false);
            return attr;
        }

        public IEnumerable<App.Domain.Entities.Data.GenericAttribute> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
        {
            return this._genericAttributeRepository.PagedSearchList(sortbuBuilder, page);
        }

        public void SaveGenericAttribute(int entityId, string keyGroup, string key, string value, int storeId = 0)
        {
            App.Domain.Entities.Data.GenericAttribute objAttribute = new App.Domain.Entities.Data.GenericAttribute
            {
                EntityId = entityId,
                KeyGroup = keyGroup,
                Key = key,
                Value = value,
                StoreId = storeId
            };

            var attribute = this.GetByKey(entityId, keyGroup, key);

            if (attribute == null)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    Create(objAttribute);
                }
            }
            else
            {
                //Không có value thì delete
                if (string.IsNullOrWhiteSpace(value))
                {
                    Delete(attribute);
                }
                else
                {
                    attribute.Value = value;
                    Update(attribute);
                }
            }
        }
    }
}