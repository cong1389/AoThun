using App.Admin.Helpers;
using App.Aplication;
using App.Core.Caching;
using App.Core.Utils;
using App.Domain.Entities.Attribute;
using App.Domain.Entities.Data;
using App.Domain.Entities.Language;
using App.Domain.Entities.Menu;
using App.FakeEntity.Gallery;
using App.FakeEntity.Post;
using App.Framework.Ultis;
using App.Service.Attribute;
using App.Service.Gallery;
using App.Service.GenericControl;
using App.Service.Language;
using App.Service.LocalizedProperty;
using App.Service.Menu;
using App.Service.Post;
using AutoMapper;
using Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace App.Admin.Controllers
{
    public class PostController : BaseAdminUploadController
    {
        private const string CACHE_POST_KEY = "db.Post";
        private readonly ICacheManager _cacheManager;

        private readonly IAttributeValueService _attributeValueService;

        private readonly IMenuLinkService _menuLinkService;

        private readonly IPostService _postService;

        private readonly IAttributeService _attributeService;

        private readonly IGalleryService _galleryService;

        private readonly IImagePlugin _imagePlugin;

        private readonly ILanguageService _languageService;

        private readonly ILocalizedPropertyService _localizedPropertyService;

        private readonly IPostGalleryService _postGalleryService;

        private readonly IGenericControlService _genericControlService;

        public PostController(
            IPostService postService
            , IMenuLinkService menuLinkService
            , IAttributeValueService attributeValueService
            , IGalleryService galleryService
            , IImagePlugin imagePlugin
            , IAttributeService attributeService
            , ILanguageService languageService
            , ILocalizedPropertyService localizedPropertyService
            , IPostGalleryService postGalleryService
            , IGenericControlService genericControlService
            , ICacheManager cacheManager)
            : base(cacheManager)
        {
            this._postService = postService;
            this._menuLinkService = menuLinkService;
            this._attributeValueService = attributeValueService;
            this._galleryService = galleryService;
            this._imagePlugin = imagePlugin;
            this._attributeService = attributeService;
            this._languageService = languageService;
            this._localizedPropertyService = localizedPropertyService;
            this._postGalleryService = postGalleryService;
            _genericControlService = genericControlService;
            _cacheManager = cacheManager;

            //Clear cache
            _cacheManager.Clear();
        }

        [RequiredPermisson(Roles = "CreatePost")]
        public ActionResult Create()
        {
            var model = new PostViewModel
            {
                OrderDisplay = 0,
                Status = 1
            };

            //Add locales to model
            AddLocales(_languageService, model.Locales);

            return base.View(model);
        }

        [HttpPost]
        [RequiredPermisson(Roles = "CreatePost")]
        [ValidateInput(false)]
        public ActionResult Create(PostViewModel model, string ReturnUrl)
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
                else //if (!this._postService.FindBy((Post x) => x.ProductCode.Equals(model.ProductCode), true).IsAny<Post>())
                {
                    string titleNonAccent = model.Title.NonAccent();
                    IEnumerable<Post> bySeoUrl = this._postService.GetListSeoUrl(titleNonAccent);
                    model.SeoUrl = model.Title.NonAccent();
                    if (bySeoUrl.Any<Post>((Post x) => x.Id != model.Id))
                    {
                        PostViewModel postViewModel = model;
                        postViewModel.SeoUrl = string.Concat(postViewModel.SeoUrl, "-", bySeoUrl.Count<Post>());
                    }

                    string folderName = string.Format("{0:ddMMyyyy}", DateTime.UtcNow);
                    if (model.Image != null && model.Image.ContentLength > 0)
                    {
                        string fileExtension = Path.GetExtension(model.Image.FileName);
                        //string fileName = titleNonAccent.FileNameFormat(extension);

                        string fileName1 = titleNonAccent.FileNameFormat(fileExtension);
                        string fileName2 = titleNonAccent.FileNameFormat(fileExtension);
                        string fileName3 = titleNonAccent.FileNameFormat(fileExtension);

                        _imagePlugin.CropAndResizeImage(model.Image, string.Format("{0}{1}/", Contains.PostFolder, folderName), fileName1, ImageSize.WithBigSize, ImageSize.HeightBigSize, false);
                        _imagePlugin.CropAndResizeImage(model.Image, string.Format("{0}{1}/", Contains.PostFolder, folderName), fileName2, ImageSize.WithMediumSize, ImageSize.HeightMediumSize, false);
                        _imagePlugin.CropAndResizeImage(model.Image, string.Format("{0}{1}/", Contains.PostFolder, folderName), fileName3, ImageSize.WithSmallSize, ImageSize.HeightSmallSize, false);

                        model.ImageBigSize = string.Format("{0}{1}/{2}", Contains.PostFolder, folderName, fileName1);
                        model.ImageMediumSize = string.Format("{0}{1}/{2}", Contains.PostFolder, folderName, fileName2);
                        model.ImageSmallSize = string.Format("{0}{1}/{2}", Contains.PostFolder, folderName, fileName3);
                    }

                    int? menuId = model.MenuId;
                    int i = 0;
                    if ((menuId.GetValueOrDefault() > i ? menuId.HasValue : false))
                    {
                        IMenuLinkService menuLinkService = this._menuLinkService;
                        menuId = model.MenuId;
                        MenuLink byId = menuLinkService.GetById(menuId.Value);
                        model.VirtualCatUrl = byId.VirtualSeoUrl;
                        model.VirtualCategoryId = byId.VirtualId;
                    }

                    //Gallery image
                    HttpFileCollectionBase files = base.Request.Files;
                    List<GalleryImage> galleryImages = new List<GalleryImage>();
                    if (files.Count > 0)
                    {
                        int count = files.Count - 1;
                        int num = 0;
                        string str6 = titleNonAccent;
                        string[] allKeys = files.AllKeys;
                        for (i = 0; i < (int)allKeys.Length; i++)
                        {
                            string str7 = allKeys[i];
                            if (num <= count)
                            {
                                if (!str7.Equals("Image"))
                                {
                                    string str8 = str7.Replace("[]", "");
                                    HttpPostedFileBase item = files[num];
                                    if (item.ContentLength > 0)
                                    {
                                        string item1 = base.Request[str8];
                                        GalleryImageViewModel galleryImageViewModel = new GalleryImageViewModel()
                                        {
                                            PostId = model.Id,
                                            AttributeValueId = int.Parse(str8)
                                        };

                                        string fileExtension = Path.GetExtension(model.Image.FileName);
                                        string fileName1 = titleNonAccent.FileNameFormat(fileExtension);
                                        string fileName2 = titleNonAccent.FileNameFormat(fileExtension);

                                        _imagePlugin.CropAndResizeImage(item, string.Format("{0}{1}/", Contains.PostFolder, folderName), fileName1, ImageSize.WithBigSize, ImageSize.HeightBigSize, false);
                                        _imagePlugin.CropAndResizeImage(item, string.Format("{0}{1}/", Contains.PostFolder, folderName), fileName2, ImageSize.WithThumbnailSize, ImageSize.HeightThumbnailSize, false);

                                        galleryImageViewModel.ImageThumbnail = string.Format("{0}{1}/{2}", Contains.PostFolder, folderName, fileName2);
                                        galleryImageViewModel.ImagePath = string.Format("{0}{1}/{2}", Contains.PostFolder, folderName, fileName1);

                                        galleryImageViewModel.OrderDisplay = num;
                                        galleryImageViewModel.Status = 1;
                                        galleryImageViewModel.Title = model.Title;
                                        galleryImageViewModel.Price = new double?(double.Parse(item1));

                                        galleryImages.Add(Mapper.Map<GalleryImage>(galleryImageViewModel));
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

                    //Attribute
                    List<AttributeValue> attributeValues = new List<AttributeValue>();
                    string item2 = base.Request["Values"];
                    if (!string.IsNullOrEmpty(item2))
                    {
                        foreach (string list in item2.Split(new char[] { ',' }).ToList<string>())
                        {
                            int num1 = int.Parse(list);
                            attributeValues.Add(this._attributeValueService.GetById(num1));
                        }
                    }

                    Post modelMap = Mapper.Map<PostViewModel, Post>(model);
                    if (galleryImages.IsAny<GalleryImage>())
                    {
                        modelMap.GalleryImages = galleryImages;
                    }
                    if (attributeValues.IsAny<AttributeValue>())
                    {
                        modelMap.AttributeValues = attributeValues;
                    }

                    this._postService.Create(modelMap);

                    //Update Localized   
                    foreach (var localized in model.Locales)
                    {
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Title, localized.Title, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.ProductCode, localized.ProductCode, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.ShortDesc, localized.ShortDesc, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Description, localized.Description, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.TechInfo, localized.TechInfo, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.SeoUrl, localized.SeoUrl, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaTitle, localized.MetaTitle, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaKeywords, localized.MetaKeywords, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaDescription, localized.MetaDescription, localized.LanguageId);
                    }

                    base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.CreateSuccess, FormUI.Post)));
                    if (!base.Url.IsLocalUrl(ReturnUrl) || ReturnUrl.Length <= 1 || !ReturnUrl.StartsWith("/") || ReturnUrl.StartsWith("//") || ReturnUrl.StartsWith("/\\"))
                    {
                        action = base.RedirectToAction("Index");
                    }
                    else
                    {
                        action = this.Redirect(ReturnUrl);
                    }
                }
                //else
                //{
                //	base.ModelState.AddModelError("", "Mã sản phẩm đã tồn tại.");
                //	action = base.View(model);
                //}
            }
            catch (Exception ex)
            {
                ExtentionUtils.Log(string.Concat("Post.Create: ", ex.Message));
                base.ModelState.AddModelError("", ex.Message);

                return base.View(model);
            }
            return action;
        }

        [RequiredPermisson(Roles = "DeletePost")]
        public ActionResult Delete(string[] ids)
        {
            try
            {
                if (ids.Length != 0)
                {
                    List<Post> posts = new List<Post>();
                    List<GalleryImage> galleryImages = new List<GalleryImage>();
                    string[] strArrays = ids;
                    for (int i = 0; i < (int)strArrays.Length; i++)
                    {
                        int num = int.Parse(strArrays[i]);
                        Post post = this._postService.Get((Post x) => x.Id == num, false);
                        galleryImages.AddRange(post.GalleryImages.ToList<GalleryImage>());
                        post.AttributeValues.ToList<AttributeValue>().ForEach((AttributeValue att) => post.AttributeValues.Remove(att));
                        posts.Add(post);
                    }
                    this._galleryService.BatchDelete(galleryImages);
                    this._postService.BatchDelete(posts);

                    //Delete localize
                    for (int i = 0; i < ids.Length; i++)
                    {
                        IEnumerable<LocalizedProperty> ieLocalizedProperty
                           = _localizedPropertyService.GetByEntityId(int.Parse(ids[i]));
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

        [RequiredPermisson(Roles = "CreatePost")]
        public ActionResult DeleteGallery(int postId, int galleryId)
        {
            ActionResult actionResult;
            if (!base.Request.IsAjaxRequest())
            {
                return base.Json(new { success = false });
            }
            try
            {
                GalleryImage galleryImage = this._galleryService.Get((GalleryImage x) => x.PostId == postId && x.Id == galleryId, false);
                this._galleryService.Delete(galleryImage);

                string path1 = base.Server.MapPath(string.Concat("~/", galleryImage.ImagePath));
                string path2 = base.Server.MapPath(string.Concat("~/", galleryImage.ImageThumbnail));

                System.IO.File.Delete(path1);
                System.IO.File.Delete(path2);

                actionResult = base.Json(new { success = true });
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                actionResult = base.Json(new { success = false, messages = exception.Message });
            }
            return actionResult;
        }

        [RequiredPermisson(Roles = "CreatePost")]
        public ActionResult Edit(int Id)
        {
            Post byId = this._postService.GetById(Id);

            PostViewModel modelMap = Mapper.Map<Post, PostViewModel>(byId);

            ((dynamic)base.ViewBag).Galleries = byId.GalleryImages;

            //Add Locales to model
            AddLocales(_languageService, modelMap.Locales, (locale, languageId) =>
            {
                locale.Id = modelMap.Id;
                locale.LocalesId = modelMap.Id;
                locale.Title = modelMap.GetLocalized(x => x.Title, Id, languageId, false, false);
                locale.ProductCode = modelMap.GetLocalized(x => x.ProductCode, Id, languageId, false, false);
                locale.ShortDesc = modelMap.GetLocalized(x => x.ShortDesc, Id, languageId, false, false);
                locale.Description = modelMap.GetLocalized(x => x.Description, Id, languageId, false, false);
                locale.TechInfo = modelMap.GetLocalized(x => x.TechInfo, Id, languageId, false, false);
                locale.MetaTitle = modelMap.GetLocalized(x => x.MetaTitle, Id, languageId, false, false);
                locale.MetaKeywords = modelMap.GetLocalized(x => x.MetaKeywords, Id, languageId, false, false);
                locale.MetaDescription = modelMap.GetLocalized(x => x.MetaDescription, Id, languageId, false, false);
                locale.SeoUrl = modelMap.GetLocalized(x => x.SeoUrl, Id, languageId, false, false);
            });

            return base.View(modelMap);
        }

        [HttpPost]
        [RequiredPermisson(Roles = "CreatePost")]
        [ValidateInput(false)]
        public ActionResult Edit(PostViewModel model, string ReturnUrl)
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
                else if (!_postService.FindBy((Post x) => x.ProductCode.Equals(model.ProductCode) && x.Id != model.Id, true).IsAny<Post>())
                {
                    Post byId = this._postService.GetById(model.Id, isCache: false);

                    string titleNonAccent = model.Title.NonAccent();
                    IEnumerable<MenuLink> bySeoUrl = this._menuLinkService.GetListSeoUrl(titleNonAccent, isCache: false);

                    model.SeoUrl = model.Title.NonAccent();
                    if (bySeoUrl.Any<MenuLink>((MenuLink x) => x.Id != model.Id))
                    {
                        PostViewModel postViewModel = model;
                        postViewModel.SeoUrl = string.Concat(postViewModel.SeoUrl, "-", bySeoUrl.Count<MenuLink>());
                    }
                    //string str1 = titleNonAccent;
                    //if (str1.Length > 250)
                    //{
                    //    str1 = Utils.Utils.SplitWords(250, str1);
                    //}

                    string folderName = string.Format("{0:ddMMyyyy}", DateTime.UtcNow);
                    if (model.Image != null && model.Image.ContentLength > 0)
                    {
                        string fileExtension = Path.GetExtension(model.Image.FileName);

                        string fileName1 = titleNonAccent.FileNameFormat(fileExtension);
                        string fileName2 = titleNonAccent.FileNameFormat(fileExtension);
                        string fileName3 = titleNonAccent.FileNameFormat(fileExtension);

                        this._imagePlugin.CropAndResizeImage(model.Image, string.Format("{0}{1}/", Contains.PostFolder, folderName), fileName1, ImageSize.WithBigSize, ImageSize.HeightBigSize, false);
                        this._imagePlugin.CropAndResizeImage(model.Image, string.Format("{0}{1}/", Contains.PostFolder, folderName), fileName2, ImageSize.WithMediumSize, ImageSize.HeightMediumSize, false);
                        this._imagePlugin.CropAndResizeImage(model.Image, string.Format("{0}{1}/", Contains.PostFolder, folderName), fileName3, ImageSize.WithSmallSize, ImageSize.HeightSmallSize, false);

                        model.ImageBigSize = string.Format("{0}{1}/{2}", Contains.PostFolder, folderName, fileName1);
                        model.ImageMediumSize = string.Format("{0}{1}/{2}", Contains.PostFolder, folderName, fileName2);
                        model.ImageSmallSize = string.Format("{0}{1}/{2}", Contains.PostFolder, folderName, fileName3);
                    }
                    int? menuId = model.MenuId;
                    int i = 0;
                    if ((menuId.GetValueOrDefault() > i ? menuId.HasValue : false))
                    {
                        IMenuLinkService menuLinkService = this._menuLinkService;
                        menuId = model.MenuId;
                        MenuLink menuLink = menuLinkService.GetById(menuId.Value, isCache: false);
                        model.VirtualCatUrl = menuLink.VirtualSeoUrl;
                        model.VirtualCategoryId = menuLink.VirtualId;
                    }

                    //GalleryImage
                    HttpFileCollectionBase files = base.Request.Files;
                    List<GalleryImage> lstGalleryImages = new List<GalleryImage>();
                    if (files.Count > 0)
                    {
                        int count = files.Count - 1;
                        int num = 0;

                        string[] allKeys = files.AllKeys;
                        for (i = 0; i < (int)allKeys.Length; i++)
                        {
                            string str7 = allKeys[i];
                            if (num <= count)
                            {
                                if (!str7.Equals("Image"))
                                {
                                    string str8 = str7.Replace("[]", "");
                                    HttpPostedFileBase item = files[num];
                                    if (item.ContentLength > 0)
                                    {
                                        string item1 = base.Request[str8];
                                        GalleryImageViewModel galleryImageViewModel = new GalleryImageViewModel()
                                        {
                                            PostId = model.Id,
                                            AttributeValueId = int.Parse(str8)
                                        };
                                        string fileName1 = string.Format("{0}-{1}.jpg", titleNonAccent, Guid.NewGuid());
                                        string fileName2 = string.Format("{0}-{1}.jpg", titleNonAccent, Guid.NewGuid());

                                        this._imagePlugin.CropAndResizeImage(item, string.Format("{0}{1}/", Contains.PostFolder, folderName), fileName1, ImageSize.WithBigSize, ImageSize.WithBigSize, false);
                                        this._imagePlugin.CropAndResizeImage(item, string.Format("{0}{1}/", Contains.PostFolder, folderName), fileName2, ImageSize.WithThumbnailSize, ImageSize.HeightThumbnailSize, false);

                                        galleryImageViewModel.ImageThumbnail = string.Format("{0}{1}/{2}", Contains.PostFolder, folderName, fileName2);
                                        galleryImageViewModel.ImagePath = string.Format("{0}{1}/{2}", Contains.PostFolder, folderName, fileName1);

                                        galleryImageViewModel.OrderDisplay = num;
                                        galleryImageViewModel.Status = 1;
                                        galleryImageViewModel.Title = model.Title;
                                        galleryImageViewModel.Price = new double?(double.Parse(item1));

                                        lstGalleryImages.Add(Mapper.Map<GalleryImage>(galleryImageViewModel));
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
                    if (lstGalleryImages.IsAny<GalleryImage>())
                    {
                        byId.GalleryImages = lstGalleryImages;
                    }

                    //AttributeValue
                    List<AttributeValue> lstAttributeValues = new List<AttributeValue>();
                    List<int> nums = new List<int>();
                    string item2 = base.Request["Values"];
                    if (!string.IsNullOrEmpty(item2))
                    {
                        foreach (string list in item2.Split(new char[] { ',' }).ToList<string>())
                        {
                            int num1 = int.Parse(list);
                            nums.Add(num1);
                            lstAttributeValues.Add(this._attributeValueService.GetById(num1, isCache: false));
                        }

                        if (nums.IsAny<int>())
                        {
                            (
                                from x in byId.AttributeValues
                                where !nums.Contains(x.Id)
                                select x).ToList<AttributeValue>().ForEach((AttributeValue att) => byId.AttributeValues.Remove(att));
                        }
                    }

                    byId.AttributeValues = lstAttributeValues;

                    Post modelMap = Mapper.Map(model, byId);
                    this._postService.Update(byId);

                    //Update GalleryImage
                    if (lstAttributeValues.IsAny<AttributeValue>())
                    {
                        foreach (AttributeValue attributeValue in lstAttributeValues)
                        {
                            GalleryImage nullable = this._galleryService.Get((GalleryImage x) => x.AttributeValueId == attributeValue.Id && x.PostId == model.Id, false);
                            if (nullable == null)
                            {
                                continue;
                            }
                            HttpRequestBase request = base.Request;
                            i = attributeValue.Id;
                            double num2 = double.Parse(request[i.ToString()]);
                            nullable.Price = new double?(num2);
                            this._galleryService.Update(nullable);
                        }
                    }

                    //Update Localized
                    foreach (var localized in model.Locales)
                    {
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Title, localized.Title, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.ProductCode, localized.ProductCode, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.ShortDesc, localized.ShortDesc, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.Description, localized.Description, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.TechInfo, localized.TechInfo, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.SeoUrl, localized.SeoUrl, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaTitle, localized.MetaTitle, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaKeywords, localized.MetaKeywords, localized.LanguageId);
                        _localizedPropertyService.SaveLocalizedValue(modelMap, x => x.MetaDescription, localized.MetaDescription, localized.LanguageId);
                    }

                    base.Response.Cookies.Add(new HttpCookie("system_message", string.Format(MessageUI.UpdateSuccess, FormUI.Post)));
                    if (!base.Url.IsLocalUrl(ReturnUrl) || ReturnUrl.Length <= 1 || !ReturnUrl.StartsWith("/") || ReturnUrl.StartsWith("//") || ReturnUrl.StartsWith("/\\"))
                    {
                        action = base.RedirectToAction("Index");
                    }
                    else
                    {
                        action = this.Redirect(ReturnUrl);
                    }
                }
                else
                {
                    base.ModelState.AddModelError("", "Mã sản phẩm đã tồn tại.");
                    action = base.View(model);
                }
            }
            catch (Exception ex)
            {
                base.ModelState.AddModelError("", ex.Message);
                ExtentionUtils.Log(string.Concat("Post.Edit: ", ex.Message));

                return base.View(model);
            }

            return action;
        }

        [RequiredPermisson(Roles = "ViewPost")]
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
            IEnumerable<Post> posts = this._postService.PagedList(sortingPagingBuilder, paging);
            if (posts != null && posts.Any<Post>())
            {
                Helper.PageInfo pageInfo = new Helper.PageInfo(ExtentionUtils.PageSize, page, paging.TotalRecord, (int i) => this.Url.Action("Index", new { page = i, keywords = keywords }));
                ((dynamic)base.ViewBag).PageInfo = pageInfo;
            }
            return base.View(posts);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.RouteData.Values["action"].Equals("edit") || filterContext.RouteData.Values["action"].Equals("create"))
            {
                //IEnumerable<MenuLink> menuLinks = this._menuLinkService.FindBy((MenuLink x) => x.Status == 1 && (x.TemplateType == 2) && x.ParentId.HasValue, true);
                //((dynamic)base.ViewBag).MenuList = menuLinks;

                IEnumerable<Domain.Entities.Attribute.Attribute> attributes = this._attributeService.FindBy((Domain.Entities.Attribute.Attribute x) => x.Status == 1, false);
                if (attributes.IsAny())
                {
                    ((dynamic)base.ViewBag).Attributes = attributes;
                }

                //IEnumerable<GenericControl> genericControl = this._genericControlService.FindBy((GenericControl x) => x.MenuId == 3, false);
                //if (attributes.IsAny())
                //{
                //    ((dynamic)base.ViewBag).GenericControls = genericControl;
                //}
            }
        }

        #region Post image gallery

        [HttpPost]
        public ActionResult PostGalleryAdd(int postId)
        {
            List<PostGallery> lstPostGallery = new List<PostGallery>();

            HttpFileCollectionBase files = Request.Files;
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
                                PostGallery postGallery = new PostGallery()
                                {
                                    PostId = postId
                                };

                                string fileNameNonAccent = string.Format("{0}", item.FileName.NonAccent());
                                string folderName = string.Format("{0:ddMMyyyy}", DateTime.UtcNow);

                                string fileExtension = Path.GetExtension(item.FileName);

                                string fileName1 = fileNameNonAccent.FileNameFormat(fileExtension);
                                string fileName2 = fileNameNonAccent.FileNameFormat(fileExtension);
                                string fileName3 = fileNameNonAccent.FileNameFormat(fileExtension);

                                //string str3 = string.Format("{0}.jpg", fileNameNonAccent);
                                //string str4 = string.Format("{0}-{1}.jpg", fileNameNonAccent, Guid.NewGuid());
                                //string str5 = string.Format("{0}-{1}.jpg", fileNameNonAccent, Guid.NewGuid());

                                this._imagePlugin.CropAndResizeImage(item, string.Format("{0}{1}/", Contains.PostFolder, folderName), fileName1, ImageSize.WithBigSize, ImageSize.HeightBigSize, false);
                                this._imagePlugin.CropAndResizeImage(item, string.Format("{0}{1}/", Contains.PostFolder, folderName), fileName2, ImageSize.WithMediumSize, ImageSize.HeightMediumSize, false);
                                this._imagePlugin.CropAndResizeImage(item, string.Format("{0}{1}/", Contains.PostFolder, folderName), fileName3, ImageSize.WithSmallSize, ImageSize.HeightSmallSize, false);

                                postGallery.ImageBigSize = string.Format("{0}{1}/{2}", Contains.PostFolder, folderName, fileName1);
                                postGallery.ImageMediumSize = string.Format("{0}{1}/{2}", Contains.PostFolder, folderName, fileName2);
                                postGallery.ImageSmallSize = string.Format("{0}{1}/{2}", Contains.PostFolder, folderName, fileName3);

                                _postGalleryService.Create(postGallery);
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

            return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PostGalleryEdit(PostGalleryViewModel model)
        {
            PostGallery postGallery = _postGalleryService.GetById(model.Id);

            if (postGallery != null)
            {
                model.ImageSmallSize = postGallery.ImageSmallSize;
                model.ImageBigSize = postGallery.ImageBigSize;
                model.ImageMediumSize = postGallery.ImageMediumSize;

                PostGallery postGalleryMap = Mapper.Map<PostGalleryViewModel, PostGallery>(model, postGallery);
                _postGalleryService.Update(postGalleryMap);
            }

            return base.Json(
                new
                {
                    succes = true
                }
                , JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> PostGalleryList(int postId)
        {
            IEnumerable<PostGallery> postGalleryList = _postGalleryService.GetByPostId(postId);

            IOrderedEnumerable<PostGallery> posts = await Task.FromResult(
                from x in postGalleryList
                orderby x.Id descending
                select x);

            JsonResult jsonResult = this.Json(new { data = posts }, JsonRequestBehavior.AllowGet);



            return jsonResult;
        }

        public ActionResult DeletePostGallery(int postId, int id)
        {
            ActionResult actionResult;

            if (!base.Request.IsAjaxRequest())
            {
                return base.Json(new { success = false });
            }
            try
            {
                PostGallery galleryImage = this._postGalleryService.Get((PostGallery x) => x.PostId == postId && x.Id == id, false);

                _postGalleryService.Delete(galleryImage);

                string path1 = base.Server.MapPath(string.Concat("~/", galleryImage.ImageBigSize));
                string path2 = base.Server.MapPath(string.Concat("~/", galleryImage.ImageMediumSize));
                string path3 = base.Server.MapPath(string.Concat("~/", galleryImage.ImageSmallSize));

                System.IO.File.Delete(path1);
                System.IO.File.Delete(path2);
                System.IO.File.Delete(path3);

                actionResult = base.Json(new { success = true });
            }
            catch (Exception ex)
            {
                actionResult = base.Json(new { success = false, messages = ex.Message });
            }
            return actionResult;
        }

        #endregion
    }
}