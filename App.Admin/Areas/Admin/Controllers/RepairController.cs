using App.Admin.Helpers;
using App.Aplication;
using App.Core.Utils;
using App.Domain.Entities.Brandes;
using App.Domain.Entities.Data;
using App.FakeEntity.Repairs;
using App.Framework.Ultis;
using App.Service.Attribute;
using App.Service.Brandes;
using App.Service.Menu;
using App.Service.Repairs;
using App.Service.Repair;
using AutoMapper;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Core.Caching;

namespace App.Admin.Controllers
{
    public class RepairController : BaseAdminUploadController
    {
        private readonly IBrandService _brandService;

        private readonly IRepairGalleryService _galleryService;

        private readonly IImagePlugin _imagePlugin;

        private readonly IMenuLinkService _menuLinkService;

        private readonly IRepairService _repairService;

        private readonly IRepairItemService _repairItemService;


        public RepairController(IRepairService repairService, IMenuLinkService menuLinkService, IAttributeValueService attributeValueService, IRepairGalleryService galleryService
            , IImagePlugin imagePlugin, IBrandService brandService, IRepairItemService repairItemService
            , ICacheManager cacheManager)
            : base(cacheManager)
        {
            this._repairService = repairService;
            this._menuLinkService = menuLinkService;
            this._galleryService = galleryService;
            this._imagePlugin = imagePlugin;
            this._brandService = brandService;
            this._repairItemService = repairItemService;
        }

        [RequiredPermisson(Roles = "CreateEditRepair")]
        public ActionResult Create()
        {
            int id = 1;
            Repair repair = this._repairService.GetTop<int>(1, (Repair x) => x.RepairCode != null, (Repair x) => x.Id).FirstOrDefault<Repair>();
            if (repair != null)
            {
                id = repair.Id;
            }
            return base.View(new RepairViewModel()
            {
                RepairCode = string.Concat("DH", id.ToString()),
                CustomerCode = string.Concat("KH", id.ToString())
            });
        }

