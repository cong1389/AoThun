using App.Domain.Entities.Data;
using App.Service.Step;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Front.Controllers
{
    public class FlowStepController : Controller
    {
        private readonly IFlowStepService _flowStepService;
        public FlowStepController(IFlowStepService flowStepService)
        {
            _flowStepService = flowStepService;
        }

        public ActionResult FlowStepHome()
        {
            IEnumerable<FlowStep> flowSteps = this._flowStepService.FindBy((FlowStep x) => x.Status == 1, false);

            return base.PartialView(flowSteps);
        }

    }
}