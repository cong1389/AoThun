using App.Aplication.Extensions;
using App.Core.Caching;
using App.Domain.Entities.Menu;
using App.FakeEntity.Menu;
using App.Front.Models;
using App.Service.Common;
using App.Service.Language;
using App.Service.LocalizedProperty;
using App.Service.Menu;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace App.Front.Controllers.Custom
{
    public class MenuNavController : FrontBaseController
    {
        private const string CACHE_MENU_KEY = "db.Menu.{0}";

        private readonly IWorkContext _workContext;

        private readonly IMenuLinkService _menuLinkService;

        private readonly ILocalizedPropertyService _localizedPropertyService;

        private readonly ILanguageService _languageService;

        private readonly ICacheManager _cacheManager;

        public MenuNavController(IMenuLinkService menuLinkService, ILocalizedPropertyService localizedPropertyService
            , IWorkContext workContext, ILanguageService languageService
            , ICacheManager cacheManage)
        {
            this._menuLinkService = menuLinkService;
            this._localizedPropertyService = localizedPropertyService;
            this._workContext = workContext;
            this._languageService = languageService;
            _cacheManager = cacheManage;
        }

        [PartialCache("Long")]
        private List<MenuNavViewModel> CreateMenuNav(int? parentId, IEnumerable<MenuNavViewModel> source)
        {
            List<MenuNavViewModel> ieMenuNav = (from x in source
                                                orderby x.OrderDisplay descending
                                                select x).Where((MenuNavViewModel x) =>
                                                {
                                                    int? nullable1 = x.ParentId;
                                                    int? nullable = parentId;
                                                    if (nullable1.GetValueOrDefault() != nullable.GetValueOrDefault())
                                                    {
                                                        return false;
                                                    }
                                                    return nullable1.HasValue == nullable.HasValue;
                                                }).Select((MenuNavViewModel x) => new MenuNavViewModel()
                                                {
                                                    MenuId = x.MenuId,
                                                    ParentId = x.ParentId,
                                                    MenuName = x.MenuName,
                                                    SeoUrl = x.SeoUrl,
                                                    OrderDisplay = x.OrderDisplay,
                                                    ImageUrl = x.ImageUrl,
                                                    CurrentVirtualId = x.CurrentVirtualId,
                                                    VirtualId = x.VirtualId,
                                                    TemplateType = x.TemplateType,
                                                    OtherLink = x.OtherLink,
                                                    IconNav = x.IconNav,
                                                    IconBar = x.IconBar,
                                                    ChildNavMenu = this.CreateMenuNav(new int?(x.MenuId), source)
                                                }).ToList();

            return ieMenuNav;
        }

        [PartialCache("Long")]
        public ActionResult GetFixedHomePage()
        {
            List<MenuNavViewModel> menuNavs = new List<MenuNavViewModel>();
            IEnumerable<MenuLink> menuLinks = _menuLinkService.GetByOption(template: new List<int>() { 6 }, isDisplayHomePage: true);

            //IEnumerable<MenuLink> menuLinks = _menuLinkService.FindBy((MenuLink x) => x.Status == 1 && x.TemplateType == 6 && x.DisplayOnHomePage, true);
            if (menuLinks.Any())
            {
                IEnumerable<MenuNavViewModel> menuNav =
                    from x in menuLinks
                    select new MenuNavViewModel()
                    {
                        MenuId = x.Id,
                        ParentId = x.ParentId,
                        MenuName = x.MenuName,
                        SeoUrl = x.SeoUrl,
                        OrderDisplay = x.OrderDisplay,
                        ImageUrl = x.ImageUrl,
                        CurrentVirtualId = x.CurrentVirtualId,
                        VirtualId = x.VirtualId,
                        TemplateType = x.TemplateType,
                        IconNav = x.Icon1,
                        IconBar = x.Icon2
                    };

                menuNavs = this.CreateMenuNav(null, menuNav);
            }
            return base.PartialView(menuNavs);
        }

        [ChildActionOnly]
        public ActionResult GetFooterLink()
        {
            List<MenuNavViewModel> menuNavs = new List<MenuNavViewModel>();

            IEnumerable<MenuLink> menuLinks = _menuLinkService.GetByOption(position: new List<int>() { 2 });
            //IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) => x.Status == 1 && x.Position == 2, true);

            if (menuLinks.Any<MenuLink>())
            {
                IEnumerable<MenuNavViewModel> menuNav =
                    from x in menuLinks
                    select new MenuNavViewModel()
                    {
                        MenuId = x.Id,
                        ParentId = x.ParentId,
                        MenuName = x.MenuName,
                        SeoUrl = x.SeoUrl,
                        OrderDisplay = x.OrderDisplay,
                        ImageUrl = x.ImageUrl,
                        CurrentVirtualId = x.CurrentVirtualId,
                        VirtualId = x.VirtualId,
                        TemplateType = x.TemplateType,
                        IconNav = x.Icon1,
                        IconBar = x.Icon2
                    };
                menuNavs = this.CreateMenuNav(null, menuNav);
            }
            return base.PartialView(menuNavs);
        }

        [NonAction]
        [PartialCache("Long")]
        public ActionResult TopMenu()
        {
            IEnumerable<MenuLink> menuLinks = _menuLinkService.GetByOption(position: new List<int>() { 1 });

            //Convert to localized
            menuLinks = menuLinks.Select(x =>
            {
                return x.ToModel();
            });

            List<MenuNavViewModel> menuNavs = new List<MenuNavViewModel>();
            if (menuLinks.Any())
            {
                IEnumerable<MenuNavViewModel> menuNav =
                    from x in menuLinks
                    select new MenuNavViewModel()
                    {
                        MenuId = x.Id,
                        ParentId = x.ParentId,
                        MenuName = x.MenuName,
                        SeoUrl = x.SeoUrl,
                        OtherLink = x.SourceLink,
                        OrderDisplay = x.OrderDisplay,
                        ImageUrl = x.ImageUrl,
                        CurrentVirtualId = x.CurrentVirtualId,
                        VirtualId = x.VirtualId,
                        TemplateType = x.TemplateType,
                        IconNav = x.Icon1,
                        IconBar = x.Icon2
                    };

                menuNavs = this.CreateMenuNav(null, menuNav);
            }

            return base.PartialView(menuNavs);
        }

        [ChildActionOnly]
        public ActionResult GetTopMenu()
        {
            return TopMenu();
        }

        [ChildActionOnly]
        public ActionResult GetTopMenuMobile()
        {
            return TopMenu();
        }

        /// <summary>
        /// Get danh mục cha bên trái ở trang chủ, 
        /// </summary>
        /// <param name="virtualId"></param>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult MenuNavLeft(string virtualId)
        {
            virtualId = (virtualId != null && virtualId.Count(i => i.Equals('/')) > 0) ? virtualId.Split('/')[0] : virtualId;
            IEnumerable<MenuLink> menuLinks = _menuLinkService.GetByOption(virtualId: virtualId);

            //IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) =>
            //                                        x.Status == 1
            //                                        && x.VirtualId.Contains(virtualId)
            //                                        , true);
            //Convert to localized
            menuLinks = menuLinks.Select(x =>
            {
                return x.ToModel();
            });

            List<MenuNavViewModel> menuNavs = new List<MenuNavViewModel>();
            if (menuLinks.Any<MenuLink>())
            {
                IEnumerable<MenuNavViewModel> menuNavLocalized =
                    from x in menuLinks
                    select new MenuNavViewModel()
                    {
                        MenuId = x.Id,
                        ParentId = x.ParentId,
                        MenuName = x.MenuName,
                        SeoUrl = x.SeoUrl,
                        OrderDisplay = x.OrderDisplay,
                        ImageUrl = x.ImageUrl,
                        CurrentVirtualId = x.CurrentVirtualId,
                        VirtualId = x.VirtualId,
                        TemplateType = x.TemplateType,
                        IconNav = x.Icon1,
                        IconBar = x.Icon2
                    };
                menuNavs = this.CreateMenuNav(null, menuNavLocalized);
            }
            return base.PartialView(menuNavs);
        }

        /// <summary>
        /// Menu category slidebar bên trái
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        [PartialCache("Long")]
        public ActionResult GetMenuCategory()
        {
            List<MenuNavViewModel> menuNavs = new List<MenuNavViewModel>();
            IEnumerable<MenuLink> menuLinks = _menuLinkService.GetByOption(position: new List<int>() { 1, 5 });

            //IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) => x.Status == 1 && (x.Position == 5 || x.Position == 1), true);
            if (menuLinks.Any<MenuLink>())
            {
                IEnumerable<MenuNavViewModel> menuNav =
                    from x in menuLinks
                    select new MenuNavViewModel()
                    {
                        MenuId = x.Id,
                        ParentId = x.ParentId,
                        MenuName = x.MenuName,
                        SeoUrl = x.SeoUrl,
                        OrderDisplay = x.OrderDisplay,
                        ImageUrl = x.ImageUrl,
                        CurrentVirtualId = x.CurrentVirtualId,
                        VirtualId = x.VirtualId,
                        TemplateType = x.TemplateType,
                        IconNav = x.Icon1,
                        IconBar = x.Icon2
                    };
                menuNavs = this.CreateMenuNav(null, menuNav);
            }
            return base.PartialView(menuNavs);
        }

        [ChildActionOnly]
        public ActionResult GetMenuLinkSideBar(List<int> Ids = null)
        {
            List<MenuNavViewModel> menuNavs = new List<MenuNavViewModel>();
            MenuLink menuLink = this._menuLinkService.Get((MenuLink x) => x.Status == 1 && x.TemplateType == 2 && !x.ParentId.HasValue, false);

            IEnumerable<MenuLink> menuLinks = _menuLinkService.GetByOption(template: new List<int>() { 2 }, virtualId: menuLink.VirtualId);
            //IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) => x.Status == 1 && x.TemplateType == 2 && x.VirtualId.Contains(menuLink.VirtualId), true);

            ((dynamic)base.ViewBag).ProIds = Ids;

            if (menuLinks.Any<MenuLink>())
            {
                IEnumerable<MenuNavViewModel> menuNav =
                    from x in menuLinks
                    select new MenuNavViewModel()
                    {
                        MenuId = x.Id,
                        ParentId = x.ParentId,
                        MenuName = x.MenuName,
                        SeoUrl = x.SeoUrl,
                        OrderDisplay = x.OrderDisplay,
                        ImageUrl = x.ImageUrl,
                        CurrentVirtualId = x.CurrentVirtualId,
                        VirtualId = x.VirtualId,
                        TemplateType = x.TemplateType,
                        IconNav = x.Icon1,
                        IconBar = x.Icon2
                    };
                menuNavs = this.CreateMenuNav(new int?(menuLink.Id), menuNav);
            }

            return base.PartialView(menuNavs);
        }

        [ChildActionOnly]
        public ActionResult GetMenuOnHomePage()
        {
            IEnumerable<MenuLink> menuLinks = _menuLinkService.GetByOption(isDisplayHomePage: true);
            ////IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) => x.Status == 1 && x.DisplayOnHomePage, false);
            return base.PartialView(menuLinks);
        }

        [ChildActionOnly]
        public ActionResult Header()
        {
            List<MenuNavViewModel> menuNavs = new List<MenuNavViewModel>();
            IEnumerable<MenuLink> menuLinks = _menuLinkService.GetByOption(position: new List<int>() { 7 });

            //Convert to localized
            menuLinks = menuLinks.Select(x =>
            {
                return x.ToModel();
            });

            if (menuLinks.Any<MenuLink>())
            {
                IEnumerable<MenuNavViewModel> menuNav =
                    from x in menuLinks
                    select new MenuNavViewModel()
                    {
                        MenuId = x.Id,
                        ParentId = x.ParentId,
                        MenuName = x.MenuName,
                        SeoUrl = x.SeoUrl,
                        OrderDisplay = x.OrderDisplay,
                        ImageUrl = x.ImageUrl,
                        CurrentVirtualId = x.CurrentVirtualId,
                        VirtualId = x.VirtualId,
                        TemplateType = x.TemplateType
                    };

                menuNavs = this.CreateMenuNav(null, menuNav);
            }
            return base.PartialView(menuNavs);
        }

        [PartialCache("Long")]
        public ActionResult StickyBar()
        {
            List<MenuNavViewModel> menuNavs = new List<MenuNavViewModel>();
            IEnumerable<MenuLink> menuLinks = _menuLinkService.GetByOption(position: new List<int>() { 6 }, isDisplaySearch: true);

            if (menuLinks.Any<MenuLink>())
            {
                IEnumerable<MenuNavViewModel> menuNav =
                    from x in menuLinks
                    select new MenuNavViewModel()
                    {
                        MenuId = x.Id,
                        ParentId = x.ParentId,
                        MenuName = x.MenuName,
                        SeoUrl = x.SeoUrl,
                        OrderDisplay = x.OrderDisplay,
                        ImageUrl = x.ImageUrl,
                        CurrentVirtualId = x.CurrentVirtualId,
                        VirtualId = x.VirtualId,
                        TemplateType = x.TemplateType,
                        IconNav = x.Icon1,
                        IconBar = x.Icon2
                    };
                menuNavs = this.CreateMenuNav(null, menuNav);
            }
            return base.PartialView(menuNavs);
        }
    }


}