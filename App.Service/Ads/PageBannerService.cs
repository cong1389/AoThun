using App.Core.Caching;
using App.Core.Utils;
using App.Domain.Entities.Ads;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Ads;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Service.Ads
{
	public class PageBannerService : BaseService<PageBanner>, IPageBannerService, IBaseService<PageBanner>, IService
	{
        private const string CACHE_PAGEBANNER_KEY = "db.PageBanner.{0}";
        private readonly ICacheManager _cacheManager;

        private readonly IPageBannerRepository _pageBannerRepository;

		private readonly IUnitOfWork _unitOfWork;

		public PageBannerService(IUnitOfWork unitOfWork, IPageBannerRepository pageBannerRepository
            , ICacheManager cacheManager) : base(unitOfWork, pageBannerRepository)
		{
			this._unitOfWork = unitOfWork;
			this._pageBannerRepository = pageBannerRepository;
            _cacheManager = cacheManager;
        }

		public PageBanner GetById(int id,bool isCache = true)
        {
            PageBanner pageBanner;

            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_PAGEBANNER_KEY, "GetById");
                sbKey.Append(id);

                string key = sbKey.ToString();
                pageBanner = _cacheManager.Get<PageBanner>(key);
                if (pageBanner == null)
                {
                    pageBanner = _pageBannerRepository.GetById(id);
                    _cacheManager.Put(key, pageBanner);
                }
            }
            else
            {
                pageBanner = _pageBannerRepository.GetById(id);
            }           

            return pageBanner;
		}

		public IEnumerable<PageBanner> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
		{
			return this._pageBannerRepository.PagedSearchList(sortbuBuilder, page);
		}
	}
}