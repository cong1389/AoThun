using App.Core.Caching;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Post;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Service.Post
{
    public class PostGalleryService : BaseService<PostGallery>, IPostGalleryService, IBaseService<PostGallery>, IService
    {
        private const string CACHE_POSTGALLERY_KEY = "db.PostGallery.{0}";
        private readonly ICacheManager _cacheManager;

        private readonly IPostGalleryRepository _galleryRepository;

        private readonly IUnitOfWork _unitOfWork;

        public PostGalleryService(IUnitOfWork unitOfWork, IPostGalleryRepository galleryRepository
            , ICacheManager cacheManager) : base(unitOfWork, galleryRepository)
        {
            this._unitOfWork = unitOfWork;
            this._galleryRepository = galleryRepository;
            _cacheManager = cacheManager;
        }

        public PostGallery GetById(int id, bool isCache = true)
        {
            PostGallery postGallery;

            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_POSTGALLERY_KEY, "GetById");
                sbKey.Append(id);

                string key = sbKey.ToString();
                postGallery = _cacheManager.Get<PostGallery>(key);
                if (postGallery == null)
                {
                    postGallery = _galleryRepository.GetById(id);
                    _cacheManager.Put(key, postGallery);
                }
            }
            else
            {
                postGallery = _galleryRepository.GetById(id);
            }

            return postGallery;
        }

        public IEnumerable<PostGallery> GetByPostId(int postId, bool isCache = true)
        {
            IEnumerable<PostGallery> postGallery;

            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_POSTGALLERY_KEY, "GetByPostId");
                sbKey.Append(postId);

                string key = sbKey.ToString();
                postGallery = _cacheManager.GetCollection<PostGallery>(key);
                if (postGallery == null)
                {
                    postGallery = _galleryRepository.FindBy((PostGallery x) => x.PostId == postId);
                    _cacheManager.Put(key, postGallery);
                }
            }
            else
            {
                postGallery = _galleryRepository.FindBy((PostGallery x) => x.PostId == postId);
            }

            //IEnumerable<PostGallery> postGallery = this._galleryRepository.FindBy((PostGallery x) => x.PostId == postId);

            return postGallery;
        }
    }
}