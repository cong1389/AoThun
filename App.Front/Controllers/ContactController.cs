using App.Aplication.Extensions;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Entities.Menu;
using App.Front.Models;
using App.Service.ContactInformation;
using App.Service.Language;
using App.Service.Menu;
using System.Collections.Generic;
using System.Web.Mvc;

namespace App.Front.Controllers
{
    public class ContactController : FrontBaseController
    {
        private readonly IContactInfoService _contactInfoService;
        private readonly IMenuLinkService _menuLinkService;

        public ContactController(IContactInfoService contactInfoService, IMenuLinkService menuLinkService)
        {
            this._contactInfoService = contactInfoService;
            this._menuLinkService = menuLinkService;
        }

        [ChildActionOnly]
        public ActionResult ContactUs(int id, string title)
        {
            MenuLink menuLink = this._menuLinkService.GetById(Id: id);

            ContactInformation contactInformation = this._contactInfoService.Get((ContactInformation x) => x.Type == 1 && x.Status == 1, true);

            if (contactInformation == null)
                return HttpNotFound();

            ContactInformation contactInformationLocalize = contactInformation.ToModel();

            List<BreadCrumb> breadCrumbs = new List<BreadCrumb>();
            if (menuLink != null)
            {

                breadCrumbs.Add(new BreadCrumb()
                {
                    Title = menuLink.GetLocalized(m => m.MenuName, menuLink.Id),
                    Current = false,
                    Url = base.Url.Action("GetContent", "Menu", new { area = "", menu = menuLink.SeoUrl })
                });               
                ((dynamic)base.ViewBag).MenuId = menuLink.Id;
            }
            ((dynamic)base.ViewBag).BreadCrumb = breadCrumbs;

            ((dynamic)base.ViewBag).Title = title;
            ((dynamic)base.ViewBag).Contact = contactInformationLocalize;          

            return base.PartialView(menuLink);
        }
    }
}