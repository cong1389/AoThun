using App.Admin.Helpers;
using App.Aplication;
using App.Core.Utils;
using App.Domain.Entities.Data;
using App.FakeEntity.Step;
using App.Framework.Ultis;
using App.Service.Step;
using AutoMapper;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Core.Caching;
using System.IO;

namespace App.Admin.Controllers
{
    public class FlowStepController : BaseAdminController
    {
        private const string CACHE_FLOWSTEP_KEY = "db.FlowStep";
        private readonly ICacheManager _cacheManager;

        private readonly IFlowStepService _flowStepService;

		private IImagePlugin _imagePlugin;

		public FlowStepController(IFlowStepService flowStepService, IImagePlugin imagePlugin
            , ICacheManager cacheManager)
		{
			this._flowStepService = flowStepService;
			this._imagePlugin = imagePlugin;
            _cacheManager = cacheManager;

            //Clear cache
            _cacheManager.RemoveByPattern(CACHE_FLOWSTEP_KEY);

        }

        [RequiredPermisson(Roles="CreateEditFlowStep")]
		public ActionResult Create()
		{
			return base.View();
		}

		[HttpPost]
		[RequiredPermisson(Roles="CreateEditFlowStep")]
		public ActionResult Create(FlowStepViewModel model, string ReturnUrl)
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
                    string titleNonAccent = model.Title.NonAccent();
                    if (model.Image != null && model.Image.ContentLength > 0)
					{
                        string fileExtension = Path.GetExtension(model.Image.FileName);
                        string fileName1 = titleNonAccent.FileNameFormat(fileExtension);
      //                  int? nullable = null;
						//int? nullable1 = nullable;
						//nullable = null;

                        _imagePlugin.CropAndResizeImage(model.Image, string.Format("{0}", Contains.FlowStepFolder), fileName1, ImageSize.FlowStep_WithMediumSize, ImageSize.FlowStep_HeightMediumSize, false);

                        model.ImageUrl = string.Concat(Contains.FlowStepFolder, fileName1);
					}

					FlowStep flowStep = Mapper.Map<FlowStepViewModel, FlowStep>(model);

					this._flowStepService.Create(flowStep);

					base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.CreateSuccess, FormUI.FlowStep)));
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
				ExtentionUtils.Log(string.Concat("Post.Create: ", exception.Message));
				base.ModelState.AddModelError("", exception.Message);
				return base.View(model);
			}
			return action;
		}

		[RequiredPermisson(Roles="CreateEditFlowStep")]
		public ActionResult Delete(int[] ids)
		{
			try
			{
				if (ids.Length != 0)
				{
					IEnumerable<FlowStep> flowSteps = 
						from id in ids
						select this._flowStepService.Get((FlowStep x) => x.Id == id, false);
					this._flowStepService.BatchDelete(flowSteps);
				}
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				ExtentionUtils.Log(string.Concat("Post.Delete: ", exception.Message));
			}
			return base.RedirectToAction("Index");
		}

		[RequiredPermisson(Roles="CreateEditFlowStep")]
		public ActionResult Edit(int Id)
		{
			FlowStepViewModel flowStepViewModel = Mapper.Map<FlowStep, FlowStepViewModel>(this._flowStepService.Get((FlowStep x) => x.Id == Id, false));
			return base.View(flowStepViewModel);
		}

		[HttpPost]
		[RequiredPermisson(Roles="CreateEditFlowStep")]
		public ActionResult Edit(FlowStepViewModel model, string ReturnUrl)
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
					FlowStep byId = this._flowStepService.Get((FlowStep x) => x.Id == model.Id, false);

					string titleNonAccent = model.Title.NonAccent();
					if (model.Image != null && model.Image.ContentLength > 0)
					{
                        string fileExtension = Path.GetExtension(model.Image.FileName);
                        
                        string fileName1 = titleNonAccent.FileNameFormat(fileExtension);
      //                  int? nullable = null;
						//int? nullable1 = nullable;
						//nullable = null;

						this._imagePlugin.CropAndResizeImage(model.Image, string.Format("{0}", Contains.FlowStepFolder), fileName1, ImageSize.FlowStep_WithMediumSize, ImageSize.FlowStep_HeightMediumSize, false);

                        model.ImageUrl = string.Concat(Contains.FlowStepFolder, fileName1);
					}

					FlowStep flowStep1 = Mapper.Map(model, byId);

                    _flowStepService.Update(flowStep1);

					base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.UpdateSuccess, FormUI.FlowStep)));
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
				ExtentionUtils.Log(string.Concat("Post.Edit: ", exception.Message));
				return base.View(model);
			}
			return action;
		}

		[RequiredPermisson(Roles="ViewFlowStep")]
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
			IEnumerable<FlowStep> flowSteps = this._flowStepService.PagedList(sortingPagingBuilder, paging);
			if (flowSteps != null && flowSteps.Any<FlowStep>())
			{
				Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => this.Url.Action("Index", new { page = i, keywords = keywords }));
				((dynamic)base.ViewBag).PageInfo = pageInfo;
			}
			return base.View(flowSteps);
		}
	}
}