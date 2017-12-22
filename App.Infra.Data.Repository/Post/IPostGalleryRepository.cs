using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Repository;
using System.Collections.Generic;

namespace App.Infra.Data.Repository.Post
{
	public interface IPostGalleryRepository : IRepositoryBase<PostGallery>
	{
        PostGallery GetById(int Id);

        IEnumerable<PostGallery> PagedList(Paging page);

        //IEnumerable<PostGallery> PagedSearchList(SortingPagingBuilder sortBuider, Paging page);

        //IEnumerable<PostGallery> PagedSearchListByMenu(SortingPagingBuilder sortBuider, Paging page);
    }
}