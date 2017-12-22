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
    public class BannerService : BaseService<Banner>, IBannerService, IBaseService<Banner>, IService
    {
        private const string CACHE_BANNER_KEY = "db.Banner.{0}";
        private readonly ICacheManager _cacheManager;

        private readonly IBannerRepository _bannerRepository;

        private readonly IUnitOfWork _unitOfWork;

        public BannerService(IUnitOfWork unitOfWork, IBannerRepository bannerRepository, ICacheManager cacheManager) : base(unitOfWork, bannerRepository)
        {
            this._unitOfWork = unitOfWork;
            this._bannerRepository = bannerRepository;
            _cacheManager = cacheManager;
        }

        public Banner GetById(int id, bool isCache = true)
        {
            Banner banner;

            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_BANNER_KEY, "GetById");
                sbKey.Append(id);

                string key = sbKey.ToString();
                banner = _cacheManager.Get<Banner>(key);
                if (banner == null)
                {
                    banner = _bannerRepository.GetById(id);
                    _cacheManager.Put(key, banner);
                }
            }
            else
            {
                banner = _bannerRepository.GetById(id);
            }            

            return banner;
        }

        public IEnumerable<Banner> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
        {
            return this._bannerRepository.PagedSearchList(sortbuBuilder, page);
        }
    }
}