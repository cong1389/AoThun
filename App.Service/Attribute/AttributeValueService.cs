using App.Core.Caching;
using App.Core.Utils;
using App.Domain.Entities.Attribute;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Attribute;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Service.Attribute
{
    public class AttributeValueService : BaseService<AttributeValue>, IAttributeValueService, IBaseService<AttributeValue>, IService
    {
        private const string CACHE_ATTRIBUTEVALUE_KEY = "db.AttributeValue.{0}";
        private readonly ICacheManager _cacheManager;

        private readonly IAttributeValueRepository _attributeValueRepository;

        private readonly IUnitOfWork _unitOfWork;

        public AttributeValueService(IUnitOfWork unitOfWork, IAttributeValueRepository attributeValueRepository
            , ICacheManager cacheManager) : base(unitOfWork, attributeValueRepository)
        {
            this._unitOfWork = unitOfWork;
            this._attributeValueRepository = attributeValueRepository;
            _cacheManager = cacheManager;
        }

        public AttributeValue GetById(int id, bool isCache = true)
        {
            AttributeValue attributeValue;

            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_ATTRIBUTEVALUE_KEY, "GetById");
                sbKey.Append(id);

                string key = sbKey.ToString();
                attributeValue = _cacheManager.Get<AttributeValue>(key);
                if (attributeValue == null)
                {
                    attributeValue = _attributeValueRepository.GetById(id);
                    _cacheManager.Put(key, attributeValue);
                }
            }
            else
            {
                attributeValue = _attributeValueRepository.GetById(id);
            }

            return attributeValue;
        }

        public IEnumerable<AttributeValue> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
        {
            return this._attributeValueRepository.PagedSearchList(sortbuBuilder, page);
        }
    }
}