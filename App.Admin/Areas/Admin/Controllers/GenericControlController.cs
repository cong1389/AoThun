using App.Admin.Helpers;
using App.Core.Utils;
using App.Domain.Entities.GenericControl;
using App.Domain.Entities.Language;
using App.Domain.Entities.Menu;
using App.FakeEntity.GenericControl;
using App.FakeEntity.Menu;
using App.Framework.Ultis;
using App.Service.GenericControl;
using App.Service.Language;
using App.Service.LocalizedProperty;
using App.Service.Menu;
using App.Aplication;
using AutoMapper;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Core.Caching;
using App.Aplication.Extensions;

namespace App.Admin.Controllers
{
    public class GenericControlController : BaseAdminController
    {
        private const string CACHE_GENERICCONTROL_KEY = "db.GenericControl";
        private readonly ICacheManager _cacheManager;

        private readonly IGenericControlService _genericControlService;

        private readonly ILanguageService _languageService;

        private readonly ILocalizedPropertyService _localizedPropertyService;

        private readonly IGenericControlValueService _genericControlValueService;
        private readonly IMenuLinkService _menuLinkService;

        public GenericControlController(IGenericControlService genericControlService
              , ILanguageService languageService
             , ILocalizedPropertyService localizedPropertyService
            , IGenericControlValueService genericControlValueService
            , IMenuLinkService menuLinkService
            , ICacheManager cacheManager)
        {
            _genericControlService = genericControlService;
            _languageService = languageService;
            _localizedPropertyService = localizedPropertyService;
            _genericControlValueService = genericControlValueService;
            _menuLinkService = menuLinkService;
            _cacheManager = cacheManager;

            //Clear cache
            _cacheManager.RemoveByPattern(CACHE_GENERICCONTROL_KEY);
        }

        [RequiredPermisson(Roles = "CreateEditGenericControl")]
        public ActionResult Create()
        {
            var model = new GenericControlViewModel
            {
                OrderDisplay = 0,
                Status = 1
            };

            //Add locales to model
            AddLocales(_languageService, model.Locales);

            return base.View(model);
        }

