using App.Core.Caching;
using App.Core.Utils;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.System;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Service.SystemApp
{
    public class SystemSettingService : BaseService<SystemSetting>, ISystemSettingService, IBaseService<SystemSetting>, IService
    {
        private const string CACHE_SYSTEMSETTING_KEY = "db.SystemSetting.{0}";
        private readonly ICacheManager _cacheManager;

        private readonly ISystemSettingRepository _systemSettingRepository;

        private readonly IUnitOfWork _unitOfWork;

        public SystemSettingService(IUnitOfWork unitOfWork, ISystemSettingRepository systemSettingRepository
            , ICacheManager cacheManager) : base(unitOfWork, systemSettingRepository)
        {
            this._unitOfWork = unitOfWork;
            this._systemSettingRepository = systemSettingRepository;
            _cacheManager = cacheManager;
        }

        public SystemSetting GetById(int id, bool isCache = true)
        {
            StringBuilder sbKey = new StringBuilder();
            sbKey.AppendFormat(CACHE_SYSTEMSETTING_KEY, "GetById");
            sbKey.Append(id);

            string key = sbKey.ToString();
            SystemSetting systemSetting ;
            if (isCache)
            {
                systemSetting = _cacheManager.Get<SystemSetting>(key);
                if (systemSetting == null)
                {
                    systemSetting = _systemSettingRepository.GetById(id);
                    _cacheManager.Put(key, systemSetting);
                }
            }
            else
            {
                systemSetting = _systemSettingRepository.GetById(id);
            }           

            return systemSetting; 
        }

        public IEnumerable<SystemSetting> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
        {
            return this._systemSettingRepository.PagedSearchList(sortbuBuilder, page);
        }
    }
}
