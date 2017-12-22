using App.Core.Caching;
using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Repository;
using App.Infra.Data.Common;
using App.Infra.Data.DbFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace App.Infra.Data.Repository.Post
{
	public class PostGalleryRepository : RepositoryBase<PostGallery>, IPostGalleryRepository, IRepositoryBase<PostGallery>
	{

        public PostGalleryRepository(IDbFactory dbFactory) : base(dbFactory)
		{
        }

        public PostGallery GetById(int id)
        {
            PostGallery postGallery = this.FindBy((App.Domain.Entities.Data.PostGallery x) => x.Id == id, false).FirstOrDefault();
            
            return postGallery;
        }

        protected override IOrderedQueryable<PostGallery> GetDefaultOrder(IQueryable<App.Domain.Entities.Data.PostGallery> query)
        {
            IOrderedQueryable<App.Domain.Entities.Data.PostGallery> PostGallery =
                from p in query
                orderby p.Id
                select p;
            return PostGallery;
        }

        public IEnumerable<PostGallery> PagedList(Paging page)
        {
            return this.GetAllPagedList(page).ToList<App.Domain.Entities.Data.PostGallery>();
        }

        //public IEnumerable<App.Domain.Entities.Data.PostGallery> PagedSearchList(SortingPagingBuilder sortBuider, Paging page)
        //{
        //    Expression<Func<App.Domain.Entities.Data.PostGallery, bool>> expression = PredicateBuilder.True<App.Domain.Entities.Data.PostGallery>();
        //    if (!string.IsNullOrEmpty(sortBuider.Keywords))
        //    {
        //        expression = expression.And<App.Domain.Entities.Data.PostGallery>((App.Domain.Entities.Data.PostGallery x) => x.PostId.Contains(sortBuider.Keywords.ToLower()) 
        //        );
        //    }
        //    return this.FindAndSort(expression, sortBuider.Sorts, page);
        //}

        //public IEnumerable<App.Domain.Entities.Data.PostGallery> PagedSearchListByMenu(SortingPagingBuilder sortBuider, Paging page)
        //{
        //    Expression<Func<App.Domain.Entities.Data.PostGallery, bool>> expression = PredicateBuilder.True<App.Domain.Entities.Data.PostGallery>();
        //    if (!string.IsNullOrEmpty(sortBuider.Keywords))
        //    {
        //        expression = expression.And<App.Domain.Entities.Data.PostGallery>((App.Domain.Entities.Data.PostGallery x) => x.VirtualCategoryId.Contains(sortBuider.Keywords) && x.Status == 1);
        //    }
        //    return this.FindAndSort(expression, sortBuider.Sorts, page);
        //}
    }
}