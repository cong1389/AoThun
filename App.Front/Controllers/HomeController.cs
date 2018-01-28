using App.Aplication;
using App.Core.Utils;
using App.Domain.Entities.Brandes;
using App.Domain.Entities.Data;
using App.Domain.Entities.GlobalSetting;
using App.FakeEntity.Repairs;
using App.Front.Models;
using App.Service.Brandes;
using App.Service.Locations;
using App.Service.MailSetting;
using App.Service.Repairs;
using App.Service.SystemApp;
using App.Aplication.MVCHelper;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace App.Front.Controllers
{
    public class HomeController : FrontBaseController
    {
        private readonly ISystemSettingService _systemSettingService;

        private readonly IMailSettingService _mailSettingService;

        private readonly IProvinceService _province;

        private readonly IBrandService _brandService;

        private readonly IRepairService _orderService;

        private readonly IImagePlugin _imagePlugin;

        public HomeController(ISystemSettingService systemSettingService, IProvinceService province, IMailSettingService mailSettingService, IBrandService brandService, IImagePlugin imagePlugin, IRepairService orderService)
        {
            this._systemSettingService = systemSettingService;
            this._province = province;
            this._mailSettingService = mailSettingService;
            this._brandService = brandService;
            this._imagePlugin = imagePlugin;
            this._orderService = orderService;
        }

        public ActionResult BuyProduct(RepairViewModel repair)
        {
            int id = 1;
            Repair order1 = this._orderService.GetTop<int>(1, (Repair x) => x.Id > 0, (Repair x) => x.Id).FirstOrDefault<Repair>();
            if (order1 != null)
            {
                id = order1.Id;
            }
            repair.RepairCode = string.Concat("DH", id.ToString());
            repair.CustomerCode = string.Concat("KH", id.ToString());
            repair.Category = "Định giá và thu mua sản phẩm";
            if (!base.Request.IsAjaxRequest())
            {
                return base.Json(new
                {
                    success = false,
                    errors = string.Join(", ", base.ModelState.Values.SelectMany<System.Web.Mvc.ModelState, string>((System.Web.Mvc.ModelState v) =>
from x in v.Errors
select x.ErrorMessage).ToArray<string>())
                });
            }
            HttpFileCollectionBase files = base.Request.Files;
            List<RepairGallery> orderGalleries = new List<RepairGallery>();
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
                                RepairGalleryViewModel orderGalleryViewModel = new RepairGalleryViewModel()
                                {
                                    RepairId = repair.Id
                                };
                                string str1 = string.Format("{0}-{1}.jpg", repair.RepairCode, Guid.NewGuid());
                                this._imagePlugin.CropAndResizeImage(item, string.Format("{0}{1}/", Contains.ImageFolder, repair.RepairCode), str1, ImageSize.WithBigSize, ImageSize.WithBigSize, false);
                                orderGalleryViewModel.ImagePath = string.Format("{0}{1}/{2}", Contains.ImageFolder, repair.RepairCode, str1);
                                orderGalleries.Add(Mapper.Map<RepairGallery>(orderGalleryViewModel));
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
            Repair order2 = Mapper.Map<RepairViewModel, Repair>(repair);
            if (orderGalleries.IsAny<RepairGallery>())
            {
                order2.RepairGalleries = orderGalleries;
            }
            this._orderService.Create(order2);
            return base.Json(new { success = true });
        }

        public ActionResult CheckRepair(string phone, string ordercode)
        {
            if (!base.Request.IsAjaxRequest())
            {
                return base.Json(new { data = this.RenderRazorViewToString("_RepairResult", null), success = true }, JsonRequestBehavior.AllowGet);
            }
            Expression<Func<Repair, bool>> expression = PredicateBuilder.True<Repair>();
            if (!string.IsNullOrEmpty(phone))
            {
                expression = expression.And<Repair>((Repair x) => x.PhoneNumber == phone).Or<Repair>((Repair x) => x.CustomerName.ToLower().Equals(phone));
            }
            if (!string.IsNullOrEmpty(ordercode))
            {
                expression = expression.And<Repair>((Repair x) => x.RepairCode.ToLower() == ordercode);
            }
            Repair repair = this._orderService.Get(expression, true);
            return base.Json(new { data = this.RenderRazorViewToString("_RepairResult", repair), success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckProduct(RepairViewModel repair)
        {
            int id = 1;
            Repair order1 = this._orderService.GetTop<int>(1, (Repair x) => x.Id > 0, (Repair x) => x.Id).FirstOrDefault<Repair>();
            if (order1 != null)
            {
                id = order1.Id;
            }
            repair.RepairCode = string.Concat("DH", id.ToString());
            repair.CustomerCode = string.Concat("KH", id.ToString());
            repair.Category = "kiểm tra tình trạng máy";
            if (!base.Request.IsAjaxRequest())
            {
                return base.Json(new
                {
                    success = false,
                    errors = string.Join(", ", base.ModelState.Values.SelectMany<System.Web.Mvc.ModelState, string>((System.Web.Mvc.ModelState v) =>
from x in v.Errors
select x.ErrorMessage).ToArray<string>())
                });
            }
            HttpFileCollectionBase files = base.Request.Files;
            List<RepairGallery> orderGalleries = new List<RepairGallery>();
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
                                RepairGalleryViewModel orderGalleryViewModel = new RepairGalleryViewModel()
                                {
                                    RepairId = repair.Id
                                };
                                string str1 = string.Format("{0}-{1}.jpg", repair.RepairCode, Guid.NewGuid());
                                this._imagePlugin.CropAndResizeImage(item, string.Format("{0}{1}/", Contains.ImageFolder, repair.RepairCode), str1, ImageSize.WithBigSize, ImageSize.WithBigSize, false);
                                orderGalleryViewModel.ImagePath = string.Format("{0}{1}/{2}", Contains.ImageFolder, repair.RepairCode, str1);
                                orderGalleries.Add(Mapper.Map<RepairGallery>(orderGalleryViewModel));
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
            Repair order2 = Mapper.Map<RepairViewModel, Repair>(repair);
            if (orderGalleries.IsAny<RepairGallery>())
            {
                order2.RepairGalleries = orderGalleries;
            }
            this._orderService.Create(order2);
            return base.Json(new { success = true });
        }

        public ActionResult Error()
        {
            return base.View();
        }

        public ActionResult FlowForm()
        {
            dynamic viewBag = base.ViewBag;
            IBrandService brandService = this._brandService;
            viewBag.Brands = brandService.FindBy((Brand x) => x.Status == 1, false);
            return base.PartialView();
        }

        [OutputCache(CacheProfile = "Medium")]
        public ActionResult Index()
        {
            SystemSetting systemSetting = this._systemSettingService.Get((SystemSetting x) => x.Status == 1, true);
            if (systemSetting != null)
            {
                ((dynamic)base.ViewBag).Title = systemSetting.MetaTitle ?? systemSetting.Title;
                ((dynamic)base.ViewBag).KeyWords = systemSetting.MetaKeywords;
                ((dynamic)base.ViewBag).SiteUrl = base.Url.Action("Index", "Home", new { area = "" });
                ((dynamic)base.ViewBag).Description = systemSetting.Description;
                ((dynamic)base.ViewBag).Image = base.Url.Content(string.Concat("~/", systemSetting.LogoImage));
                ((dynamic)base.ViewBag).Favicon = base.Url.Content(string.Concat("~/", systemSetting.FaviconImage));
            }
            dynamic viewBag = base.ViewBag;

            //IProvinceService provinceService = this._province;
            //viewBag.Provinces = provinceService.FindBy((Province x) => x.Status == 1, false);
            return base.View();
        }

        [HttpPost]
        public ActionResult SendContact(string name, string email, string content)
        {
            ActionResult actionResult;
            try
            {
                if (!base.Request.IsAjaxRequest())
                {
                    return base.Json(new { succes = false, message = "Liên hệ chưa gửi được, vui lòng thử lại." });
                }
                else
                {
                    ServerMailSetting serverMailSetting = this._mailSettingService.Get((ServerMailSetting x) => x.Status == 1, false);
                    string str = this._systemSettingService.Get((SystemSetting x) => x.Status == 1, false).Email;
                    string str1 = "Thông tin liên hệ";
                    string str2 = string.Concat("", "Người gửi: ", name, "<br>");
                    str2 = string.Concat(str2, string.Concat("E-mail: ", email), "<br>");
                    str2 = string.Concat(str2, content, "<br>");
                    SendMail sendMail = new SendMail();
                    sendMail.InitMail(serverMailSetting.FromAddress, serverMailSetting.SmtpClient, serverMailSetting.UserID, serverMailSetting.Password, serverMailSetting.SMTPPort, serverMailSetting.EnableSSL);
                    sendMail.SendToMail("Email", str, new string[] { str1, str2 });
                    actionResult = base.Json(new { success = true, message = "Gửi liên hệ thành công, chúng tôi sẽ liên lạc với bạn ngay khi có thể." });
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            return actionResult;
        }
    }
}