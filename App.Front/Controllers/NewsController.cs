using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Entities.Menu;
using App.Framework.Ultis;
using App.Front.Models;
using App.Service.Common;
using App.Service.Language;
using App.Service.Menu;
using App.Service.News;
using App.Service.Static;
using App.Aplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using App.Aplication.Extensions;

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
        public ActionResult GetNewsHome(int? Id)
        {
            IEnumerable<News> iePost = null;

            //Get danh sách menu có DisplayOnHomePage ==true
            IEnumerable<MenuLink> menuLinks = _menuLinkService.GetByOption(isDisplayHomePage: true, template: new List<int> { 1 });

            //Convert to localized
            menuLinks = menuLinks.Select(x =>
            {
                return x.ToModel();
            });

            if (menuLinks.IsAny())
            {
                ((dynamic)base.ViewBag).MenuLinkHome = menuLinks;

                List<News> lstPost = new List<News>();

                foreach (var item in menuLinks)
                {
                    iePost = _newsService.GetByOption(virtualCategoryId: item.CurrentVirtualId, isDisplayHomePage: true);

                    if (iePost.IsAny())
                    {
                        iePost = iePost.Select(x =>
                        {
                            return x.ToModel();
                        });

                        lstPost.AddRange(iePost);
                    }
                }

                iePost = from x in lstPost orderby x.OrderDisplay descending select x;
            }

            return PartialView(iePost);
        }

        public ActionResult GetNewsByCategory(string virtualCategoryId, int? menuId, string title, int page, int? month, int? year)
        {
            dynamic viewBag = base.ViewBag;

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

            IEnumerable<News> news = this._newsService.FindAndSort((News x) => !x.Video && x.Status == 1 && x.VirtualCategoryId.Contains(virtualCategoryId)
            , sortBuilder, paging);

            if (news == null)
                return HttpNotFound();

            Expression<Func<StaticContent, bool>> status = (StaticContent x) => x.Status == 1;
            viewBag.fixItems = _staticContentService.GetTop<int>(3, status, (StaticContent x) => x.ViewCount);

            if (month != null)            
                news = news.Where(n => n.CreatedDate.Month == month);
            if (year != null)
                news = news.Where(n => n.CreatedDate.Year == year);

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
                    menuLink = _menuLinkService.GetByMenuName(str, title);
                    if (menuLink != null)
                    {
                        breadCrumbs.Add(new BreadCrumb()
                        {
                            Title = menuLink.GetLocalized(m => m.MenuName, menuLink.Id),
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

            ((dynamic)base.ViewBag).MenuId = menuId;
            ((dynamic)base.ViewBag).VirtualId = virtualCategoryId;
            ((dynamic)base.ViewBag).Title = title;

            return base.PartialView(newsLocalized);
        }

        [ChildActionOnly]
        [PartialCache("Short")]
        public ActionResult GetRelativeNews(string virtualId, int newsId)
        {
            List<News> news = new List<News>();
            IEnumerable<News> top = this._newsService.GetTop<int>(4, (News x) => x.Status == 1 && x.VirtualCategoryId.Contains(virtualId) && x.Id != newsId && !x.Video, (News x) => x.ViewCount);
            if (top.IsAny())
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
            dynamic viewBag = base.ViewBag;

            IStaticContentService staticContentService = this._staticContentService;
            Expression<Func<StaticContent, bool>> status = (StaticContent x) => x.Status == 1;
            viewBag.fixItems = staticContentService.GetTop<int>(3, status, (StaticContent x) => x.ViewCount);

            List<BreadCrumb> breadCrumbs = new List<BreadCrumb>();
            News news = _newsService.Get((News x) => x.SeoUrl.Equals(seoUrl), true);

            if (news == null)
                return HttpNotFound();

            News newsLocalized = new News();
            if (news != null)
            {
                newsLocalized = news.ToModel();

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
                        Title = menuLink.GetLocalized(m => m.MenuName, menuLink.Id),// menuLink.MenuName, menuLink.Id, languageId, "MenuLink", "MenuName"),
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

        /// <summary>
        /// Lấy tất cả bài viết nhóm theo tháng, năm.
        /// </summary>
        /// <returns></returns>       
        public ActionResult NewsTimeLine()
        {
            IEnumerable<News> ieNews = _newsService.GetAll();

            if (ieNews == null)
                return HttpNotFound();

            IEnumerable<News> ieNewsGroup = ieNews.GroupBy(l => l.CreatedDate.Year + 12 + l.CreatedDate.Month)
                .Select(g =>
                {
                    return g.First().ToModel();
                }).ToList();

            ((dynamic)base.ViewBag).NewsGroup = ieNewsGroup;

            return PartialView(ieNewsGroup);
        }
    }
}