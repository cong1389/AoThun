using App.Core.Caching;
using App.Core.Extensions;
using App.Core.Utils;
using App.Domain.Entities.Menu;
using App.Domain.Interfaces.Repository;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Menu;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;

namespace App.Service.Menu
{
    public class MenuLinkService : BaseService<MenuLink>, IMenuLinkService, IBaseService<MenuLink>, IService
    {
        private const string CACHE_MENU_KEY = "db.MenuLink.{0}";
        private const string CACHE_MENU_KEY_1 = "db.MenuLink.{0}-{1}";
        private const string CACHE_MENU_KEY_2 = "db.MenuLink.{0}-{1}-{2}";
        private const string CACHE_MENU_KEY_3 = "db.MenuLink.{0}-{1}-{2}-{3}";
        private const string CACHE_MENU_KEY_Option = "db.MenuLink.";
        private readonly ICacheManager _cacheManager;
        private readonly IMenuLinkRepository _menuLinkRepository;

        private readonly IUnitOfWork _unitOfWork;

        public MenuLinkService(IUnitOfWork unitOfWork, IMenuLinkRepository menuLinkRepository
            , ICacheManager cacheManager) : base(unitOfWork, menuLinkRepository)
        {
            _unitOfWork = unitOfWork;
            _menuLinkRepository = menuLinkRepository;
            _cacheManager = cacheManager;
        }

        public MenuLink GetById(int id, bool isCache = true)
        {
            MenuLink menuLink;
            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_MENU_KEY, "GetById");
                sbKey.Append(id);

                string key = sbKey.ToString();
                menuLink = _cacheManager.Get<MenuLink>(key);
                if (menuLink == null)
                {
                    menuLink = _menuLinkRepository.GetById(id);
                    _cacheManager.Put(key, menuLink);
                }
            }
            else
            {
                menuLink = _menuLinkRepository.GetById(id);
            }

