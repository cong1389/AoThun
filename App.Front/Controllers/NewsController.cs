using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Entities.Menu;
using App.Domain.Interfaces.Services;
using App.Extensions;
using App.Framework.Ultis;
using App.Front.Models;
using App.Service.Common;
using App.Service.Language;
using App.Service.Menu;
using App.Service.News;
using App.Service.Static;
using App.Aplication;
using App.Aplication.MVCHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Web.Mvc;

namespace App.Front.Controllers
{
    public class NewsController : FrontBaseController
    {
        private readonly INewsService _newsService;

        private readonly IMenuLinkService _menuLinkService;

        private IStaticContentService _staticContentService;

        private readonly IWorkContext _workContext;

        public NewsController(
            INewsService newsService
            , IMenuLinkService menuLinkService
            , IStaticContentService staticContentService
            , IWorkContext workContext)
        {
            this._newsService = newsService;
            this._menuLinkService = menuLinkService;
            this._staticContentService = staticContentService;
            this._workContext = workContext;
        }

        [ChildActionOnly]
        [PartialCache("Short")]
        public ActionResult BreadCrumNews(string virtualId)
        {
            ((dynamic)base.ViewBag).VirtualId = virtualId;
            List<MenuLink> lstMenuLink = new List<MenuLink>();
            IEnumerable<MenuLink> menuLinks1 = this._menuLinkService.FindBy((MenuLink x) => x.TemplateType == 1 && x.Status == 1, true);
            if (menuLinks1.IsAny<MenuLink>())
            {
                lstMenuLink.AddRange(menuLinks1);
                ((dynamic)base.ViewBag).TitleNews = menuLinks1.ElementAt(0).MenuName;
            }
            return base.PartialView(lstMenuLink);
        }

        public ActionResult GetCareerByCategory(string virtualCategoryId, int page, string title)
        {
            SortBuilder sortBuilder = new SortBuilder()
            {
                ColumnName = "CreatedDate",
                ColumnOrder = SortBuilder.SortOrder.Descending
            };
            Paging paging = new Paging()
            {
                PageNumber = page,
                PageSize = base._pageSize,
                TotalRecord = 0
            };
            IEnumerable<News> news = this._newsService.FindAndSort((News x) => !x.Video && x.Status == 1 && x.VirtualCategoryId.Contains(virtualCategoryId), sortBuilder, paging);
            if (news.IsAny<News>())
            {
                Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => base.Url.Action("GetContent", "Menu", new { page = i }));
                ((dynamic)base.ViewBag).PageInfo = pageInfo;
                ((dynamic)base.ViewBag).CountItem = pageInfo.TotalItems;
            }
            ((dynamic)base.ViewBag).Title = title;
            ((dynamic)base.ViewBag).virtualCategoryId = virtualCategoryId;
            return base.PartialView(news);
        }

        [ChildActionOnly]
        [PartialCache("Short")]
        public ActionResult GetHomeNews(int? Id)
        {
            IEnumerable<News> top = _newsService.GetTop<DateTime>(4, (News x) => x.HomeDisplay == true && x.Status == 1 && x.VirtualCategoryId.Contains("34c7a8cc-5b53-4c8c-ab6c-22f2e2f5f57d"), (News x) => x.CreatedDate);

            if (top == null)
                return HttpNotFound();

            IEnumerable<News> ieNews = top.Select(x =>
            {
                return x.ToModel();
            });

            return base.PartialView(ieNews);
        }

        public ActionResult GetNewsByCategory(string virtualCategoryId, int? menuId, string title, int page)
        {
            int languageId = _workContext.WorkingLanguage.Id;

            ((dynamic)base.ViewBag).MenuId = menuId;
            ((dynamic)base.ViewBag).VirtualId = virtualCategoryId;
            dynamic viewBag = base.ViewBag;
            
            Expression<Func<StaticContent, bool>> status = (StaticContent x) => x.Status == 1;
            viewBag.fixItems = _staticContentService.GetTop<int>(3, status, (StaticContent x) => x.ViewCount);

            SortBuilder sortBuilder = new SortBuilder()
            {
                ColumnName = "CreatedDate",
                ColumnOrder = SortBuilder.SortOrder.Descending
            };
            Paging paging = new Paging()
            {
                PageNumber = page,
                PageSize = base._pageSize,
                TotalRecord = 0
            };

            IEnumerable<News> news = this._newsService.FindAndSort((News x) => !x.Video && x.Status == 1 && x.VirtualCategoryId.Contains(virtualCategoryId), sortBuilder, paging);

            if (news == null)
                return HttpNotFound();

            IEnumerable<News> newsLocalized = news
                .Select(x =>
            {
                return x.ToModel();
            });

            if (news.IsAny())
            {
                Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => base.Url.Action("GetContent", "Menu", new { page = i }));
                ((dynamic)base.ViewBag).PageInfo = pageInfo;
                ((dynamic)base.ViewBag).CountItem = pageInfo.TotalItems;

                MenuLink menuLink = null;
                List<BreadCrumb> breadCrumbs = new List<BreadCrumb>();
                string[] strArrays2 = virtualCategoryId.Split(new char[] { '/' });
                for (int i1 = 0; i1 < (int)strArrays2.Length; i1++)
                {
                    string str = strArrays2[i1];
                    menuLink = this._menuLinkService.Get((MenuLink x) => x.CurrentVirtualId.Equals(str) && x.Id != menuId, false);

                    if (menuLink != null)
                    {
                        //Lấy bannerId từ post để hiển thị banner trên post
                        if (i1 == 0)
                            ((dynamic)base.ViewBag).MenuId = menuLink.Id;

                        breadCrumbs.Add(new BreadCrumb()
                        {
                            Title = menuLink.GetLocalizedByLocaleKey(menuLink.MenuName, menuLink.Id, languageId, "MenuLink", "MenuName"),
                            Current = false,
                            Url = base.Url.Action("GetContent", "Menu", new { area = "", menu = menuLink.SeoUrl })
                        });
                    }
                }
                breadCrumbs.Add(new BreadCrumb()
                {
                    Current = true,
                    Title = title
                });
                ((dynamic)base.ViewBag).BreadCrumb = breadCrumbs;
            }

