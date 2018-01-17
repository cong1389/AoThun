using App.Front.Models;
using App.Service.Attribute;
using System.Collections.Generic;
using System.Web.Mvc;

namespace App.Front.Controllers
{
    public class SearchBoxController : Controller
    {
        private IAttributeService _attributeService;

        public SearchBoxController(IAttributeService attributeService)
        {
            this._attributeService = attributeService;
        }

        public ActionResult SearchMenu()
        {
            SeachConditions seachConditions = new SeachConditions();

            return PartialView(seachConditions);
        }

        public ActionResult SearchMenuMobile()
        {
            SeachConditions seachConditions = new SeachConditions();

            return PartialView(seachConditions);
        }

        public ActionResult GetAttributeSearchBox(List<int> attributes = null)
        {
            ((dynamic)base.ViewBag).Attributes = attributes;

            IEnumerable<App.Domain.Entities.Attribute.Attribute> ieAttributes = this._attributeService.FindBy((App.Domain.Entities.Attribute.Attribute x) => x.Status == 1, false);

            return base.PartialView(ieAttributes);
        }

        public ActionResult SearchBoxSideBar(int? productOld, int? productNew, List<int> attributes = null, List<double> prices = null, List<int> proids = null)
        {
            ((dynamic)base.ViewBag).Attributes = attributes;
            ((dynamic)base.ViewBag).ProAttrs = proids;
            ((dynamic)base.ViewBag).Prices = prices;
            ((dynamic)base.ViewBag).ProductOld = productOld;
            ((dynamic)base.ViewBag).ProductNew = productNew;
            return base.PartialView();
        }
    }
}