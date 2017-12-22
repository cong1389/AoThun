using App.Core.Caching;
using App.Core.Utils;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.SeoSetting;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Service.SeoSetting
{
	public class SettingSeoGlobalService : BaseService<SettingSeoGlobal>, ISettingSeoGlobalService, IBaseService<SettingSeoGlobal>, IService
	{
        private const string CACHE_SETTINGSEOGLOBAL_KEY = "db.SettingSeoGlobal.{0}";
        private readonly ICacheManager _cacheManager;

        private readonly ISettingSeoGlobalRepository _SettingSeoGlobalRepository;

		private readonly IUnitOfWork _unitOfWork;

		public SettingSeoGlobalService(IUnitOfWork unitOfWork, ISettingSeoGlobalRepository SettingSeoGlobalRepository
            , ICacheManager cacheManager) : base(unitOfWork, SettingSeoGlobalRepository)
		{
			this._unitOfWork = unitOfWork;
			this._SettingSeoGlobalRepository = SettingSeoGlobalRepository;
            _cacheManager = cacheManager;

        }

		public SettingSeoGlobal GetById(int id)
		{
            StringBuilder sbKey = new StringBuilder();
            sbKey.AppendFormat(CACHE_SETTINGSEOGLOBAL_KEY, "GetById");
            sbKey.Append(id);

            string key = sbKey.ToString();
            SettingSeoGlobal settingSeoGlobal = _cacheManager.Get<SettingSeoGlobal>(key);
            if (settingSeoGlobal == null)
            {
                settingSeoGlobal = _SettingSeoGlobalRepository.GetById(id);
                _cacheManager.Put(key, settingSeoGlobal);
            }

            return settingSeoGlobal;
		}

		public IEnumerable<SettingSeoGlobal> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
		{
			return this._SettingSeoGlobalRepository.PagedSearchList(sortbuBuilder, page);
		}
	}
}