            ((dynamic)base.ViewBag).Title = title;

            return base.PartialView(newsLocalized);
        }

        [ChildActionOnly]
        [PartialCache("Short")]
        public ActionResult GetRelativeNews(string virtualId, int newsId)
        {
            List<News> news = new List<News>();
            IEnumerable<News> top = this._newsService.GetTop<int>(4, (News x) => x.Status == 1 && x.VirtualCategoryId.Contains(virtualId) && x.Id != newsId && !x.Video, (News x) => x.ViewCount);
            if (top.IsAny<News>())
            {
                news.AddRange(top);
            }
            return base.PartialView(news);
        }

        [ChildActionOnly]
        public ActionResult GetVideoSlide(string virtualCategoryId)
        {
            List<News> news = new List<News>();
            IEnumerable<News> news1 = this._newsService.FindBy((News x) => x.Video && x.Status == 1, true);
            if (news1.IsAny<News>())
            {
                news.AddRange(news1);
            }
            return base.PartialView(news);
        }

        [OutputCache(CacheProfile = "Medium")]
        public ActionResult NewsDetail(string seoUrl)
        {
            int languageId = _workContext.WorkingLanguage.Id;

            dynamic viewBag = base.ViewBag;

            IStaticContentService staticContentService = this._staticContentService;
            Expression<Func<StaticContent, bool>> status = (StaticContent x) => x.Status == 1;
            viewBag.fixItems = staticContentService.GetTop<int>(3, status, (StaticContent x) => x.ViewCount);

            List<BreadCrumb> breadCrumbs = new List<BreadCrumb>();
            News news = this._newsService.Get((News x) => x.SeoUrl.Equals(seoUrl), true);
            if (news == null)
                return HttpNotFound();

            News newsLocalized = new News();
            if (news != null)
            {
                newsLocalized.ToModel();

                ((dynamic)base.ViewBag).Title = newsLocalized.MetaTitle;
                ((dynamic)base.ViewBag).KeyWords = newsLocalized.MetaKeywords;
                ((dynamic)base.ViewBag).SiteUrl = base.Url.Action("NewsDetail", "News", new { seoUrl = seoUrl, area = "" });
                ((dynamic)base.ViewBag).Description = newsLocalized.MetaDescription;
                ((dynamic)base.ViewBag).Image = base.Url.Content(string.Concat("~/", newsLocalized.ImageMediumSize));
                ((dynamic)base.ViewBag).MenuId = newsLocalized.MenuId;
                string[] strArrays = newsLocalized.VirtualCategoryId.Split(new char[] { '/' });
                for (int i = 0; i < (int)strArrays.Length; i++)
                {
                    string str = strArrays[i];
                    MenuLink menuLink = this._menuLinkService.Get((MenuLink x) => x.CurrentVirtualId.Equals(str), false);

                    //Lấy bannerId từ post để hiển thị banner trên post
                    if (i == 0)
                        ((dynamic)base.ViewBag).BannerId = menuLink.Id;

                    breadCrumbs.Add(new BreadCrumb()
                    {
                        Title = menuLink.GetLocalizedByLocaleKey(menuLink.MenuName, menuLink.Id, languageId, "MenuLink", "MenuName"),
                        Current = false,
                        Url = base.Url.Action("GetContent", "Menu", new { area = "", menu = menuLink.SeoUrl })
                    });
                }
                breadCrumbs.Add(new BreadCrumb()
                {
                    Current = true,
                    Title = newsLocalized.Title
                });
                ((dynamic)base.ViewBag).BreadCrumb = breadCrumbs;
            }
            ((dynamic)base.ViewBag).SeoUrl = newsLocalized.MenuLink.SeoUrl;

            return base.View(newsLocalized);
        }
    }
}