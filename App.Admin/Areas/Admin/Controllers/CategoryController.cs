using App.Admin.Helpers;
using App.Core.Caching;
using App.Core.Common;
using App.Domain.Entities.Menu;
using App.Domain.Interfaces.Services;
using App.Service.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace App.Admin.Controllers
{
	public class CategoryController : BaseAdminController
	{
        private const string CACHE_CATEGORY_KEY = "db.Category";
        private readonly ICacheManager _cacheManager;

        private readonly IMenuLinkService _menuLinkService;

		public CategoryController(IMenuLinkService menuLinkService
             , ICacheManager cacheManager)
		{
			this._menuLinkService = menuLinkService;
            _cacheManager = cacheManager;

            //Clear cache
            _cacheManager.RemoveByPattern(CACHE_CATEGORY_KEY);
        }

        private List<MenuNav> CreateMenuNav(int? parentId, IEnumerable<MenuNav> source)
		{
			return source.Where<MenuNav>((MenuNav x) => {
				int? nullable1 = x.ParentId;
				int? nullable = parentId;
				if (nullable1.GetValueOrDefault() != nullable.GetValueOrDefault())
				{
					return false;
				}
				return nullable1.HasValue == nullable.HasValue;
			}).Select<MenuNav, MenuNav>((MenuNav x) => new MenuNav()
			{
				MenuId = x.MenuId,
				ParentId = x.ParentId,
				MenuName = x.MenuName,
				ChildNavMenu = this.CreateMenuNav(new int?(x.MenuId), source)
			}).ToList<MenuNav>();
		}

		public ActionResult GetMenu(int? selected)
		{
			List<MenuNav> menuNavs = new List<MenuNav>();
			IEnumerable<MenuLink> all = this._menuLinkService.GetAll();
			if (all.Any<MenuLink>())
			{
				IEnumerable<MenuNav> orderDisplay = 
					from x in all
					orderby x.OrderDisplay descending
					select new MenuNav()
					{
						MenuId = x.Id,
						ParentId = x.ParentId,
						MenuName = x.MenuName
					};
				menuNavs = this.CreateMenuNav(null, orderDisplay);
			}
			((dynamic)base.ViewBag).Selected = selected;
			return base.PartialView(menuNavs.ToList<MenuNav>());
		}

		public ActionResult GetMenuProduct(int? selected)
		{
			List<MenuNav> menuNavs = new List<MenuNav>();
			IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) => x.TemplateType == 2 || x.TemplateType == 8, true);
			if (menuLinks.Any<MenuLink>())
			{
				IEnumerable<MenuNav> orderDisplay = 
					from x in menuLinks
					orderby x.OrderDisplay descending
					select new MenuNav()
					{
						MenuId = x.Id,
						ParentId = x.ParentId,
						MenuName = x.MenuName
					};
				menuNavs = this.CreateMenuNav(null, orderDisplay);
			}
			((dynamic)base.ViewBag).Selected = selected;
			return base.PartialView(menuNavs.ToList<MenuNav>());
		}
	}
}