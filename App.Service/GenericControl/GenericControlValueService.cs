using App.Core.Caching;
using App.Core.Utils;
using App.Domain.Entities.GenericControl;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.GenericControl;
using App.Infra.Data.UOW.Interfaces;
using App.Service.GenericControl;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Service.GenericControl
{
    public class GenericControlValueService : BaseService<GenericControlValue>, IGenericControlValueService, IBaseService<GenericControlValue>, IService
    {
        private const string CACHE_GENERICONTROLVALUE_KEY = "db.GenericControlValue.{0}";
        private readonly ICacheManager _cacheManager;

        private readonly IGenericControlValueRepository _attributeValueRepository;

        private readonly IUnitOfWork _unitOfWork;

        public GenericControlValueService(IUnitOfWork unitOfWork, IGenericControlValueRepository attributeValueRepository
            , ICacheManager cacheManager) : base(unitOfWork, attributeValueRepository)
        {
            this._unitOfWork = unitOfWork;
            this._attributeValueRepository = attributeValueRepository;
            _cacheManager = cacheManager;
        }

        public GenericControlValue GetById(int id, bool isCache = true)
        {
            GenericControlValue genericControlValue;

            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_GENERICONTROLVALUE_KEY, "GetById");
                sbKey.Append(id);

                string key = sbKey.ToString();
                genericControlValue = _cacheManager.Get<GenericControlValue>(key);
                if (genericControlValue == null)
                {
                    genericControlValue = _attributeValueRepository.GetById(id);
                    _cacheManager.Put(key, genericControlValue);
                }
            }
            else
            {
                genericControlValue = _attributeValueRepository.GetById(id);
            }           

            return genericControlValue;
        }

        public IEnumerable<GenericControlValue> GetByEntityId(int entityId)
        {
            StringBuilder sbKey = new StringBuilder();
            sbKey.AppendFormat(CACHE_GENERICONTROLVALUE_KEY, "GetByEntityId");
            sbKey.AppendFormat("-{0}", entityId);

            string key = sbKey.ToString();
            IEnumerable<GenericControlValue> genericControlValues = _cacheManager.GetCollection<GenericControlValue>(key);
            if (genericControlValues == null)
            {
                genericControlValues = _attributeValueRepository.FindBy((GenericControlValue x) => x.EntityId == entityId);
                _cacheManager.Put(key, genericControlValues);
            }

            return genericControlValues;
        }

        public IEnumerable<GenericControlValue> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
        {
            return this._attributeValueRepository.PagedSearchList(sortbuBuilder, page);
        }
    }
}