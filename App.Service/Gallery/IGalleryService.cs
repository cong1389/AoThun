using App.Domain.Entities.Data;
using App.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace App.Service.Gallery
{
    public interface IGalleryService : IBaseService<GalleryImage>, IService
    {
        GalleryImage GetById(int Id, bool isCache = true);

        IEnumerable<GalleryImage> GetByOption(int? attributeValueId = null
           , int? postId = null
           , int status = 1
            , bool isCache = true);

    }
}