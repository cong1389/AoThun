using App.Core.Caching;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Entities.Location;
using App.Domain.Entities.Menu;
using App.FakeEntity.Menu;
using App.Front.Models;
using App.Service.Common;
using App.Service.ContactInformation;
using App.Service.Locations;
using App.Service.Menu;
using App.Service.SeoSetting;
using App.Service.SystemApp;
using App.Aplication.MVCHelper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using App.Aplication.Extensions;

namespace App.Front.Controllers
{
    public class SummaryController : FrontBaseController
    {
        private const string CACHE_SETTINGSYSTEM_KEY = "db.SettingSystem.{0}";
        private const string CACHE_SETTINGSEOGLOBAL_KEY = "db.SettingSeoGlobal.{0}";
        private readonly ICacheManager _cacheManager;
        private readonly IMenuLinkService _menuLinkService;

        private readonly IProvinceService _provinceService;

        private readonly IDistrictService _districtService;

        private readonly ISystemSettingService _systemSettingService;

        private readonly IContactInfoService _contactInfoService;

        private readonly ISettingSeoGlobalService _settingSeoGlobal;

        private readonly IWorkContext _workContext;

        public SummaryController(IMenuLinkService menuLinkService
            , IProvinceService provinceService, IDistrictService districtService, ISystemSettingService systemSettingService
            , IContactInfoService contactInfoService
            , ISettingSeoGlobalService settingSeoGlobal
            , IWorkContext workContext
            , ICacheManager cacheManager)
        {
            this._menuLinkService = menuLinkService;
            this._provinceService = provinceService;
            this._districtService = districtService;
            this._systemSettingService = systemSettingService;
            this._contactInfoService = contactInfoService;
            this._settingSeoGlobal = settingSeoGlobal;
            this._workContext = workContext;
            _cacheManager = cacheManager;
        }

        [PartialCache("Long")]
        private List<MenuNavViewModel> CreateMenuNav(int? parentId, IEnumerable<MenuNavViewModel> source)
        {
            return (
                from x in source
                orderby x.OrderDisplay descending
                select x).Where<MenuNavViewModel>((MenuNavViewModel x) =>
                {
                    int? nullable1 = x.ParentId;
                    int? nullable = parentId;
                    if (nullable1.GetValueOrDefault() != nullable.GetValueOrDefault())
                    {
                        return false;
                    }
                    return nullable1.HasValue == nullable.HasValue;
                }).Select<MenuNavViewModel, MenuNavViewModel>((MenuNavViewModel x) => new MenuNavViewModel()
                {
                    MenuId = x.MenuId,
                    ParentId = x.ParentId,
                    MenuName = x.MenuName,
                    SeoUrl = x.SeoUrl,
                    OrderDisplay = x.OrderDisplay,
                    ImageUrl = x.ImageUrl,
                    CurrentVirtualId = x.CurrentVirtualId,
                    VirtualId = x.VirtualId,
                    ChildNavMenu = this.CreateMenuNav(new int?(x.MenuId), source)
                }).ToList<MenuNavViewModel>();
        }

        [PartialCache("Long")]
        public ActionResult GetAddressInfo()
        {
            ContactInformation contactInformation = this._contactInfoService.Get((ContactInformation x) => x.Status == 1 && x.Type == 1, true);

            var contactInformationLocalize = contactInformation.ToModel();

            return base.PartialView(contactInformationLocalize);
        }

