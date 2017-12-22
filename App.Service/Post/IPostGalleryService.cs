using App.Domain.Entities.Data;
using App.Domain.Interfaces.Services;
using System.Collections.Generic;

namespace App.Service.Post
{
	public interface IPostGalleryService : IBaseService<PostGallery>, IService
	{
        PostGallery GetById(int Id, bool isCache = true);

        IEnumerable<PostGallery> GetByPostId(int postId, bool isCache = true);

    }
}