            return menuLink;
        }

        public IEnumerable<MenuLink> GetListSeoUrl(string seoUrl, bool isCache = true)
        {
            IEnumerable<MenuLink> menuLinks;
            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_MENU_KEY, "GetListSeoUrl");

                if (seoUrl.HasValue())
                {
                    sbKey.AppendFormat("-{0}", seoUrl);
                }

                string key = sbKey.ToString();
                menuLinks = _cacheManager.GetCollection<MenuLink>(key);
                if (menuLinks == null)
                {
                    menuLinks = _menuLinkRepository.FindBy((MenuLink x) => x.SeoUrl.Equals(seoUrl), false);
                    _cacheManager.Put(key, menuLinks);
                }
            }
            else
            {
                menuLinks = this._menuLinkRepository.FindBy((MenuLink x) => x.SeoUrl.Equals(seoUrl), false);

            }

            //IEnumerable<MenuLink> menuLinks = this._menuLinkRepository.FindBy((MenuLink x) => x.SeoUrl.Equals(seoUrl), false);
            return menuLinks;
        }

        public MenuLink GetBySeoUrl(string seoUrl, bool @readonly = false, bool isCache = true)
        {
            MenuLink menuLink;
            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_MENU_KEY, "GetBySeoUrl");

                if (seoUrl.HasValue())
                {
                    sbKey.AppendFormat("-{0}", seoUrl);
                }

                string key = sbKey.ToString();
                menuLink = _cacheManager.Get<MenuLink>(key);
                if (menuLink == null)
                {
                    menuLink = _menuLinkRepository.Get((MenuLink x) => x.SeoUrl.Equals(seoUrl), @readonly);
                    _cacheManager.Put(key, menuLink);
                }
            }
            else
            {
                menuLink = _menuLinkRepository.Get((MenuLink x) => x.SeoUrl.Equals(seoUrl), @readonly);
            }

            return menuLink;
        }

        public MenuLink GetByParentId(int parentId, string currentVirtualId, bool isCache = true)
        {
            MenuLink menuLink;
            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_MENU_KEY, "GetByParentId");
                sbKey.AppendFormat("-{0}", parentId);

                if (currentVirtualId.HasValue())
                    sbKey.AppendFormat("-{0}", currentVirtualId);

                string key = sbKey.ToString();
                menuLink = _cacheManager.Get<MenuLink>(key);
                if (menuLink == null)
                {
                    menuLink = _menuLinkRepository.Get((MenuLink x) => x.CurrentVirtualId.Equals(currentVirtualId) && x.ParentId != parentId, false);
                    _cacheManager.Put(key, menuLink);
                }
            }
            else
            {
                menuLink = _menuLinkRepository.Get((MenuLink x) => x.CurrentVirtualId.Equals(currentVirtualId) && x.ParentId != parentId, false);
            }

            return menuLink;
        }

        public MenuLink GetByCurrentVirtualId(string currentVirtualId, bool isCache = true)
        {
            MenuLink menuLink;
            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_MENU_KEY, "GetByCurrentVirtualId");

                if (currentVirtualId.HasValue())
                    sbKey.AppendFormat("-{0}", currentVirtualId);

                string key = sbKey.ToString();
                menuLink = _cacheManager.Get<MenuLink>(key);
                if (menuLink == null)
                {
                    menuLink = _menuLinkRepository.Get((MenuLink x) => x.CurrentVirtualId.Equals(currentVirtualId), false);
                    _cacheManager.Put(key, menuLink);
                }
            }
            else
            {
                menuLink = _menuLinkRepository.Get((MenuLink x) => x.CurrentVirtualId.Equals(currentVirtualId), false);
            }


            return menuLink;
        }

        public MenuLink GetByMenuName(string virtualCategoryId, string menuName, bool isCache = true)
        {
            MenuLink menuLink;
            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_MENU_KEY, "GetByMenuName");

                if (virtualCategoryId.HasValue())
                    sbKey.AppendFormat("-{0}", virtualCategoryId);

                if (menuName.HasValue())
                    sbKey.AppendFormat("-{0}", menuName);

                string key = sbKey.ToString();
                menuLink = _cacheManager.Get<MenuLink>(key);
                if (menuLink == null)
                {
                    menuLink = _menuLinkRepository.Get((MenuLink x) => x.CurrentVirtualId.Equals(virtualCategoryId) && !x.MenuName.Equals(menuName), false);
                    _cacheManager.Put(key, menuLink);
                }
            }
            else
            {
                menuLink = _menuLinkRepository.Get((MenuLink x) => x.CurrentVirtualId.Equals(virtualCategoryId) && !x.MenuName.Equals(menuName), false);
            }

            return menuLink;
        }

        public IEnumerable<MenuLink> PagedList(SortingPagingBuilder sortbuBuilder, Paging page)
        {
            return this._menuLinkRepository.PagedSearchList(sortbuBuilder, page);
        }

        public int Save()
        {
            return this._unitOfWork.Commit();
        }

        public IEnumerable<MenuLink> GetByTemplateType(int templateType, bool @readonly = false, bool isCache = true)
        {
            IEnumerable<MenuLink> menuLinks;
            if (isCache)
            {
                string key = string.Format(CACHE_MENU_KEY_2, "GetByTemplateType", templateType, @readonly);

                menuLinks = _cacheManager.GetCollection<MenuLink>(key);
                if (menuLinks == null)
                {
                    menuLinks = _menuLinkRepository.FindBy((MenuLink x) => x.TemplateType == templateType, @readonly);
                    _cacheManager.Put(CACHE_MENU_KEY_2, menuLinks);
                }
            }
            else
            {
                menuLinks = _menuLinkRepository.FindBy((MenuLink x) => x.TemplateType == templateType, @readonly);
            }

            return menuLinks;
        }

        /// <summary>
        /// Get nhiều param
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MenuLink> GetByOption(List<int> position = null
            , List<int> template = null
            , string virtualId = null
            , string seoUrl = null
            , List<int> parentId = null
            , bool? isDisplayHomePage = null
            , bool? isDisplaySearch = null
            , int status = 1
            , bool isCache = true)
        {

            StringBuilder sbKey = new StringBuilder();
            sbKey.AppendFormat(CACHE_MENU_KEY, "GetByOption");

            Expression<Func<MenuLink, bool>> expression = PredicateBuilder.True<MenuLink>();
            sbKey.AppendFormat("-{0}", status);
            expression = expression.And((MenuLink x) => x.Status == status);

            if (!position.IsNullOrEmpty())
            {
                int i = 0;
                foreach (int pos in position)
                {
                    sbKey.AppendFormat("-{0}", pos);
                    expression = i == 0 ? expression.And((MenuLink x) => x.Position == pos) : expression.Or((MenuLink x) => x.Position == pos);
                    i++;
                }
            }

            if (!template.IsNullOrEmpty())
            {
                int i = 0;
                foreach (int temp in template)
                {
                    sbKey.AppendFormat("-{0}", temp);
                    expression = i == 0 ? expression.And((MenuLink x) => x.TemplateType == temp) : expression.Or((MenuLink x) => x.TemplateType == temp);
                    i++;
                }
            }

            if (!parentId.IsNullOrEmpty())
            {
                int i = 0;
                foreach (int par in parentId)
                {
                    sbKey.AppendFormat("-{0}", parentId);
                    expression = i == 0 ? expression.And((MenuLink x) => x.ParentId == par) : expression.Or((MenuLink x) => x.ParentId == par);
                    i++;
                }
            }

            if (virtualId.HasValue())
            {
                sbKey.AppendFormat("-{0}", virtualId);
                expression = expression.And((MenuLink x) => x.VirtualId.Contains(virtualId));
            }

            if (seoUrl.HasValue())
            {
                sbKey.AppendFormat("-{0}", seoUrl);
                expression = expression.And((MenuLink x) => x.SeoUrl.Equals(seoUrl));
            }

            if (isDisplayHomePage != null)
            {
                //isDisplayHomePage
                sbKey.AppendFormat("-{0}", isDisplayHomePage);
                expression = expression.And((MenuLink x) => x.DisplayOnHomePage == isDisplayHomePage);
            }

            if (isDisplaySearch != null)
            {
                //isDisplaySearch
                sbKey.AppendFormat("-{0}", isDisplaySearch);
                expression = expression.And((MenuLink x) => x.DisplayOnSearch == isDisplaySearch);
            }

            IEnumerable<MenuLink> menuLinks;
            if (isCache)
            {

                string key = sbKey.ToString();
                menuLinks = _cacheManager.GetCollection<MenuLink>(key);
                if (menuLinks == null)
                {
                    menuLinks = FindBy(expression, false);
                    _cacheManager.Put(key, menuLinks);
                }
            }
            else
            {
                menuLinks = FindBy(expression, false);
            }            

            return menuLinks;
        }
    }
}