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
    public class GenericControlService : BaseService<App.Domain.Entities.GenericControl.GenericControl>, IGenericControlService, IBaseService<App.Domain.Entities.GenericControl.GenericControl>, IService
    {
        private const string CACHE_GENERICCONTROL_KEY = "db.GenericControl.{0}";
        private readonly ICacheManager _cacheManager;

        private readonly IGenericControlRepository _attributeRepository;

        private readonly IUnitOfWork _unitOfWork;

        public GenericControlService(IUnitOfWork unitOfWork, IGenericControlRepository attributeRepository
            , ICacheManager cacheManager) : base(unitOfWork, attributeRepository)
        {
            this._unitOfWork = unitOfWork;
            this._attributeRepository = attributeRepository;
            _cacheManager = cacheManager;
        }

        public App.Domain.Entities.GenericControl.GenericControl GetById(int id, bool isCache = true)
        {
            Domain.Entities.GenericControl.GenericControl genericControl;

            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_GENERICCONTROL_KEY, "GetById");
                sbKey.Append(id);

                string key = sbKey.ToString();
                genericControl = _cacheManager.Get<Domain.Entities.GenericControl.GenericControl>(key);
                if (genericControl == null)
                {
                    genericControl = _attributeRepository.GetById(id);
                    _cacheManager.Put(key, genericControl);
                }
            }
            else
            {
                genericControl = _attributeRepository.GetById(id);
            }

            //Domain.Entities.GenericControl.GenericControl genericControl = _attributeRepository.GetById(id);
            return genericControl;
        }

        public IEnumerable<App.Domain.Entities.GenericControl.GenericControl> GetByMenuId(int menuId, bool isCache = true)
        {
            IEnumerable<App.Domain.Entities.GenericControl.GenericControl> genericControls;

            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_GENERICCONTROL_KEY, "GetByMenuId");
                sbKey.AppendFormat("-{0}", menuId);

                string key = sbKey.ToString();
                genericControls = _cacheManager.GetCollection<App.Domain.Entities.GenericControl.GenericControl>(key);
                if (genericControls == null)
                {
                    genericControls = _attributeRepository.FindBy((Domain.Entities.GenericControl.GenericControl x) => x.MenuId == menuId);
                    _cacheManager.Put(key, genericControls);
                }
            }
            else
            {
                genericControls = _attributeRepository.FindBy((Domain.Entities.GenericControl.GenericControl x) => x.MenuId == menuId);
            }

            return genericControls;
        }

        public IEnumerable<App.Domain.Entities.GenericControl.GenericControl> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
        {
            return this._attributeRepository.PagedSearchList(sortbuBuilder, page);
        }
    }
}