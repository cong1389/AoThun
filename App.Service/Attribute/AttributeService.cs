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
    public class AttributeService : BaseService<App.Domain.Entities.Attribute.Attribute>, IAttributeService, IBaseService<App.Domain.Entities.Attribute.Attribute>, IService
    {
        private const string CACHE_ATTRIBUTE_KEY = "db.Attribute.{0}";
        private readonly ICacheManager _cacheManager;

        private readonly IAttributeRepository _attributeRepository;

        private readonly IUnitOfWork _unitOfWork;

        public AttributeService(IUnitOfWork unitOfWork, IAttributeRepository attributeRepository
            , ICacheManager cacheManager) : base(unitOfWork, attributeRepository)
        {
            this._unitOfWork = unitOfWork;
            this._attributeRepository = attributeRepository;
            _cacheManager = cacheManager;
        }

        public App.Domain.Entities.Attribute.Attribute GetById(int id, bool isCache = true)
        {
            App.Domain.Entities.Attribute.Attribute attribute;

            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_ATTRIBUTE_KEY, "GetById");
                sbKey.Append(id);

                string key = sbKey.ToString();
                attribute = _cacheManager.Get<App.Domain.Entities.Attribute.Attribute>(key);
                if (attribute == null)
                {
                    attribute = this._attributeRepository.GetById(id);
                    _cacheManager.Put(key, attribute);
                }
            }
            else
            {
                attribute = this._attributeRepository.GetById(id);
            }

            return attribute;
        }

        public IEnumerable<App.Domain.Entities.Attribute.Attribute> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
        {
            return this._attributeRepository.PagedSearchList(sortbuBuilder, page);
        }
    }
}