        [ChildActionOnly]
        [PartialCache("Short")]
        public ActionResult GetCategorySearchBox()
        {
            List<MenuNavViewModel> menuNavs = new List<MenuNavViewModel>();
            IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) => x.Status == 1 && x.TemplateType == 2, true);
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
                        VirtualId = x.VirtualId
                    };
                menuNavs = this.CreateMenuNav(null, menuNav);
            }
            return base.PartialView(menuNavs);
        }

        [ChildActionOnly]
        [PartialCache("Short")]
        public ActionResult GetContactHeader()
        {
            SystemSetting systemSetting = this._systemSettingService.Get((SystemSetting x) => x.Status == 1, false);

            var systemSettingLocalized = systemSetting.ToModel();

            return base.PartialView(systemSettingLocalized);
        }

        [ChildActionOnly]
        [PartialCache("Short")]
        public ActionResult GetContactOrder()
        {
            ContactInformation contactInformation = this._contactInfoService.Get((ContactInformation x) => x.Status == 1 && x.Type == 1, true);

            var contactInformationLocalized = contactInformation.ToModel();

            return base.PartialView(contactInformationLocalized);
        }

        [ChildActionOnly]
        [PartialCache("Short")]
        public ContentResult GetContentFooter()
        {
            SystemSetting systemSetting = this._systemSettingService.Get((SystemSetting x) => x.Status == 1, true);

            var systemSettingLocalized = systemSetting.ToModel();

            if (systemSetting == null)
            {
                return base.Content(string.Empty);
            }

            return base.Content(systemSetting.FooterContent);
        }

        [HttpPost]
        public JsonResult GetDistrictByProvinceId(int provinceId)
        {
            if (!base.Request.IsAjaxRequest())
            {
                return null;
            }

            var byProvinceId =
                from x in this._districtService.GetByProvinceId(provinceId)
                select new { Id = x.Id, Name = x.Name };

            return base.Json(byProvinceId);
        }

        [ChildActionOnly]
        [PartialCache("Short")]
        public ActionResult Footer()
        {
            ContactInformation contactInformation = this._contactInfoService.Get((ContactInformation x) => x.Status == 1 && x.Type == 1, true);

            var contactInformationLocalize = contactInformation.ToModel();

            return base.PartialView(contactInformationLocalize);
        }

        public ActionResult GetLogo()
        {
            SystemSetting systemSetting = GetSystemSettingData();

            var systemSettingLocalized = systemSetting.ToModel();

            return base.PartialView(systemSettingLocalized);
        }

        [ChildActionOnly]
        [PartialCache("Short")]
        public ActionResult GetMetaTagsSeo()
        {
            SettingSeoGlobal settingSeoGlobal = GetSettingSeoData();

            return base.PartialView(settingSeoGlobal);
        }

        public ActionResult GetMeta()
        {
            SystemSetting systemSetting = _systemSettingService.Get((SystemSetting x) => x.Status == 1, true);

            if (systemSetting == null)
                return HttpNotFound();

            var systemSettingLocalized = systemSetting.ToModel();

            string controller = Request.RequestContext.RouteData.Values["Controller"].ToString();
            string action = Request.RequestContext.RouteData.Values["Action"].ToString();

            if (controller.Equals("Home") && action.Equals("Index"))
            {
                ((dynamic)base.ViewBag).Title = systemSettingLocalized.Title;
                ((dynamic)base.ViewBag).KeyWords = systemSettingLocalized.MetaKeywords;
                ((dynamic)base.ViewBag).SiteUrl = base.Url.Action("Index", "Home", new { area = "" });
                ((dynamic)base.ViewBag).Description = systemSettingLocalized.Description;
                ((dynamic)base.ViewBag).Image = base.Url.Content(string.Concat("~/", systemSettingLocalized.LogoImage));
            }

            ((dynamic)base.ViewBag).Favicon = base.Url.Content(string.Concat("~/", systemSettingLocalized.FaviconImage));

            return base.PartialView(systemSettingLocalized);
        }

        [ChildActionOnly]
        [PartialCache("Short")]
        public ActionResult GetProvinceSearchBox()
        {
            IEnumerable<Province> provinces = this._provinceService.FindBy((Province x) => x.Status == 1, false);
            return base.PartialView(provinces);
        }

        [ChildActionOnly]
        [PartialCache("Short")]
        public ActionResult ScriptSippet()
        {
            SettingSeoGlobal settingSeoGlobal = GetSettingSeoData();
            return base.PartialView(settingSeoGlobal);
        }

        [PartialCache("Long")]
        public JsonResult GetPostAddress()
        {
            ContactInformation contactInformation = this._contactInfoService.Get((ContactInformation x) => x.Status == 1 && x.Type == 1, true);

            var contactInformationLocalize = contactInformation.ToModel();

            JsonResult jsonResult = Json(new { success = true, list = this.RenderRazorViewToString("_Post.Address", contactInformationLocalize) }, JsonRequestBehavior.AllowGet);

            return jsonResult;

        }

        [PartialCache("Long")]
        public JsonResult GetFooterAddress()
        {
            IEnumerable<ContactInformation> contactInformation = _contactInfoService.FindBy((ContactInformation x) => x.Status == 1, true);

            var contactInformationLocalize = contactInformation.Select(x => x.ToModel());

            JsonResult jsonResult = Json(new { success = true, list = this.RenderRazorViewToString("_Footer.Address", contactInformation) }, JsonRequestBehavior.AllowGet);

            return jsonResult;

        }

        [PartialCache("Long")]
        public JsonResult GetFooterLogo()
        {
            SystemSetting systemSetting = this._systemSettingService.Get((SystemSetting x) => x.Status == 1, false);

            var systemSettingLocalize = systemSetting.ToModel();

            JsonResult jsonResult = Json(new { success = true, list = this.RenderRazorViewToString("_Footer.Logo", systemSettingLocalize) }, JsonRequestBehavior.AllowGet);

            return jsonResult;

        }

        [PartialCache("Long")]
        public JsonResult GetSystemSetting()
        {
            SystemSetting systemSetting = this._systemSettingService.Get((SystemSetting x) => x.Status == 1, false);

            var systemSettingLocalize = systemSetting.ToModel();

            JsonResult jsonResult = Json(new { success = true, list = systemSettingLocalize }, JsonRequestBehavior.AllowGet);

            return jsonResult;

        }

        #region Social

        public async Task<JsonResult> GetSettingSeo()
        {
            SettingSeoGlobal settingSeoGlobal = await Task.FromResult(GetSettingSeoData());

            JsonResult jsonResult = Json(new { success = true, response = settingSeoGlobal }, JsonRequestBehavior.AllowGet);

            return jsonResult;
        }

        #endregion

        #region System Setting

        [PartialCache("Long")]
        public JsonResult GetLogoMobile()
        {
            SystemSetting systemSetting = GetSystemSettingData();

            var systemSettingLocalize = systemSetting.ToModel();

            JsonResult jsonResult = Json(new { success = true, list = systemSettingLocalize }, JsonRequestBehavior.AllowGet);

            return jsonResult;
        }

        private SystemSetting GetSystemSettingData()
        {
            StringBuilder sbKey = new StringBuilder();
            sbKey.AppendFormat(CACHE_SETTINGSYSTEM_KEY, "GetSystemSettingData");

            string key = sbKey.ToString();
            SystemSetting systemSetting = _cacheManager.Get<SystemSetting>(key);
            if (systemSetting == null)
            {
                systemSetting = _systemSettingService.Get((SystemSetting x) => x.Status == 1, false);
                _cacheManager.Put(key, systemSetting);
            }

            return systemSetting;
        }

        #endregion

        #region Setting Seo

        private SettingSeoGlobal GetSettingSeoData()
        {
            StringBuilder sbKey = new StringBuilder();
            sbKey.AppendFormat(CACHE_SETTINGSEOGLOBAL_KEY, "GetSettingSeoGlobal");

            string key = sbKey.ToString();
            SettingSeoGlobal settingSeoGlobal = _cacheManager.Get<SettingSeoGlobal>(key);
            if (settingSeoGlobal == null)
            {
                settingSeoGlobal = this._settingSeoGlobal.Get((SettingSeoGlobal x) => x.Status == 1, false);
                _cacheManager.Put(key, settingSeoGlobal);
            }

            return settingSeoGlobal;
        }

        #endregion
    }
}