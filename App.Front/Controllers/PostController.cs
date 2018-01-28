using App.Core.Caching;
using App.Core.Utils;
using App.Domain.Entities.Attribute;
using App.Domain.Entities.Data;
using App.Domain.Entities.GenericControl;
using App.Domain.Entities.Menu;
using App.Framework.Ultis;
using App.Front.Models;
using App.Service.Common;
using App.Service.Gallery;
using App.Service.GenericControl;
using App.Service.Language;
using App.Service.Menu;
using App.Service.Post;
using App.Aplication;
using App.Aplication.MVCHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using App.FakeEntity.GenericControl;
using App.Aplication.Extensions;

namespace App.Front.Controllers
{
    public class PostController : FrontBaseController
    {
        private readonly ICacheManager _cacheManager;

        private readonly IPostService _postService;

        private readonly IMenuLinkService _menuLinkService;

        private readonly IGalleryService _galleryService;

        private readonly IWorkContext _workContext;

        private readonly IGenericControlService _genericControlService;
        public PostController(
            IPostService postService
            , IMenuLinkService menuLinkService
            , IGalleryService galleryService
             , IWorkContext workContext
            , IGenericControlService genericControlService
            , ICacheManager cacheManager)
        {
            this._postService = postService;
            this._menuLinkService = menuLinkService;
            this._galleryService = galleryService;
            this._workContext = workContext;
            this._genericControlService = genericControlService;
            _cacheManager = cacheManager;
        }

        [PartialCache("Medium")]
        public ActionResult FillterProduct(string attribute = null)
        {
            return base.PartialView();
        }

