using App.Domain.Entities.Data;
using App.Domain.Entities.Menu;
using App.Front.Models;
using App.Service.Common;
using App.Service.Language;
using App.Service.Locations;
using App.Service.Menu;
using App.Service.Static;
using App.Aplication;
using App.Aplication.MVCHelper;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Web;
using System;
using App.Aplication.Extensions;

namespace App.Front.Controllers
{
    public class MenuController : FrontBaseController
    {
        private readonly IMenuLinkService _menuLinkService;

        private readonly IProvinceService _provinceService;

        private readonly IDistrictService _isDistrictService;

        private IStaticContentService _staticContentService;

        private readonly IWorkContext _workContext;

        public MenuController(
            IMenuLinkService menuLinkService
            , IProvinceService provinceService
            , IDistrictService isDistrictService
            , IStaticContentService staticContentService
            , IWorkContext workContext)
        {
            this._menuLinkService = menuLinkService;
            this._provinceService = provinceService;
            this._isDistrictService = isDistrictService;
            this._staticContentService = staticContentService;
            this._workContext = workContext;
        }

        [ChildActionOnly]
        [PartialCache("Short")]
        public ActionResult GetAccesssories()
        {
            IEnumerable<MenuLink> menuLinks = _menuLinkService.GetByOption(position: new List<int>() { 8 }, isDisplayHomePage: true);
            //IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) => x.TemplateType == 8 && x.DisplayOnHomePage && x.Status == 1, true);

            return base.PartialView(menuLinks);
        }

        [PartialCache("Short")]
        public ActionResult GetContent(string menu, int page)
        {
            dynamic viewBag = base.ViewBag;

            MenuLink menuLink = _menuLinkService.GetBySeoUrl(seoUrl: menu, @readonly: false);

            if (menuLink != null)
            {
                var menuLinkLocalized = menuLink.ToModel();

                ((dynamic)base.ViewBag).Title = menuLinkLocalized.MetaTitle ?? menuLinkLocalized.MenuName;
                ((dynamic)base.ViewBag).MetaKeyWords = menuLinkLocalized.MetaKeywords;
                ((dynamic)base.ViewBag).SiteUrl = base.Url.Action("GetContent", "Menu", new { menu = menu, page = page, area = "" });
                ((dynamic)base.ViewBag).Description = menuLinkLocalized.MetaDescription;
                ((dynamic)base.ViewBag).Image = base.Url.Content(string.Concat("~/", menuLinkLocalized.ImageUrl));

                //((dynamic)base.ViewBag).Title = menuLinkLocalized.MetaTitle;
                //((dynamic)base.ViewBag).KeyWords = menuLinkLocalized.MetaKeywords;D:\Project\MVC\AoThun\AoThun_ANT\App.Front\App.Front\Views\Post\GetProductTimeLine.cshtml
                //((dynamic)base.ViewBag).SiteUrl = base.Url.Action("GetContent", "Menu", new { menu = menu, page = page, area = "" });
                //((dynamic)base.ViewBag).Description = menuLinkLocalized.MetaDescription;
                //((dynamic)base.ViewBag).Image = base.Url.Content(string.Concat("~/", menuLinkLocalized.ImageUrl));

                if (menuLinkLocalized.TemplateType == 1)
                {
                    viewBag.MenuList = _menuLinkService.GetByOption(template: new List<int>() { 1 });
                    //IMenuLinkService menuLinkService = this._menuLinkService;
                    //viewBag.MenuList = _menuLinkService.FindBy((MenuLink x) => x.TemplateType == 1, false);
                }


            ((dynamic)base.ViewBag).ParentId = menuLink.ParentId;
            ((dynamic)base.ViewBag).Attrs = base.Request["attribute"];
            ((dynamic)base.ViewBag).Prices = base.Request["price"];
            ((dynamic)base.ViewBag).KeyWords = base.Request["keywords"];
            ((dynamic)base.ViewBag).ProAttrs = base.Request["proattribute"];

            ((dynamic)base.ViewBag).ProductHot = base.Request["producthot"];
            ((dynamic)base.ViewBag).ProductOld = base.Request["productold"];
            ((dynamic)base.ViewBag).ProductNew = base.Request["productnew"];

                ((dynamic)base.ViewBag).TemplateType = menuLink.TemplateType;
                ((dynamic)base.ViewBag).MenuId = menuLink.Id;
                ((dynamic)base.ViewBag).ImgePath = menuLink.ImageUrl;
                ((dynamic)base.ViewBag).PageNumber = page;
                ((dynamic)base.ViewBag).VirtualId = menuLink.VirtualId;
            }

            return base.View();
        }