        [HttpPost]
        [RequiredPermisson(Roles = "CreateEditGenericControl")]
        public ActionResult Create(GenericControlViewModel model, string ReturnUrl)
        {
            ActionResult action;
            try
            {
                if (!base.ModelState.IsValid)
                {
                    String messages = String.Join(Environment.NewLine, ModelState.Values.SelectMany(v => v.Errors)
                                                         .Select(v => v.ErrorMessage + " " + v.Exception));
                    base.ModelState.AddModelError("", messages);
                    return base.View(model);
                }
                else
                {
                    GenericControl modelMap = Mapper.Map<GenericControlViewModel, App.Domain.Entities.GenericControl.GenericControl>(model);
                    this._genericControlService.Create(modelMap);

                    //Update Localized   
                    foreach (var localized in model.Locales)
                    {
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Name, localized.Name, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Description, localized.Description, localized.LanguageId);
                    }

                    base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.CreateSuccess, FormUI.GenericControl)));
                    if (!base.Url.IsLocalUrl(ReturnUrl) || ReturnUrl.Length <= 1 || !ReturnUrl.StartsWith("/") || ReturnUrl.StartsWith("//") || ReturnUrl.StartsWith("/\\"))
                    {
                        action = base.RedirectToAction("Index");
                    }
                    else
                    {
                        action = this.Redirect(ReturnUrl);
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                ExtentionUtils.Log(string.Concat("GenericControl.Create: ", exception.Message));
                return base.View(model);
            }
            return action;
        }

        [RequiredPermisson(Roles = "DeleteGenericControl")]
        public ActionResult Delete(string[] ids)
        {
            try
            {
                if (ids.Length != 0)
                {
                    IEnumerable<GenericControl> GenericControls =
                        from id in ids
                        select this._genericControlService.GetById(int.Parse(id));
                    this._genericControlService.BatchDelete(GenericControls);

                    //Delete localize
                    for (int i = 0; i < ids.Length; i++)
                    {
                        IEnumerable<LocalizedProperty> ieLocalizedProperty
                           = _localizedPropertyService.GetByEntityId(int.Parse(ids[i]));
                        this._localizedPropertyService.BatchDelete(ieLocalizedProperty);
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                ExtentionUtils.Log(string.Concat("ServerGenericControl.Delete: ", exception.Message));
            }
            return base.RedirectToAction("Index");
        }

        [RequiredPermisson(Roles = "CreateEditGenericControl")]
        public ActionResult Edit(int Id)
        {
            GenericControlViewModel modelMap = Mapper.Map<GenericControl, GenericControlViewModel>(this._genericControlService.GetById(Id));
            
            //Add Locales to model
            AddLocales(_languageService, modelMap.Locales, (locale, languageId) =>
            {
                locale.Id = modelMap.Id;
                locale.LocalesId = modelMap.Id;
                locale.Name = modelMap.GetLocalized(x => x.Name, Id, languageId, false, false);
                locale.Description = modelMap.GetLocalized(x => x.Description, Id, languageId, false, false);
            });

            return base.View(modelMap);
        }

        [HttpPost]
        [RequiredPermisson(Roles = "CreateEditGenericControl")]
        public ActionResult Edit(GenericControlViewModel model, string ReturnUrl)
        {
            ActionResult action;
            try
            {
                if (!base.ModelState.IsValid)
                {
                    String messages = String.Join(Environment.NewLine, ModelState.Values.SelectMany(v => v.Errors)
                                                         .Select(v => v.ErrorMessage + " " + v.Exception));
                    base.ModelState.AddModelError("", messages);
                    return base.View(model);
                }
                else
                {
                    GenericControl objGenericControl = _genericControlService.GetById(model.Id);

                    List<MenuLink> lstMenuLink = new List<MenuLink>();
                    List<int> lstMenuId = new List<int>();

                    string menuLink = this.Request["MenuLink"];
                    if (!string.IsNullOrEmpty(menuLink))
                    {
                        foreach (var item in menuLink.Split(new char[] { ',' }).ToList<string>())
                        {
                            int menuId = int.Parse(item);
                            lstMenuId.Add(menuId);

                            lstMenuLink.Add(_menuLinkService.GetById(menuId,isCache:false));
                        }

                        if (lstMenuId.IsAny())
                        {
                            (from x in objGenericControl.MenuLinks
                             where !lstMenuId.Contains(x.Id)
                             select x).ToList<MenuLink>()
                             .ForEach((MenuLink m) =>
                             objGenericControl.MenuLinks.Remove(m)
                             );
                        }
                    }
                    objGenericControl.MenuLinks = lstMenuLink;
                    
                    GenericControl modelMap = Mapper.Map<GenericControlViewModel, GenericControl>(model);
                    this._genericControlService.Update(objGenericControl);


                    //Update Localized   
                    foreach (var localized in model.Locales)
                    {
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Name, localized.Name, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Description, localized.Description, localized.LanguageId);
                    }

                    base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.UpdateSuccess, FormUI.GenericControl)));
                    if (!base.Url.IsLocalUrl(ReturnUrl) || ReturnUrl.Length <= 1 || !ReturnUrl.StartsWith("/") || ReturnUrl.StartsWith("//") || ReturnUrl.StartsWith("/\\"))
                    {
                        action = base.RedirectToAction("Index");
                    }
                    else
                    {
                        action = this.Redirect(ReturnUrl);
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                ExtentionUtils.Log(string.Concat("GenericControl.Create: ", exception.Message));
                return base.View(model);
            }
            return action;
        }

        [RequiredPermisson(Roles = "ViewGenericControl")]
        public ActionResult Index(int page = 1, string keywords = "")
        {
            ((dynamic)base.ViewBag).Keywords = keywords;
            SortingPagingBuilder sortingPagingBuilder = new SortingPagingBuilder()
            {
                Keywords = keywords,
                Sorts = new SortBuilder()
                {
                    ColumnName = "CreatedDate",
                    ColumnOrder = SortBuilder.SortOrder.Descending
                }
            };
            Paging paging = new Paging()
            {
                PageNumber = page,
                PageSize = base._pageSize,
                TotalRecord = 0
            };
            IEnumerable<App.Domain.Entities.GenericControl.GenericControl> GenericControls = this._genericControlService.PagedList(sortingPagingBuilder, paging);
            if (GenericControls != null && GenericControls.Any<App.Domain.Entities.GenericControl.GenericControl>())
            {
                Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => this.Url.Action("Index", new { page = i, keywords = keywords }));
                ((dynamic)base.ViewBag).PageInfo = pageInfo;
            }
            return base.View(GenericControls);
        }


        public JsonResult GetGenericControlByMenuId(int menuId)
        {
            IEnumerable<GenericControl> ieGenericControls = null;
            try
            {
                ieGenericControls = _genericControlService.GetByMenuId(menuId);

                //if (ieGenericControls.Count() == 0)
                //    return null;

                //int genericControlId = ieGenericControls.FirstOrDefault().Id;

                //genericControlValue = _genericControlValueService.FindBy((GenericControlValue x) => x.GenericControlId == genericControlId && x.Status == 1, false);

            }
            catch 
            {

            }

            JsonResult jsonResult = Json(
                new
                {
                    success = ieGenericControls.Count(),
                    list = this.RenderRazorViewToString("_CreateOrUpdate.GenericControl", ieGenericControls)
                },
                JsonRequestBehavior.AllowGet);

            return jsonResult;
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.RouteData.Values["action"].Equals("create"))
            {
                IEnumerable<MenuLink> lstMenuLink = _menuLinkService.GetAll();

                MenuLinkViewModel menuLinkViewModel = new MenuLinkViewModel();
                IEnumerable<MenuLinkViewModel> model = lstMenuLink.Select(m =>
                {
                    return m.ToModel(menuLinkViewModel);
                });

                ((dynamic)base.ViewBag).MenuLink = model;
            }

            if (filterContext.RouteData.Values["action"].Equals("edit"))
            {
                int id = int.Parse(filterContext.RouteData.Values["id"].ToString());

                IEnumerable<MenuLink> lstMenuLink = _menuLinkService.GetAll();

                IEnumerable<MenuLinkViewModel> model = from x in lstMenuLink
                                                       select new MenuLinkViewModel()
                                                       {
                                                           Id = x.Id,
                                                           ParentId = x.ParentId,
                                                           Status = x.Status,
                                                           TypeMenu = x.TypeMenu,
                                                           Position = x.Position,
                                                           MenuName = x.MenuName,
                                                           VirtualId = x.VirtualId,
                                                           Selected = x.SelectedMenu(id)
                                                       };

                ((dynamic)base.ViewBag).GCMenuLink = model;
            }
        }
    }
}