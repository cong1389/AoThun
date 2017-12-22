using App.Domain.Entities.Data;
using App.Service.Repairs;
using System.Web.Mvc;

namespace App.Admin.Controllers
{
    public class PrintController : Controller
    {
        private readonly IRepairService _orderService;

        public PrintController(IRepairService orderService)
        {
            this._orderService = orderService;
        }

        public ActionResult Bill(int id)
        {
            Repair order = this._orderService.Get((Repair x) => x.Id == id, false);
            return base.View(order);
        }

        public ActionResult Warranty(int id)
        {
            Repair order = this._orderService.Get((Repair x) => x.Id == id, false);
            return base.View(order);
        }
    }
}