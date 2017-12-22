using App.Core.Caching;
using App.Core.Utils;
using App.Domain.Entities.GenericControl;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.GenericControl;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace App.Service.GenericControl
{
    public class GenericControlValueItemService : BaseService<GenericControlValueItem>, IGenericControlValueItemService, IBaseService<GenericControlValueItem>, IService
    {
        private const string CACHE_GENERICCONTROLVALUEITEM_KEY = "db.GenericControlValueItem.{0}";
        private readonly ICacheManager _cacheManager;

        private readonly IGenericControlValueItemRepository _genericControlValueItemRepository;

        private readonly IUnitOfWork _unitOfWork;

        public GenericControlValueItemService(IUnitOfWork unitOfWork, IGenericControlValueItemRepository genericControlValueItemRepository
            , ICacheManager cacheManager) : base(unitOfWork, genericControlValueItemRepository)
        {
            this._unitOfWork = unitOfWork;
            this._genericControlValueItemRepository = genericControlValueItemRepository;
            _cacheManager = cacheManager;

        }

        public GenericControlValueItem GetById(int id, bool isCache = true)
        {
            GenericControlValueItem genericControlValueItem;

            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_GENERICCONTROLVALUEITEM_KEY, "GetById");
                sbKey.Append(id);

                string key = sbKey.ToString();
                genericControlValueItem = _cacheManager.Get<GenericControlValueItem>(key);
                if (genericControlValueItem == null)
                {
                    genericControlValueItem = _genericControlValueItemRepository.GetById(id);
                    _cacheManager.Put(key, genericControlValueItem);
                }
            }
            else
            {
                genericControlValueItem = _genericControlValueItemRepository.GetById(id);
            }          

            return genericControlValueItem;
        }

        public IEnumerable<GenericControlValueItem> GetByOption(int? genericControlValueId = null
            , int? entityId = null
            , int status = 1
            , bool isCache = true)
        {
            IEnumerable<GenericControlValueItem> genericControlValueItem;
            StringBuilder sbKey = new StringBuilder();
            sbKey.AppendFormat(CACHE_GENERICCONTROLVALUEITEM_KEY, "GetByOption");

            Expression<Func<GenericControlValueItem, bool>> expression = PredicateBuilder.True<GenericControlValueItem>();
            sbKey.AppendFormat("-{0}", status);
            expression = expression.And((GenericControlValueItem x) => x.Status == status);

            if (genericControlValueId != null)
            {
                sbKey.AppendFormat("-{0}", genericControlValueId);
                expression = expression.And((GenericControlValueItem x) => x.GenericControlValueId == genericControlValueId);
            }

            if (entityId != null)
            {
                sbKey.AppendFormat("-{0}", entityId);
                expression = expression.And((GenericControlValueItem x) => x.EntityId == entityId);
            }

            if (isCache)
            {
                string key = sbKey.ToString();
                genericControlValueItem = _cacheManager.GetCollection<GenericControlValueItem>(key);
                if (genericControlValueItem.Any())
                {
                    genericControlValueItem = FindBy(expression, false);
                    _cacheManager.Put(key, genericControlValueItem);
                }
            }
            else
            {
                genericControlValueItem = FindBy(expression, false);
            }


            return genericControlValueItem;
        }
    }
}