        [PartialCache("Medium")]
        public ActionResult GetAccesssoriesHome(int page, string id)
        {
            Expression<Func<Post, bool>> expression = PredicateBuilder.True<Post>();
            expression = expression.And<Post>((Post x) => x.Status == 1);
            expression = expression.And<Post>((Post x) => x.VirtualCategoryId.Contains(id));
            expression = expression.And<Post>((Post x) => x.ProductHot);
            SortBuilder sortBuilder = new SortBuilder()
            {
                ColumnName = "UpdatedDate",
                ColumnOrder = SortBuilder.SortOrder.Descending
            };
            Paging paging = new Paging()
            {
                PageNumber = page,
                PageSize = 4,
                TotalRecord = 0
            };
            IEnumerable<Post> posts = this._postService.FindAndSort(expression, sortBuilder, paging);
            if (!posts.IsAny<Post>())
            {
                return base.Json(new { success = true, data = "" }, JsonRequestBehavior.AllowGet);
            }
            return base.Json(new { data = this.RenderRazorViewToString("_SlideProductHome", posts), success = true }, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        [PartialCache("Short")]
        public ActionResult GetCareerHomePage(string virtualId)
        {
            List<Post> posts = new List<Post>();
            IEnumerable<Post> top = this._postService.GetTop<int>(4, (Post x) => x.Status == 1 && x.VirtualCategoryId.Contains(virtualId), (Post x) => x.ViewCount);
            if (top.IsAny<Post>())
            {
                posts.AddRange(top);
            }
            return this.PartialView("GetPostRelative", posts);
        }

        public ActionResult GetGallery(int postId, int typeId)
        {
            IEnumerable<GalleryImage> galleryImages = _galleryService.GetByOption(attributeValueId: typeId, postId: postId);

            //IEnumerable<GalleryImage> galleryImages = _galleryService.FindBy((GalleryImage x) => x.AttributeValueId == typeId && x.PostId == postId, false);
            if (!galleryImages.IsAny())
            {
                return base.Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            return base.Json(new { data = this.RenderRazorViewToString("_PartialGallery", galleryImages), success = true }, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        [PartialCache("Short")]
        public ActionResult GetNewProductRelative(double? price, int productId)
        {
            double? nullable;
            double? nullable1;
            double? nullable2;
            double? nullable3 = price;
            double num = (double)2000000;
            if (nullable3.HasValue)
            {
                nullable1 = new double?(nullable3.GetValueOrDefault() - num);
            }
            else
            {
                nullable = null;
                nullable1 = nullable;
            }
            double? nullable4 = nullable1;
            nullable3 = price;
            num = (double)2000000;
            if (nullable3.HasValue)
            {
                nullable2 = new double?(nullable3.GetValueOrDefault() + num);
            }
            else
            {
                nullable = null;
                nullable2 = nullable;
            }
            double? nullable5 = nullable2;
            List<Post> posts = new List<Post>();
            IEnumerable<Post> top = this._postService.GetTop<DateTime?>(4, (Post x) => x.Status == 1 && x.Price >= nullable4 && x.Price <= nullable5 && x.Id != productId, (Post x) => x.UpdatedDate);

            if (top == null)
                return HttpNotFound();

            if (top.IsAny<Post>())
            {
                posts.AddRange(top);
            }
            return base.PartialView(posts);
        }

        [ChildActionOnly]
        [PartialCache("Short")]
        public ActionResult GetNewProductRelative2(string virtualId, int productId)
        {
            int languageId = _workContext.WorkingLanguage.Id;

            List<Post> lstPost = new List<Post>();
            IEnumerable<Post> iePost = this._postService.GetTop<DateTime?>(4, (Post x) => x.Status == 1 && x.VirtualCategoryId.Contains(virtualId) && x.Id != productId, (Post x) => x.UpdatedDate);

            if (iePost == null)
                return HttpNotFound();

            IEnumerable<Post> postLocalized = null;
            if (iePost.IsAny<Post>())
            {
                postLocalized = iePost.Select(x =>
                {
                    return x.ToModel();
                });               

                lstPost.AddRange(postLocalized);
            }
            return base.PartialView(lstPost);
        }

        [ChildActionOnly]
        public ActionResult GetPostAccessory(string virtualId)
        {
            List<Post> posts = new List<Post>();
            IEnumerable<Post> top = _postService.GetTop(4, (Post x) => x.Status == 1 && x.VirtualCategoryId.Contains(virtualId)
            , (Post x) => x.UpdatedDate);

            if (top.IsAny())
            {
                posts.AddRange(top);
            }
            return this.PartialView("_SlideProductHome", posts);
        }

        [PartialCache("Short")]
        public ActionResult GetPostByCategory(string virtualCategoryId, int page, string title, string attrs, string prices, string proattrs, string keywords
            , int? productOld, int? productNew)
        {
            Expression<Func<Post, bool>> expression = PredicateBuilder.True<Post>();
            expression = expression.And((Post x) => x.Status == 1);
            SortBuilder sortBuilder = new SortBuilder()
            {
                ColumnName = "CreatedDate",
                ColumnOrder = SortBuilder.SortOrder.Descending
            };
            Paging paging = new Paging()
            {
                PageNumber = page,
                PageSize = _pageSize,
                TotalRecord = 0
            };
            if (page == 1)
            {
                dynamic viewBag = ViewBag;
                IPostService postService = _postService;
                Expression<Func<Post, bool>> productNews = (Post x) => x.ProductNew && x.Status == 1;
                viewBag.HotCard = postService.GetTop(3, productNews, (Post x) => x.CreatedDate).ToList();
            }
            if (!string.IsNullOrEmpty(attrs))
            {
                string[] strArrays = attrs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                List<int> list = (
                    from s in strArrays
                    select int.Parse(s)).ToList();
                expression = expression.And((Post x) => x.AttributeValues.Count((AttributeValue a) => list.Contains(a.Id)) > 0);
                ((dynamic)base.ViewBag).Attributes = list;
            }
            if (!string.IsNullOrEmpty(prices))
            {
                List<double> nums = (
                    from s in prices.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    select double.Parse(s)).ToList<double>();
                double item = nums[0];
                double num = nums[1];
                expression = expression.And<Post>((Post x) => x.Price >= (double?)item && x.Price <= (double?)num);
                ((dynamic)base.ViewBag).Prices = nums;
            }
            if (!string.IsNullOrEmpty(proattrs))
            {
                string[] strArrays1 = proattrs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                List<int> list1 = (
                    from s in strArrays1
                    select int.Parse(s)).ToList<int>();
                expression = expression.And<Post>((Post x) => list1.Contains(x.Id));
                ((dynamic)base.ViewBag).ProAttributes = list1;
            }
            if (!string.IsNullOrEmpty(keywords))
            {
                expression = expression.And<Post>((Post x) => x.Title.Contains(keywords));
            }
            expression = expression.And<Post>((Post x) => x.VirtualCategoryId.Contains(virtualCategoryId));
            if (productNew.HasValue)
            {
                expression = expression.And<Post>((Post x) => !x.OldOrNew);
                ((dynamic)base.ViewBag).ProductNew = productNew;
            }
            if (productOld.HasValue & productNew.HasValue)
            {
                expression = expression.Or<Post>((Post x) => x.OldOrNew);
                ((dynamic)base.ViewBag).ProductOld = productOld;
            }
            if (productOld.HasValue & !productNew.HasValue)
            {
                expression = expression.And<Post>((Post x) => x.OldOrNew);
                ((dynamic)base.ViewBag).ProductOld = productOld;
            }

            IEnumerable<Post> posts = this._postService.GetBySort(expression, sortBuilder, paging);

            if (posts == null)
                return HttpNotFound();

            //Get menu category filter
            IEnumerable<MenuLink> menuCategoryFilter = _menuLinkService.GetByOption(virtualId: virtualCategoryId);

            if (menuCategoryFilter.IsAny())
            {
                List<BreadCrumb> lstBreadCrumb = new List<BreadCrumb>();
                ((dynamic)base.ViewBag).MenuCategoryFilter = menuCategoryFilter;

                //Lấy bannerId từ post để hiển thị banner trên post
                ((dynamic)base.ViewBag).BannerId = menuCategoryFilter.Where(x => x.VirtualId == virtualCategoryId).FirstOrDefault().Id;

                string[] strArrays2 = virtualCategoryId.Split(new char[] { '/' });
                for (int i1 = 0; i1 < (int)strArrays2.Length; i1++)
                {
                    string str = strArrays2[i1];
                    MenuLink menuLink = this._menuLinkService.GetByMenuName(str, title);
                    
                    if (menuLink != null)
                    {
                        //Lấy bannerId từ post để hiển thị banner trên post
                        if (i1 == 0)
                            ((dynamic)base.ViewBag).BannerId = menuLink.Id;

                        lstBreadCrumb.Add(new BreadCrumb()
                        {
                            Title = menuLink.GetLocalized(m => m.MenuName, menuLink.Id),
                            Current = false,
                            Url = base.Url.Action("GetContent", "Menu", new { area = "", menu = menuLink.SeoUrl })
                        });
                    }
                }
                lstBreadCrumb.Add(new BreadCrumb()
                {
                    Current = true,
                    Title = title
                });
                ((dynamic)base.ViewBag).BreadCrumb = lstBreadCrumb;
            }

            IEnumerable<Post> postLocalized = null;

            if (posts.IsAny())
            {
                postLocalized = posts.Select(x =>
                {
                    return x.ToModel();
                });

                Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => base.Url.Action("GetContent", "Menu", new { page = i }));
                ((dynamic)base.ViewBag).PageInfo = pageInfo;
                ((dynamic)base.ViewBag).CountItem = pageInfo.TotalItems;
                ((dynamic)base.ViewBag).MenuId = postLocalized.ElementAt(0).MenuId;

            }

            ((dynamic)base.ViewBag).Title = title;
            ((dynamic)base.ViewBag).VirtualId = virtualCategoryId;

            return base.PartialView(postLocalized);
        }

        [PartialCache("Medium")]
        public ActionResult GetPostForYou()
        {
            List<Post> posts = new List<Post>();
            IEnumerable<Post> top = this._postService.GetTop<DateTime>(20, (Post x) => x.Status == 1 && x.PostType == 1, (Post x) => x.CreatedDate);
            if (top.IsAny<Post>())
            {
                posts.AddRange(top);
            }
            return base.Json(new { data = this.RenderRazorViewToString("_PartialPostItems", posts), success = true }, JsonRequestBehavior.AllowGet);
        }

        [PartialCache("Medium")]
        public ActionResult GetPostLatest()
        {
            List<Post> posts = new List<Post>();
            IEnumerable<Post> top = this._postService.GetTop<DateTime>(20, (Post x) => x.Status == 1, (Post x) => x.CreatedDate);
            if (top.IsAny<Post>())
            {
                posts.AddRange(top);
            }
            return base.Json(new { data = this.RenderRazorViewToString("_PartialPostItems", posts), success = true }, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        [PartialCache("Short")]
        public ActionResult GetPostRelative(string virtualId)
        {
            List<Post> posts = new List<Post>();
            IEnumerable<Post> top = this._postService.GetTop<DateTime?>(4, (Post x) => x.Status == 1 && x.VirtualCategoryId.Contains(virtualId), (Post x) => x.UpdatedDate);
            if (top.IsAny<Post>())
            {
                posts.AddRange(top);
            }
            return base.PartialView(posts);
        }

        [ChildActionOnly]
        [PartialCache("Short")]
        public ActionResult GetPostSameMenu(int menuId, int postId)
        {
            List<Post> posts = new List<Post>();
            IEnumerable<Post> top = this._postService.GetTop<DateTime>(6, (Post x) => x.Status == 1 && x.MenuId == menuId && x.Id != postId, (Post x) => x.CreatedDate);
            if (top.IsAny<Post>())
            {
                posts.AddRange(top);
            }
            return base.PartialView(posts);
        }

        [PartialCache("Medium")]
        public ActionResult GetPriceProduct(int productId, int attributeId)
        {
            GalleryImage galleryImage = this._galleryService.Get((GalleryImage x) => x.PostId == productId && x.AttributeValueId == attributeId, false);
            if (galleryImage == null || !galleryImage.Price.HasValue)
            {
                return base.Json("Liên hệ");
            }

            return base.Json(galleryImage.Price);
            //return base.Json(string.Format("{0:##,###0 VND}", galleryImage.Price));
        }

        [PartialCache("Medium")]
        public ActionResult GetProductHot()
        {
            IEnumerable<Post> top = this._postService.GetTop(9999, (Post x) => x.Status == 1 && (x.ProductHot || x.ProductNew));
            return base.PartialView(top);
        }

        [PartialCache("Medium")]
        public ActionResult GetProductTimeLine()
        {
            IEnumerable<Post> top = this._postService.GetTop(9999, (Post x) => x.Status == 1 && x.OldOrNew);
            return base.PartialView(top);
        }

        [PartialCache("Medium")]
        public ActionResult GetProductNewHome(int page, string id)
        {
            Expression<Func<Post, bool>> expression = PredicateBuilder.True<Post>();
            expression = expression.And<Post>((Post x) => x.Status == 1);
            expression = expression.And<Post>((Post x) => x.VirtualCategoryId.Contains(id));
            expression = expression.And<Post>((Post x) => x.ProductHot);
            SortBuilder sortBuilder = new SortBuilder()
            {
                ColumnName = "UpdatedDate",
                ColumnOrder = SortBuilder.SortOrder.Descending
            };
            Paging paging = new Paging()
            {
                PageNumber = page,
                PageSize = 4,
                TotalRecord = 0
            };
            IEnumerable<Post> posts = this._postService.FindAndSort(expression, sortBuilder, paging);
            if (!posts.IsAny<Post>())
            {
                return base.Json(new { success = true, data = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            return base.Json(new
            {
                data = this.RenderRazorViewToString("_SlideProductHome",
                from x in posts
                orderby x.OrderDisplay descending
                select x),
                success = true
            }, JsonRequestBehavior.AllowGet);
        }

        //Get product trang chủ
        [PartialCache("Medium")]
        public ActionResult GetProductHome()
        {
            IEnumerable<Post> iePost = null;

            //Get danh sách menu có DisplayOnHomePage ==true
            IEnumerable<MenuLink> menuLinks = this._menuLinkService.GetByOption(isDisplayHomePage: true, template: new List<int> { 2 });

            //Convert to localized
            menuLinks = menuLinks.Select(x =>
            {
                return x.ToModel();
            });

            if (menuLinks.IsAny())
            {
                ((dynamic)base.ViewBag).MenuLinkHome = menuLinks;


                List<Post> lstPost = new List<Post>();

                foreach (var item in menuLinks)
                {
                    iePost = this._postService.GetByOption(virtualCategoryId: item.CurrentVirtualId, isDisplayHomePage: true);

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

        //Get product SearchHome
        [PartialCache("Medium")]
        public ActionResult GetProductSearchHome(bool productHot, bool productNew, bool productOld)
        {
            IEnumerable<Post> iePost = this._postService.GetTop(9999, (Post x) => x.Status == 1 && x.ProductHot);

            return PartialView(iePost);

        }

        [ChildActionOnly]
        public ActionResult GetProductNewTabHome(string virtualId)
        {
            List<Post> posts = new List<Post>();
            IEnumerable<Post> top = this._postService.GetTop<DateTime?>(4, (Post x) => x.Status == 1 && x.VirtualCategoryId.Contains(virtualId) && x.ProductHot, (Post x) => x.UpdatedDate);
            if (top.IsAny<Post>())
            {
                posts.AddRange(top);
            }
            return this.PartialView("_SlideProductHome",
                from x in posts
                orderby x.OrderDisplay descending
                select x);
        }

        [PartialCache("Medium")]
        public ActionResult PostDetail(string seoUrl)
        {
            List<BreadCrumb> breadCrumbs = new List<BreadCrumb>();
            Post post = _postService.GetBySeoUrl(seoUrl);

            if (post == null)
                return HttpNotFound();

            Post postLocalized = null;

            if (post != null)
            {
                postLocalized = post.ToModel();

                Post viewCount = post;
                viewCount.ViewCount = viewCount.ViewCount + 1;
                this._postService.Update(post);

                string[] strArrays = post.VirtualCategoryId.Split(new char[] { '/' });
                for (int i = 0; i < (int)strArrays.Length; i++)
                {
                    string str = strArrays[i];
                    MenuLink menuLink = _menuLinkService.GetByCurrentVirtualId(str);
                    //MenuLink menuLink = this._menuLinkService.Get((MenuLink x) => x.CurrentVirtualId.Equals(str), false);
                    breadCrumbs.Add(new BreadCrumb()
                    {
                        Title = menuLink.GetLocalized(x => x.MenuName, menuLink.Id), //menuLink.GetLocalizedByLocaleKey(menuLink.MenuName, menuLink.Id, languageId, "MenuLink", "MenuName"),
                        Current = false,
                        Url = base.Url.Action("GetContent", "Menu", new { area = "", menu = menuLink.SeoUrl })
                    });
                }
                breadCrumbs.Add(new BreadCrumb()
                {
                    Current = true,
                    Title = postLocalized.Title
                });
                ((dynamic)base.ViewBag).BreadCrumb = breadCrumbs;
                ((dynamic)base.ViewBag).Title = postLocalized.Title;
                ((dynamic)base.ViewBag).KeyWords = postLocalized.MetaKeywords;
                ((dynamic)base.ViewBag).SiteUrl = base.Url.Action("PostDetail", "Post", new { seoUrl = seoUrl, area = "" });
                ((dynamic)base.ViewBag).Description = postLocalized.MetaTitle;
                ((dynamic)base.ViewBag).Image = base.Url.Content(string.Concat("~/", postLocalized.ImageMediumSize));
                ((dynamic)base.ViewBag).MenuId = postLocalized.MenuId;
            }
            return base.View(postLocalized);
        }

        [PartialCache("Medium")]
        public ActionResult SearchResult(string catUrl, string parameters, int page)
        {
            HttpCookie httpCookie = base.HttpContext.Request.Cookies.Get("system_search");
            if (!base.Request.Cookies.ExistsCokiee("system_search"))
            {
                return base.View();
            }
            Expression<Func<Post, bool>> expression = PredicateBuilder.True<Post>();
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

            SeachConditions seachCondition = JsonConvert.DeserializeObject<SeachConditions>(base.Server.UrlDecode(httpCookie.Value));

            //expression = expression.And<Post>((Post x) => (int?)x.MenuId == seachCondition.CategoryId);
            //MenuLink byId = this._menuLinkService.GetById(seachCondition.CategoryId.Value);
            //((dynamic)base.ViewBag).KeyWords = byId.MetaKeywords;
            //((dynamic)base.ViewBag).SiteUrl = base.Url.Action("GetContent", "Menu", new { catUrl = catUrl, parameters = parameters, page = page, area = "" });
            //((dynamic)base.ViewBag).Description = byId.MetaDescription;
            //((dynamic)base.ViewBag).Image = base.Url.Content(string.Concat("~/", byId.ImageUrl));
            //((dynamic)base.ViewBag).VirtualId = byId.CurrentVirtualId;
            //string menuName = byId.MenuName;

            if (!string.IsNullOrEmpty(seachCondition.Keywords))
            {
                expression = expression.And<Post>((Post x) => x.SeoUrl.Contains(seachCondition.Keywords) || x.Title.Contains(seachCondition.Keywords) || x.Description.Contains(seachCondition.Keywords));
            }

            IEnumerable<Post> posts = this._postService.FindAndSort(expression, sortBuilder, paging);
            ((dynamic)base.ViewBag).PageNumber = page;
            //((dynamic)base.ViewBag).Title = menuName;
            if (posts.IsAny<Post>())
            {
                Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => base.Url.Action("GetContent", "Menu", new { page = i }));
                ((dynamic)base.ViewBag).PageInfo = pageInfo;
                ((dynamic)base.ViewBag).CountItem = pageInfo.TotalItems;
            }
            return base.View("GetPostByCategory", posts);
        }

        [PartialCache("Medium")]
        public async Task<JsonResult> GetGenericControlByEntityId()
        {
            IEnumerable<App.Domain.Entities.GenericControl.GenericControl> genericControls = _genericControlService.GetByMenuId(3);

            IOrderedEnumerable<GenericControl> genericControls1 = await Task.FromResult(
                from x in genericControls
                orderby x.OrderDisplay descending
                select x);

            JsonResult jsonResult = Json(new { success = true, list = this.RenderRazorViewToString("_PostDetail.Attribute", genericControls1) }, JsonRequestBehavior.AllowGet);
            return jsonResult;
        }

        //Hien thi san pham footer
        [PartialCache("Medium")]
        public JsonResult GetProductOutOfStock()
        {
            IEnumerable<Post> post = this._postService.GetTop(6, (Post x) => x.Status == 1 && x.OutOfStock);

            JsonResult jsonResult = Json(new { success = true, list = this.RenderRazorViewToString("_Footer.Product", post) }, JsonRequestBehavior.AllowGet);

            return jsonResult;
        }

        #region Attribute

        [HttpPost]
        public JsonResult GetByMenuId(int menuId, int entityId)
        {
            List<ControlValueItemResponse> lstValueResponse = new List<ControlValueItemResponse>();

            MenuLink menuLink = _menuLinkService.GetById(menuId);
            if (menuLink != null)
            {
                IEnumerable<GenericControl> ieGC = menuLink.GenericControls;
                if (ieGC.IsAny())
                {
                    foreach (GenericControl item in ieGC)
                    {
                        IEnumerable<GenericControlValue> gCVDefault = item.GenericControlValues.Where(m => m.Status == 1);

                        foreach (var gcValue in gCVDefault)
                        {
                            ControlValueItemResponse objValueResponse = new ControlValueItemResponse();

                            objValueResponse.GenericControlValueId = gcValue.Id;
                            objValueResponse.Name = gcValue.ValueName;
                            objValueResponse.ValueName = gcValue.GetValueItem(entityId);

                            lstValueResponse.Add(objValueResponse);
                        }
                    }
                }
            }

            JsonResult jsonResult = Json(
                 new
                 {
                     success = lstValueResponse.Count() > 0,
                     list = this.RenderRazorViewToString("_PostDetail.Attribute", lstValueResponse)
                 },
                 JsonRequestBehavior.AllowGet);

            return jsonResult;
        }

        #endregion
    }
}