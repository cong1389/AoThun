using App.Core.Caching;
using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Gallery;
using App.Infra.Data.UOW.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace App.Service.Gallery
{
    public class GalleryService : BaseService<GalleryImage>, IGalleryService, IBaseService<GalleryImage>, IService
    {
        private const string CACHE_GALLERY_KEY = "db.Gallery.{0}";
        private readonly ICacheManager _cacheManager;

        private readonly IGalleryRepository _galleryRepository;

        private readonly IUnitOfWork _unitOfWork;

        public GalleryService(IUnitOfWork unitOfWork, IGalleryRepository galleryRepository
            , ICacheManager cacheManager) : base(unitOfWork, galleryRepository)
        {
            this._unitOfWork = unitOfWork;
            this._galleryRepository = galleryRepository;
            _cacheManager = cacheManager;

        }

        public GalleryImage GetById(int id, bool isCache = true)
        {
            GalleryImage galleryImage;

            if (isCache)
            {
                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_GALLERY_KEY, "GetById");
                sbKey.Append(id);

                string key = sbKey.ToString();
                galleryImage = _cacheManager.Get<GalleryImage>(key);
                if (galleryImage == null)
                {
                    galleryImage = _galleryRepository.GetGalleryById(id);
                    _cacheManager.Put(key, galleryImage);
                }
            }
            else
            {
                galleryImage = _galleryRepository.GetGalleryById(id);
            }

            return galleryImage;
        }

        public IEnumerable<GalleryImage> GetByOption(int? attributeValueId = null
            , int? postId = null
            , int status = 1
            , bool isCache = true)
        {
            IEnumerable<GalleryImage> galleryImages;
            StringBuilder sbKey = new StringBuilder();
            sbKey.AppendFormat(CACHE_GALLERY_KEY, "GetByOption");

            Expression<Func<GalleryImage, bool>> expression = PredicateBuilder.True<GalleryImage>();
            sbKey.AppendFormat("-{0}", status);
            expression = expression.And((GalleryImage x) => x.Status == status);

            if (attributeValueId != null)
            {
                sbKey.AppendFormat("-{0}", attributeValueId);
                expression = expression.And((GalleryImage x) => x.AttributeValueId == attributeValueId);
            }

            if (postId != null)
            {
                sbKey.AppendFormat("-{0}", postId);
                expression = expression.And((GalleryImage x) => x.PostId == postId);
            }

            if (isCache)
            {
                string key = sbKey.ToString();
                galleryImages = _cacheManager.GetCollection<GalleryImage>(key);
                if (galleryImages == null)
                {
                    galleryImages = FindBy(expression, false);
                    _cacheManager.Put(key, galleryImages);
                }
            }
            else
            {
                galleryImages = FindBy(expression, false);
            }


            return galleryImages;
        }
    }
}