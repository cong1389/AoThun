using App.Admin.Helpers;
using App.Aplication;
using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Entities.Language;
using App.Domain.Entities.Menu;
using App.FakeEntity.Menu;
using App.FakeEntity.News;
using App.Framework.Ultis;
using App.Service.Language;
using App.Service.LocalizedProperty;
using App.Service.Menu;
using App.Service.News;
using AutoMapper;
using Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Core.Caching;

/// <summary>
/// Khi get value từ tầng service nhớ, isCache:false
/// </summary>
namespace App.Admin.Controllers
{
    public class NewsController : BaseAdminController
    {
        private const string CACHE_NEWS_KEY = "db.News";
        private readonly ICacheManager _cacheManager;

        private readonly IMenuLinkService _menuLinkService;

        private readonly INewsService _newsService;

        private IImagePlugin _imagePlugin;

        private readonly ILanguageService _languageService;

        private readonly ILocalizedPropertyService _localizedPropertyService;

        public NewsController(
            INewsService newsService
            , IMenuLinkService menuLinkService
            , IImagePlugin imagePlugin
            , ILanguageService languageService
            , ILocalizedPropertyService localizedPropertyService
            , ICacheManager cacheManager)
        {
            this._newsService = newsService;
            this._menuLinkService = menuLinkService;
            this._imagePlugin = imagePlugin;
            this._languageService = languageService;
            this._localizedPropertyService = localizedPropertyService;
            _cacheManager = cacheManager;

            //Clear cache
            _cacheManager.RemoveByPattern(CACHE_NEWS_KEY);
        }


        [RequiredPermisson(Roles = "CreateEditNews")]
        public ActionResult Create()
        {
            var model = new NewsViewModel();

            //Add locales to model
            AddLocales(_languageService, model.Locales);

            return base.View(model);
        }

        [HttpPost]
        [RequiredPermisson(Roles = "CreateEditNews")]
        public ActionResult Create(NewsViewModel model, string ReturnUrl)
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
                    string titleNonAccent = model.Title.NonAccent();
                    IEnumerable<News> bySeoUrl = this._newsService.GetBySeoUrl(titleNonAccent);
                    model.SeoUrl = model.Title.NonAccent();
                    if (bySeoUrl.Any<News>((News x) => x.Id != model.Id))
                    {
                        NewsViewModel newsViewModel = model;
                        newsViewModel.SeoUrl = string.Concat(newsViewModel.SeoUrl, "-", bySeoUrl.Count<News>());
                    }

                    string folderName = string.Format("{0:ddMMyyyy}", DateTime.UtcNow);
                    if (model.Image != null && model.Image.ContentLength > 0)
                    {
                        string fileExtension = Path.GetExtension(model.Image.FileName);

                        string fileName1 = titleNonAccent.FileNameFormat(fileExtension);
                        string fileName2 = titleNonAccent.FileNameFormat(fileExtension);
                        string fileName3 = titleNonAccent.FileNameFormat(fileExtension);

                        this._imagePlugin.CropAndResizeImage(model.Image, string.Format("{0}{1}/", Contains.NewsFolder, folderName), fileName1, ImageSize.WithBigSize, ImageSize.HeightBigSize, false);
                        this._imagePlugin.CropAndResizeImage(model.Image, string.Format("{0}{1}/", Contains.NewsFolder, folderName), fileName2, ImageSize.WithMediumSize, ImageSize.HeightMediumSize, false);
                        this._imagePlugin.CropAndResizeImage(model.Image, string.Format("{0}{1}/", Contains.NewsFolder, folderName), fileName3, ImageSize.WithSmallSize, ImageSize.HeightSmallSize, false);

                        model.ImageBigSize = string.Format("{0}{1}/{2}", Contains.NewsFolder, folderName, fileName1);
                        model.ImageMediumSize = string.Format("{0}{1}/{2}", Contains.NewsFolder, folderName, fileName2);
                        model.ImageSmallSize = string.Format("{0}{1}/{2}", Contains.NewsFolder, folderName, fileName3);
                    }

                    if (model.MenuId > 0)
                    {
                        MenuLink byId = this._menuLinkService.GetById(model.MenuId, isCache: false);
                        model.VirtualCatUrl = byId.VirtualSeoUrl;
                        model.VirtualCategoryId = byId.VirtualId;
                    }

                    News modelMap = Mapper.Map<NewsViewModel, News>(model);
                    this._newsService.Create(modelMap);

                    //Update Localized   
                    foreach (var localized in model.Locales)
                    {
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Title, localized.Title, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.ShortDesc, localized.ShortDesc, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Description, localized.Description, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.SeoUrl, localized.Title.NonAccent(), localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaTitle, localized.MetaTitle, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaKeywords, localized.MetaKeywords, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaDescription, localized.MetaDescription, localized.LanguageId);
                    }

