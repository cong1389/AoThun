using App.Core.Caching;
using App.Core.Extensions;
using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Repository;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Static;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;

namespace App.Service.Static
{
	public class StaticContentService : BaseService<StaticContent>, IStaticContentService, IBaseService<StaticContent>, IService
	{
        private const string CACHE_STATICCONTENT_KEY = "db.StaticContent.{0}";
        private readonly ICacheManager _cacheManager;

        private readonly IStaticContentRepository _staticContentRepository;

		private readonly IUnitOfWork _unitOfWork;

		public StaticContentService(IUnitOfWork unitOfWork, IStaticContentRepository staticContentRepository, ICacheManager cacheManager) : base(unitOfWork, staticContentRepository)
		{
			this._unitOfWork = unitOfWork;
			this._staticContentRepository = staticContentRepository;
            _cacheManager = cacheManager;

        }

		public StaticContent GetById(int id, bool isCache = true)
		{
            StringBuilder sbKey = new StringBuilder();
            sbKey.AppendFormat(CACHE_STATICCONTENT_KEY, "GetById");
            sbKey.Append(id);

            string key = sbKey.ToString();
            StaticContent staticContent;
            if (isCache)
            {
                staticContent = _cacheManager.Get<StaticContent>(key);
                if (staticContent == null)
                {
                    staticContent = _staticContentRepository.GetById(id);
                    _cacheManager.Put(key, staticContent);
                }
            }
            else
            {
                staticContent = _staticContentRepository.GetById(id);
            }         

            return staticContent;
		}

		public IEnumerable<StaticContent> GetBySeoUrl(string seoUrl, bool isCache = true)
		{
            StringBuilder sbKey = new StringBuilder();
            sbKey.AppendFormat(CACHE_STATICCONTENT_KEY, "GetBySeoUrl");

            if (seoUrl.HasValue())
            {
                sbKey.AppendFormat("-{0}", seoUrl);
            }

            string key = sbKey.ToString();
            IEnumerable<StaticContent> staticContents ;
            if (isCache)
            {
                staticContents = _cacheManager.GetCollection<StaticContent>(key);
                if (staticContents == null)
                {
                    staticContents = _staticContentRepository.FindBy((StaticContent x) => x.SeoUrl.Equals(seoUrl), false);
                    _cacheManager.Put(key, staticContents);
                }
            }
            else
            {
                staticContents = _staticContentRepository.FindBy((StaticContent x) => x.SeoUrl.Equals(seoUrl), false);
            }           

            //IEnumerable<StaticContent> staticContents = this._staticContentRepository.FindBy((StaticContent x) => x.SeoUrl.Equals(seoUrl), false);
			return staticContents;
		}

		public IEnumerable<StaticContent> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
		{
			return this._staticContentRepository.PagedSearchList(sortbuBuilder, page);
		}

		public IEnumerable<StaticContent> PagedListByMenu(SortingPagingBuilder sortBuider, Paging page)
		{
			return this._staticContentRepository.PagedSearchListByMenu(sortBuider, page);
		}
	}
}