        [PartialCache("Short")]
        public ActionResult GetFixItemContent(int id)
        {
            MenuLink menuLink = this._menuLinkService.GetById(id);
            //MenuLink menuLink = this._menuLinkService.Get((MenuLink x) => x.Id == Id, false);
            ((dynamic)base.ViewBag).ImgUrl = menuLink.ImageUrl;
            ((dynamic)base.ViewBag).TitleFix = menuLink.MenuName;

            IEnumerable<MenuLink> menuLinks = _menuLinkService.GetByOption(parentId: new List<int>() { id }, isDisplayHomePage: true);
            //IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) => x.ParentId == (int?)id && x.Status == 1 && x.DisplayOnHomePage, false);

            if (!menuLinks.IsAny<MenuLink>())
            {
                return base.Json(new { success = true, data = "" }, JsonRequestBehavior.AllowGet);
            }
            return base.Json(new { data = this.RenderRazorViewToString("_PartialFixItemContent", menuLinks), success = true }, JsonRequestBehavior.AllowGet);
        }

        [PartialCache("Short")]
        public ActionResult GetLeftFixItem(int id)
        {
            IEnumerable<MenuLink> menuLinks = _menuLinkService.GetByOption(parentId: new List<int>() { id }, isDisplayHomePage: true);
            //IEnumerable<MenuLink> menuLinks = _menuLinkService.FindBy((MenuLink x) => x.ParentId == (int?)Id && x.Status == 1 && x.DisplayOnHomePage, false);

            if (!menuLinks.IsAny<MenuLink>())
            {
                return base.Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            return base.Json(new { data = this.RenderRazorViewToString("_PartialLeftFixItemHome", menuLinks), success = true }, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        [PartialCache("Short")]
        public ActionResult GetProductTab()
        {
            IEnumerable<MenuLink> menuLinks = _menuLinkService.GetByOption(template: new List<int>() { 2 }, isDisplayHomePage: true);
            //IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) => x.TemplateType == 2 && x.DisplayOnHomePage && x.Status == 1, true);

            return base.PartialView(menuLinks);
        }

        [ChildActionOnly]
        public ActionResult GetStaticContent(int MenuId, string virtualId, string title)
        {
            List<BreadCrumb> breadCrumbs = new List<BreadCrumb>();
            string[] strArrays = virtualId.Split(new char[] { '/' });

            for (int i = 0; i < (int)strArrays.Length; i++)
            {
                string str = strArrays[i];
                MenuLink menuLink = _menuLinkService.GetByParentId(parentId: MenuId, currentVirtualId: str);
                //MenuLink menuLink = this._menuLinkService.Get((MenuLink x) => x.CurrentVirtualId.Equals(str) && x.ParentId != MenuId, false);

                if (menuLink != null)
                {
                    breadCrumbs.Add(new BreadCrumb()
                    {
                        Title = menuLink.GetLocalized(x => x.MenuName, menuLink.Id),
                        Current = false,
                        Url = base.Url.Action("GetContent", "Menu", new { area = "", menu = menuLink.SeoUrl })
                    });
                }
            }

            breadCrumbs.Add(new BreadCrumb()
            {
                Current = true,
                Title = title
            });
            ((dynamic)base.ViewBag).BreadCrumb = breadCrumbs;
            StaticContent staticContent = this._staticContentService.Get((StaticContent x) => x.MenuId == MenuId, true);
            if (staticContent != null)
            {
                staticContent = staticContent.ToModel();

                StaticContent viewCount = staticContent;
                viewCount.ViewCount = viewCount.ViewCount + 1;
                this._staticContentService.Update(staticContent);

                //Lấy bannerId từ post để hiển thị banner trên post
                ((dynamic)base.ViewBag).BannerId = staticContent.MenuId;

                ((dynamic)base.ViewBag).Title = staticContent.Title;
            }

            ((dynamic)base.ViewBag).MenuId = MenuId;

            return base.PartialView(staticContent);
        }

        public ActionResult GetStaticContentParent(int menuId, string title, string virtualId)
        {
            List<BreadCrumb> breadCrumbs = new List<BreadCrumb>();
            string[] strArrays = virtualId.Split(new char[] { '/' });

            StaticContent staticContent = this._staticContentService.Get((StaticContent x) => x.MenuId == menuId && x.Status == 1, false);

            //Convert to localized
            var staticContentLocalized = staticContent.ToModel();

            dynamic viewBag = base.ViewBag;

            IEnumerable<MenuLink> menuLinks = _menuLinkService.FindBy((MenuLink x) => x.Id == menuId && x.Status == 1, false);

            //Convert to localized
            menuLinks = menuLinks.Select(x => x.ToModel());

            if (menuLinks.IsAny<MenuLink>())
            {
                viewBag.ListItems = menuLinks;
            }

            string[] strArrays1 = strArrays;
            for (int i = 0; i < (int)strArrays1.Length; i++)
            {
                string str = strArrays1[i];
                MenuLink menuLink = this._menuLinkService.Get((MenuLink x) => x.CurrentVirtualId.Equals(str) && !x.MenuName.Equals(title), false);
                if (menuLink != null)
                {
                    breadCrumbs.Add(new BreadCrumb()
                    {
                        Title = menuLink.GetLocalized(x => x.MenuName, menuLink.Id),
                        Current = false,
                        Url = base.Url.Action("GetContent", "Menu", new { area = "", menu = menuLink.SeoUrl })
                    });
                }
            }

            breadCrumbs.Add(new BreadCrumb()
            {
                Current = true,
                Title = staticContentLocalized.Title
            });

            ((dynamic)base.ViewBag).TitleNews = staticContentLocalized.Title;
            ((dynamic)base.ViewBag).BreadCrumb = breadCrumbs;
            ((dynamic)base.ViewBag).Title = staticContentLocalized.Title;

            return base.PartialView(staticContentLocalized);
        }

        [ChildActionOnly]
        public ActionResult GetStaticHot(string virtualId)
        {
            string str = virtualId;
            string[] strArrays = str.Split(new char[] { '/' });

            if ((int)strArrays.Length >= 3)
            {
                str = string.Format("{0}/{1}", strArrays[0], strArrays[1]);
            }

            IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) => x.VirtualId.Contains(str) && x.TemplateType == 6, false);
            return base.PartialView(menuLinks);
        }

        public ActionResult Search(SeachConditions conditions)
        {
            string str = JsonConvert.SerializeObject(conditions);
            HttpCookie cookie = new HttpCookie("system_search", str);
            cookie.Expires = DateTime.Now.AddDays(1.0);
            base.Response.Cookies.Add(cookie);

            MenuLink byId = new MenuLink();
            IEnumerable<MenuLink> menuLinks = this._menuLinkService.GetByOption(isDisplayHomePage: true, template: new List<int> { 2 });

            if (menuLinks.IsAny())
            {
                byId = menuLinks.FirstOrDefault();
            }

            return RedirectToAction("SearchResult", "Post", new { catUrl = byId.SeoUrl, parameters = conditions.Keywords.NonAccent(), area = "" });
        }
    }
}