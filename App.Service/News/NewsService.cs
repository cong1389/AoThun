using App.Core.Caching;
using App.Core.Extensions;
using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Repository;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.News;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;

namespace App.Service.News
{
    public class NewsService : BaseService<App.Domain.Entities.Data.News>, INewsService, IBaseService<App.Domain.Entities.Data.News>, IService
    {
        private const string CACHE_NEWS_KEY = "db.News.{0}";
        private readonly ICacheManager _cacheManager;

        private readonly INewsRepository _newsRepository;

        private readonly IUnitOfWork _unitOfWork;

        public NewsService(IUnitOfWork unitOfWork, INewsRepository newsRepository, ICacheManager cacheManager) : base(unitOfWork, newsRepository)
        {
            this._unitOfWork = unitOfWork;
            this._newsRepository = newsRepository;
            _cacheManager = cacheManager;
        }

        public App.Domain.Entities.Data.News GetById(int id, bool isCache = true)
        {
            App.Domain.Entities.Data.News news;
            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_NEWS_KEY, "GetById");
                sbKey.Append(id);

                string key = sbKey.ToString();
                news = _cacheManager.Get<App.Domain.Entities.Data.News>(key);
                if (news == null)
                {
                    news = _newsRepository.GetById(id);
                    _cacheManager.Put(key, news);
                }
            }
            else
            {
                news = _newsRepository.GetById(id);

            }


            //App.Domain.Entities.Data.News news = _newsRepository.GetById(id);
            return news;
        }

        public IEnumerable<App.Domain.Entities.Data.News> GetBySeoUrl(string seoUrl, bool isCache = true)
        {
            IEnumerable<App.Domain.Entities.Data.News> news;
            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_NEWS_KEY, "GetBySeoUrl");

                if (seoUrl.HasValue())
                {
                    sbKey.AppendFormat("-{0}", seoUrl);
                }

                string key = sbKey.ToString();
                news = _cacheManager.GetCollection<App.Domain.Entities.Data.News>(key);
                if (news == null)
                {
                    news = this._newsRepository.FindBy((App.Domain.Entities.Data.News x) => x.SeoUrl.Equals(seoUrl), false);
                    _cacheManager.Put(key, news);
                }
            }
            else
            {
                news = this._newsRepository.FindBy((App.Domain.Entities.Data.News x) => x.SeoUrl.Equals(seoUrl), false);
            }

            return news;
        }

        public IEnumerable<App.Domain.Entities.Data.News> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
        {
            return this._newsRepository.PagedSearchList(sortbuBuilder, page);
        }

        public IEnumerable<App.Domain.Entities.Data.News> PagedListByMenu(SortingPagingBuilder sortBuider, Paging page)
        {
            return this._newsRepository.PagedSearchListByMenu(sortBuider, page);
        }

        public IEnumerable<App.Domain.Entities.Data.News> GetByOption(string virtualCategoryId = null
           , bool? isDisplayHomePage = null
           , bool? isVideo = null
           , int status = 1
           , bool isCache = true)
        {
            IEnumerable<App.Domain.Entities.Data.News> news;
            StringBuilder sbKey = new StringBuilder();
            sbKey.AppendFormat(CACHE_NEWS_KEY, "GetByOption");

            Expression<Func<App.Domain.Entities.Data.News, bool>> expression = PredicateBuilder.True<App.Domain.Entities.Data.News>();
            sbKey.AppendFormat("-{0}", status);
            expression = expression.And((App.Domain.Entities.Data.News x) => x.Status == status);

            if (virtualCategoryId.HasValue())
            {
                sbKey.AppendFormat("-{0}", virtualCategoryId);
                expression = expression.And((App.Domain.Entities.Data.News x) => x.VirtualCategoryId.Contains(virtualCategoryId));
            }
            if (isDisplayHomePage != null)
            {
                sbKey.AppendFormat("-{0}", isDisplayHomePage);
                expression = expression.And((App.Domain.Entities.Data.News x) => x.HomeDisplay == isDisplayHomePage);
            }
            if (isVideo != null)
            {
                sbKey.AppendFormat("-{0}", isVideo);
                expression = expression.And((App.Domain.Entities.Data.News x) => x.Video == isVideo);
            }

            if (isCache)
            {
                string key = sbKey.ToString();
                news = _cacheManager.GetCollection<App.Domain.Entities.Data.News>(key);
                if (news == null)
                {
                    news = FindBy(expression, false);
                    _cacheManager.Put(key, news);
                }
            }
            else
            {
                news = FindBy(expression, false);
            }

            return news;
        }
    }
}