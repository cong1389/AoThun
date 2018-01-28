using App.Core.Caching;
using App.Domain.Entities.GenericControl;
using App.Framework.Ultis;
using App.Service.GenericControl;
using App.Service.Menu;
using App.Aplication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using App.FakeEntity.GenericControl;

namespace App.Admin.Controllers
{
    public class GenericControlValueItemController : BaseAdminController
    {
        private const string CACHE_GENERICCONTROLVALUEITEM_KEY = "db.GenericControlValueItem";

        private readonly IGenericControlValueItemService _genericControlValueItemService;
        private readonly IGenericControlValueService _genericControlValueService;
        private readonly IMenuLinkService _menuLinkService;
        private readonly ICacheManager _cacheManager;

        public GenericControlValueItemController(IGenericControlValueItemService genericControlValueItemService
            , IGenericControlValueService genericControlValueService
            , IMenuLinkService menuLinkService
            , ICacheManager cacheManager)
        {
            this._genericControlValueItemService = genericControlValueItemService;
            this._genericControlValueService = genericControlValueService;
            this._menuLinkService = menuLinkService;
            _cacheManager = cacheManager;

            //Clear cache
            _cacheManager.RemoveByPattern(CACHE_GENERICCONTROLVALUEITEM_KEY);
        }

        [HttpPost]
        public JsonResult GetByMenuId(int menuId, int entityId)
        {
            IEnumerable<GenericControlValueItem> genericControlValueItem = null;

            genericControlValueItem = _genericControlValueItemService.GetByOption(genericControlValueId: menuId, entity: entityId);

            if (genericControlValueItem.IsAny())
            {

            }

            JsonResult jsonResult = Json(
                 new
                 {
                     success = genericControlValueItem.Count() > 0,
                     list = this.RenderRazorViewToString("_CreateOrUpdate.GenericControlValue", genericControlValueItem)
                 },
                 JsonRequestBehavior.AllowGet);

            return jsonResult;
        }


        [HttpPost]
        public JsonResult Insert(string param, int entityId)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<GenericControlValueResponse>(param);

                foreach (ControlValueItemResponse item in data.controlValueItemResponse)
                {
                    //Get data
                    GenericControlValueItem model = _genericControlValueItemService.Get((GenericControlValueItem x) => x.EntityId == entityId
                    && x.GenericControlValueId == item.GenericControlValueId);

                    if (model != null)
                    {
                        model.Title = item.Name;
                        model.Value = item.ValueName;
                        model.EntityId = entityId;
                        model.GenericControlValueId = item.GenericControlValueId;

                        _genericControlValueItemService.Update(model);
                    }
                    else
                    {
                        model = new GenericControlValueItem();

                        model.Title = item.Name;
                        model.Value = item.ValueName;
                        model.EntityId = entityId;
                        model.GenericControlValueId = item.GenericControlValueId;
                        model.Status = 1;
                        model.OrderDisplay = 1;

                        _genericControlValueItemService.Create(model);
                    }
                }
            }
            catch (Exception ex)
            {
                ExtentionUtils.Log(string.Concat("District.Edit: ", ex.Message));
            }
            JsonResult jsonResult = Json(new { success = true }, JsonRequestBehavior.AllowGet);

            return jsonResult;
        }

        [HttpPost]
        public JsonResult Update(string param)
        {
            try
            {
                //var data = JsonConvert.DeserializeObject<GenericControlValueResponseCollection>(param);

                //foreach (GenericControlValueItem item in data.GenericControlValueItem)
                //{
                //    //Get data
                //    GenericControlValueItem model = _genericControlValueItemService.Get((GenericControlValueItem x) => x.Id == item.Id && x.EntityId == item.EntityId);

                //    if (model != null)
                //    {
                //       // model = new GenericControlValueItem();
                //        model.ColorHex = item.ColorHex;
                //        model.EntityId = entityId;

                //        _genericControlValueItemService.Create(model);
                //    }
                //}
            }
            catch (Exception exception)
            {
                ExtentionUtils.Log(string.Concat("District.Edit: ", exception.Message));
            }
            JsonResult jsonResult = Json(new { success = true }, JsonRequestBehavior.AllowGet);

            return jsonResult;
        }

    }
}