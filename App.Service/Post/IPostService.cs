using App.Core.Utils;
using App.Domain.Entities.Data;
using App.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace App.Service.Post
{
    public interface IPostService : IBaseService<App.Domain.Entities.Data.Post>, IService
    {
        App.Domain.Entities.Data.Post GetById(int Id, bool isCache = true);

        IEnumerable<App.Domain.Entities.Data.Post> GetListSeoUrl(string seoUrl, bool isCache = true);

        Domain.Entities.Data.Post GetBySeoUrl(string seoUrl, bool @readonly = false);

        IEnumerable<App.Domain.Entities.Data.Post> PagedList(SortingPagingBuilder sortBuider, Paging page);

        IEnumerable<Domain.Entities.Data.Post> PagedListByMenu(SortingPagingBuilder sortBuider, Paging page);

        IEnumerable<Domain.Entities.Data.Post> GetBySort(Expression<Func<Domain.Entities.Data.Post, bool>> expression, SortBuilder sortBuilder, Paging paging);

        IEnumerable<Domain.Entities.Data.Post> GetByOption(string virtualCategoryId = null, bool? isDisplayHomePage = null, int status = 1, bool isCache = true);

    }
}