                    base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.CreateSuccess, FormUI.News)));
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
            catch (Exception ex)
            {
                ExtentionUtils.Log(string.Concat("Post.Create: ", ex.Message));
                base.ModelState.AddModelError("", ex.Message);
                return base.View(model);
            }

            return action;
        }

        [RequiredPermisson(Roles = "DeleteNews")]
        public ActionResult Delete(int[] ids)
        {
            try
            {
                if (ids.Length != 0)
                {
                    IEnumerable<News> news =
                        from id in ids
                        select this._newsService.GetById(id);
                    this._newsService.BatchDelete(news);

                    //Delete localize
                    for (int i = 0; i < ids.Length; i++)
                    {
                        IEnumerable<LocalizedProperty> ieLocalizedProperty
                           = _localizedPropertyService.GetByEntityId(ids[i]);
                        this._localizedPropertyService.BatchDelete(ieLocalizedProperty);
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                ExtentionUtils.Log(string.Concat("Post.Delete: ", exception.Message));
            }
            return base.RedirectToAction("Index");
        }

        [RequiredPermisson(Roles = "CreateEditNews")]
        public ActionResult Edit(int Id)
        {
            NewsViewModel modelMap = Mapper.Map<News, NewsViewModel>(this._newsService.GetById(Id));

            //Add Locales to model
            AddLocales(_languageService, modelMap.Locales, (locale, languageId) =>
            {
                locale.Id = modelMap.Id;
                locale.LocalesId = modelMap.Id;
                locale.Title = modelMap.GetLocalized(x => x.Title, Id, languageId, false, false);
                locale.ShortDesc = modelMap.GetLocalized(x => x.ShortDesc, Id, languageId, false, false);
                locale.Description = modelMap.GetLocalized(x => x.Description, Id, languageId, false, false);
                locale.MetaTitle = modelMap.GetLocalized(x => x.MetaTitle, Id, languageId, false, false);
                locale.MetaKeywords = modelMap.GetLocalized(x => x.MetaKeywords, Id, languageId, false, false);
                locale.MetaDescription = modelMap.GetLocalized(x => x.MetaDescription, Id, languageId, false, false);
                locale.SeoUrl = modelMap.GetLocalized(x => x.SeoUrl, Id, languageId, false, false);
            });

            return base.View(modelMap);
        }

        [HttpPost]
        [RequiredPermisson(Roles = "CreateEditNews")]
        public ActionResult Edit(NewsViewModel model, string ReturnUrl)
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
                    News byId = this._newsService.GetById(model.Id, isCache: false);

                    string titleNonAccent = model.Title.NonAccent();
                    IEnumerable<MenuLink> bySeoUrl = this._menuLinkService.GetListSeoUrl(titleNonAccent, isCache: false);
                    model.SeoUrl = model.Title.NonAccent();
                    
                    if (bySeoUrl.Any<MenuLink>((MenuLink x) => x.Id != model.Id))
                    {
                        NewsViewModel newsViewModel = model;
                        newsViewModel.SeoUrl = string.Concat(newsViewModel.SeoUrl, "-", bySeoUrl.Count<MenuLink>());
                    }

                    string folderName = string.Format("{0:ddMMyyyy}", DateTime.UtcNow);
                    if (model.Image != null && model.Image.ContentLength > 0)
                    {
                        string fileExtension = Path.GetExtension(model.Image.FileName);

                        string fileName1 = string.Concat(titleNonAccent, ".jpg");
                        string fileName2 = string.Format("{0}-{1}.jpg", titleNonAccent, Guid.NewGuid());
                        string fileName3 = string.Format("{0}-{1}.jpg", titleNonAccent, Guid.NewGuid());

                        this._imagePlugin.CropAndResizeImage(model.Image, string.Format("{0}{1}/", Contains.NewsFolder, folderName), fileName1, ImageSize.WithBigSize, ImageSize.HeightBigSize, false);
                        this._imagePlugin.CropAndResizeImage(model.Image, string.Format("{0}{1}/", Contains.NewsFolder, folderName), fileName2, ImageSize.WithMediumSize, ImageSize.HeightMediumSize, false);
                        this._imagePlugin.CropAndResizeImage(model.Image, string.Format("{0}{1}/", Contains.NewsFolder, folderName), fileName3, ImageSize.WithSmallSize, ImageSize.HeightSmallSize, false);

                        model.ImageBigSize = string.Format("{0}{1}/{2}", Contains.NewsFolder, folderName, fileName1);
                        model.ImageMediumSize = string.Format("{0}{1}/{2}", Contains.NewsFolder, folderName, fileName2);
                        model.ImageSmallSize = string.Format("{0}{1}/{2}", Contains.NewsFolder, folderName, fileName3);
                    }

                    if (model.MenuId > 0)
                    {
                        MenuLink menuLink = this._menuLinkService.GetById(model.MenuId, isCache: false);
                        model.VirtualCatUrl = menuLink.VirtualSeoUrl;
                        model.VirtualCategoryId = menuLink.VirtualId;
                        model.MenuLink = Mapper.Map<MenuLink, MenuLinkViewModel>(menuLink);
                    }

                    News modelMap = Mapper.Map(model, byId);
                    this._newsService.Update(modelMap);

                    //Update Localized   
                    foreach (var localized in model.Locales)
                    {
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Title, localized.Title, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.ShortDesc, localized.ShortDesc, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Description, localized.Description, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.SeoUrl, localized.Title.NonAccent(), localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaTitle, localized.MetaTitle, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaKeywords, localized.MetaKeywords, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaDescription, localized.MetaDescription, localized.LanguageId);
                    }

                    base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.UpdateSuccess, FormUI.News)));
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
            catch (Exception ex)
            {
                base.ModelState.AddModelError("", ex.Message);
                ExtentionUtils.Log(string.Concat("News.Edit: ", ex.Message));
                return base.View(model);
            }
            return action;
        }

        [RequiredPermisson(Roles = "ViewNews")]
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
            IEnumerable<News> news = this._newsService.PagedList(sortingPagingBuilder, paging);
            if (news != null && news.Any<News>())
            {
                Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => this.Url.Action("Index", new { page = i, keywords = keywords }));
                ((dynamic)base.ViewBag).PageInfo = pageInfo;
            }
            return base.View(news);
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.RouteData.Values["action"].Equals("create") || filterContext.RouteData.Values["action"].Equals("edit") || filterContext.RouteData.Values["action"].ToString().ToLower().Equals("index"))
            {
                IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) => x.Status == 1 && x.TemplateType == 1 || x.TemplateType == 6 || x.TemplateType == 7, true);
                ((dynamic)base.ViewBag).MenuList = menuLinks;
            }
        }
    }
}