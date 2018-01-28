using App.Admin.Helpers;
using App.Core.Caching;
using App.Core.Utils;
using App.Domain.Entities.GenericControl;
using App.Domain.Entities.Menu;
using App.FakeEntity.GenericControl;
using App.Framework.Ultis;
using App.Service.GenericControl;
using App.Service.Menu;
using App.Aplication;
using AutoMapper;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace App.Admin.Controllers
{
    public class GenericControlValueController : BaseAdminController
    {
        private const string CACHE_GENERICCONTROLVALUE_KEY = "db.GenericControlValue";
        private readonly ICacheManager _cacheManager;

        private readonly IGenericControlValueService _genericControlValueService;
        private readonly IGenericControlService _genericControlService;
        private readonly IMenuLinkService _menuLinkService;

        public GenericControlValueController(IGenericControlValueService GenericControlValueService
            , IGenericControlService genericControlService
            , IMenuLinkService menuLinkService
            , ICacheManager cacheManager)
        {
            this._genericControlValueService = GenericControlValueService;
            this._genericControlService = genericControlService;
            this._menuLinkService = menuLinkService;
            _cacheManager = cacheManager;

            //Clear cache
            _cacheManager.RemoveByPattern(CACHE_GENERICCONTROLVALUE_KEY);
        }

        [RequiredPermisson(Roles = "CreateEditGenericControlValue")]
        public ActionResult Create()
        {
            return base.View();
        }

        [HttpPost]
        [RequiredPermisson(Roles = "CreateEditGenericControlValue")]
        public ActionResult Create(GenericControlValueViewModel model, string ReturnUrl)
        {
            ActionResult action;
            try
            {
                if (!base.ModelState.IsValid)
                {
                    base.ModelState.AddModelError("", MessageUI.ErrorMessage);
                    return base.View(model);
                }
                else
                {
                    GenericControlValue modelMap = Mapper.Map<GenericControlValueViewModel, GenericControlValue>(model);
                    this._genericControlValueService.Create(modelMap);

                    base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.CreateSuccess, FormUI.Name)));
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
                ExtentionUtils.Log(string.Concat("GenericControlValue.Create: ", exception.Message));
                return base.View(model);
            }
            return action;
        }

        [RequiredPermisson(Roles = "DeleteGenericControlValue")]
        public ActionResult Delete(string[] ids)
        {
            try
            {
                if (ids.Length != 0)
                {
                    IEnumerable<GenericControlValue> GenericControlValues =
                        from id in ids
                        select this._genericControlValueService.GetById(int.Parse(id));
                    this._genericControlValueService.BatchDelete(GenericControlValues);
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                ExtentionUtils.Log(string.Concat("GenericControlValue.Delete: ", exception.Message));
            }
            return base.RedirectToAction("Index");
        }

        [RequiredPermisson(Roles = "CreateEditGenericControlValue")]
        public ActionResult Edit(int Id)
        {
            GenericControlValueViewModel genericControlValueViewModel = Mapper.Map<GenericControlValue, GenericControlValueViewModel>(this._genericControlValueService.GetById(Id));

            //_genericControlValueService.GetById(Id)


            return base.View(genericControlValueViewModel);
        }

        [HttpPost]
        [RequiredPermisson(Roles = "CreateEditGenericControlValue")]
        public ActionResult Edit(GenericControlValueViewModel model, string ReturnUrl)
        {
            ActionResult action;
            try
            {
                if (!base.ModelState.IsValid)
                {
                    base.ModelState.AddModelError("", MessageUI.ErrorMessage);
                    return base.View(model);
                }
                else
                {
                    GenericControlValue modelMap = Mapper.Map<GenericControlValueViewModel, GenericControlValue>(model);

                    _genericControlValueService.Update(modelMap);

                    base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.UpdateSuccess, FormUI.Name)));

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
                ExtentionUtils.Log(string.Concat("GenericControlValue.Edit: ", exception.Message));
                return base.View(model);
            }
            return action;
        }

        [RequiredPermisson(Roles = "ViewGenericControlValue")]
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
            List<GenericControlValue> list = this._genericControlValueService.PagedList(sortingPagingBuilder, paging).ToList<GenericControlValue>();
            list.ForEach((GenericControlValue item) => item.GenericControl = this._genericControlService.GetById(item.GenericControlId));
            if (list != null && list.Any<GenericControlValue>())
            {
                Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => this.Url.Action("Index", new { page = i, keywords = keywords }));
                ((dynamic)base.ViewBag).PageInfo = pageInfo;
            }
            return base.View(list);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.RouteData.Values["action"].Equals("create") || filterContext.RouteData.Values["action"].Equals("edit"))
            {
                IEnumerable<GenericControl> all = this._genericControlService.GetAll();
                ((dynamic)base.ViewBag).GenericControls = all;
            }
        }

        [HttpPost]
        public JsonResult GetByMenuId(int menuId, int entityId)
        {
            List<ControlValueItemResponse> lstValueResponse = new List<ControlValueItemResponse>();

            MenuLink menuLink = _menuLinkService.GetById(menuId);
            if (menuLink != null)
            {
                IEnumerable<GenericControl> ieGC = menuLink.GenericControls;
                if (ieGC.IsAny())
                {
                    foreach (GenericControl item in ieGC)
                    {
                        IEnumerable<GenericControlValue> gCVDefault = item.GenericControlValues.Where(m => m.Status == 1);
                        //if (gCVDefault.IsAny())
                        //    break;                       

                        foreach (var gcValue in gCVDefault)
                        {
                            ControlValueItemResponse objValueResponse = new ControlValueItemResponse();

                            objValueResponse.GenericControlValueId = gcValue.Id;
                            objValueResponse.Name = gcValue.ValueName;
                            objValueResponse.ValueName = gcValue.GetValueItem(entityId);

                            lstValueResponse.Add(objValueResponse);
                        }
                    }
                }
            }

            JsonResult jsonResult = Json(
                 new
                 {
                     success = lstValueResponse.Count() > 0,
                     list = this.RenderRazorViewToString("_CreateOrUpdate.GenericControlValue", lstValueResponse)
                 },
                 JsonRequestBehavior.AllowGet);

            return jsonResult;
        }

    }
}