        [HttpPost]
        [RequiredPermisson(Roles = "CreateEditRepair")]
        [ValidateInput(false)]
        public ActionResult Create([Bind] RepairViewModel repair, string ReturnUrl)
        {
            ActionResult action;
            try
            {
                if (!base.ModelState.IsValid)
                {
                    base.ModelState.AddModelError("", MessageUI.ErrorMessage);
                    return base.View(repair);
                }
                else
                {
                    HttpFileCollectionBase files = base.Request.Files;
                    List<RepairGallery> repairGalleries = new List<RepairGallery>();
                    if (files.Count > 0)
                    {
                        int count = files.Count - 1;
                        int num = 0;
                        string[] allKeys = files.AllKeys;
                        for (int i = 0; i < (int)allKeys.Length; i++)
                        {
                            string str = allKeys[i];
                            if (num <= count)
                            {
                                if (!str.Equals("Image"))
                                {
                                    HttpPostedFileBase httpPostedFileBase = files[num];
                                    if (httpPostedFileBase.ContentLength > 0)
                                    {
                                        RepairGalleryViewModel repairGalleryViewModel = new RepairGalleryViewModel()
                                        {
                                            RepairId = repair.Id
                                        };
                                        string str1 = string.Format("{0}-{1}.jpg", repair.RepairCode, Guid.NewGuid());
                                        this._imagePlugin.CropAndResizeImage(httpPostedFileBase, string.Format("{0}{1}/", Contains.ImageFolder, repair.RepairCode), str1, ImageSize.WithBigSize, ImageSize.WithBigSize, false);
                                        repairGalleryViewModel.ImagePath = string.Format("{0}{1}/{2}", Contains.ImageFolder, repair.RepairCode, str1);
                                        repairGalleries.Add(Mapper.Map<RepairGallery>(repairGalleryViewModel));
                                    }
                                    num++;
                                }
                                else
                                {
                                    num++;
                                }
                            }
                        }
                    }
                    Repair repair1 = Mapper.Map<RepairViewModel, Repair>(repair);
                    if (repairGalleries.IsAny<RepairGallery>())
                    {
                        repair1.RepairGalleries = repairGalleries;
                    }
                    List<RepairItem> repairItems = new List<RepairItem>();
                    if (repair.RepairItems.IsAny<RepairItemViewModel>())
                    {
                        repairItems.AddRange(
                            from item in repair.RepairItems
                            select Mapper.Map<RepairItem>(item));
                    }
                    if (repairItems.IsAny<RepairItem>())
                    {
                        repair1.RepairItems = repairItems;
                    }
                    this._repairService.Create(repair1);
                    base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.CreateSuccess, FormUI.Repair)));
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
                ExtentionUtils.Log(string.Concat("Repair.Create: ", exception.Message));
                base.ModelState.AddModelError("", exception.Message);
                return base.View(repair);
            }
            return action;
        }

        [RequiredPermisson(Roles = "DeleteRepair")]
        public ActionResult Delete(string[] ids)
        {
            try
            {
                if (ids.Length != 0)
                {
                    List<Repair> repairs = new List<Repair>();
                    List<RepairGallery> repairGalleries = new List<RepairGallery>();
                    string[] strArrays = ids;
                    for (int i = 0; i < (int)strArrays.Length; i++)
                    {
                        int num = int.Parse(strArrays[i]);
                        Repair repair = this._repairService.Get((Repair x) => x.Id == num, false);
                        repairGalleries.AddRange(repair.RepairGalleries.ToList<RepairGallery>());
                        repairs.Add(repair);
                    }
                    this._galleryService.BatchDelete(repairGalleries);
                    this._repairService.BatchDelete(repairs);
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                ExtentionUtils.Log(string.Concat("Repair.Delete: ", exception.Message));
            }
            return base.RedirectToAction("Index");
        }

        [RequiredPermisson(Roles = "CreateEditRepair")]
        public ActionResult DeleteGallery(int RepairId, int galleryId)
        {
            ActionResult actionResult;
            if (!base.Request.IsAjaxRequest())
            {
                return base.Json(new { success = false });
            }
            try
            {
                RepairGallery repairGallery = this._galleryService.Get((RepairGallery x) => x.RepairId == RepairId && x.Id == galleryId, false);
                this._galleryService.Delete(repairGallery);
                string str = base.Server.MapPath(string.Concat("~/", repairGallery.ImagePath));
                string str1 = base.Server.MapPath(string.Concat("~/", repairGallery.ImagePath));
                System.IO.File.Delete(str);
                System.IO.File.Delete(str1);
                actionResult = base.Json(new { success = true });
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                actionResult = base.Json(new { success = false, messages = exception.Message });
            }
            return actionResult;
        }

        [RequiredPermisson(Roles = "CreateEditRepair")]
        public ActionResult Edit(int Id)
        {
            RepairViewModel repairViewModel = Mapper.Map<Repair, RepairViewModel>(this._repairService.Get((Repair x) => x.Id == Id, false));
            return base.View(repairViewModel);
        }

        [HttpPost]
        [RequiredPermisson(Roles = "CreateEditRepair")]
        [ValidateInput(false)]
        public ActionResult Edit([Bind] RepairViewModel repairView, string ReturnUrl)
        {
            ActionResult action;
            try
            {
                if (!base.ModelState.IsValid)
                {
                    base.ModelState.AddModelError("", MessageUI.ErrorMessage);
                    return base.View(repairView);
                }
                else
                {
                    Repair repair = this._repairService.Get((Repair x) => x.Id == repairView.Id, false);
                    HttpFileCollectionBase files = base.Request.Files;
                    List<RepairGallery> lstRepairGalleries = new List<RepairGallery>();
                    if (files.Count > 0)
                    {
                        int count = files.Count - 1;
                        int num = 0;
                        string[] allKeys = files.AllKeys;
                        for (int i = 0; i < (int)allKeys.Length; i++)
                        {
                            string str = allKeys[i];
                            if (num <= count)
                            {
                                if (!str.Equals("Image"))
                                {
                                    HttpPostedFileBase item = files[num];
                                    if (item.ContentLength > 0)
                                    {
                                        RepairGalleryViewModel repairGalleryViewModel = new RepairGalleryViewModel()
                                        {
                                            RepairId = repairView.Id
                                        };
                                        string str1 = string.Format("{0}-{1}.jpg", repairView.RepairCode, Guid.NewGuid());
                                        this._imagePlugin.CropAndResizeImage(item, string.Format("{0}{1}/", Contains.ImageFolder, repairView.RepairCode), str1, ImageSize.WithBigSize, ImageSize.WithBigSize, false);
                                        repairGalleryViewModel.ImagePath = string.Format("{0}{1}/{2}", Contains.ImageFolder, repairView.RepairCode, str1);
                                        lstRepairGalleries.Add(Mapper.Map<RepairGallery>(repairGalleryViewModel));
                                    }
                                    num++;
                                }
                                else
                                {
                                    num++;
                                }
                            }
                        }
                    }
                    if (lstRepairGalleries.IsAny<RepairGallery>())
                    {
                        repair.RepairGalleries = lstRepairGalleries;
                    }
                    List<RepairItem> repairItems = new List<RepairItem>();
                    if (repairView.RepairItems.IsAny<RepairItemViewModel>())
                    {
                        foreach (RepairItemViewModel repairItem in repairView.RepairItems)
                        {
                            RepairItemViewModel repairItemViewModel = new RepairItemViewModel();
                            if (repairItem.Id > 0)
                            {
                                repairItemViewModel.Id = repairItem.Id;
                            }
                            repairItemViewModel.RepairId = repairView.Id;
                            repairItemViewModel.FixedFee = repairItem.FixedFee;
                            repairItemViewModel.Category = repairItem.Category;
                            repairItemViewModel.WarrantyFrom = repairItem.WarrantyFrom;
                            repairItemViewModel.WarrantyTo = repairItem.WarrantyTo;
                            repairItems.Add(Mapper.Map<RepairItem>(repairItemViewModel));
                        }
                    }
                    if (repairItems.IsAny<RepairItem>())
                    {
                        repair.RepairItems = repairItems;
                    }
                    IEnumerable<RepairItem> repairItems1 = this._repairItemService.FindBy((RepairItem x) => x.RepairId == repairView.Id, false);
                    this._repairItemService.BatchDelete(repairItems1);
                    repair = Mapper.Map<RepairViewModel, Repair>(repairView, repair);
                    this._repairService.Update(repair);
                    base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.UpdateSuccess, FormUI.Repair)));
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
                base.ModelState.AddModelError("", exception.Message);
                ExtentionUtils.Log(string.Concat("Repair.Edit: ", exception.Message));
                return base.View(repairView);
            }
            return action;
        }

        [RequiredPermisson(Roles = "ViewRepair")]
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
            IEnumerable<Repair> repairs = this._repairService.PagedList(sortingPagingBuilder, paging);
            if (repairs != null && repairs.Any<Repair>())
            {
                Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => this.Url.Action("Index", new { page = i, keywords = keywords }));
                ((dynamic)base.ViewBag).PageInfo = pageInfo;
            }
            return base.View(repairs);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.RouteData.Values["action"].Equals("edit") || filterContext.RouteData.Values["action"].Equals("create"))
            {
                dynamic viewBag = base.ViewBag;
                IBrandService brandService = this._brandService;
                viewBag.Brands = brandService.FindBy((Brand x) => x.Status == 1, false);
            }
        }

        public ActionResult WarrantyForm()
        {
            RepairViewModel repairViewModel = new RepairViewModel();
            repairViewModel.RepairItems.Add(new RepairItemViewModel());
            return base.View(repairViewModel);
        }
    }
}