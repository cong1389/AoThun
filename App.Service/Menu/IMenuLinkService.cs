using App.Core.Utils;
using App.Domain.Entities.Menu;
using App.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace App.Service.Menu
{
    public interface IMenuLinkService : IBaseService<MenuLink>, IService
    {
        MenuLink GetById(int Id, bool isCache = true);

        IEnumerable<MenuLink> GetListSeoUrl(string seoUrl, bool isCache = true);

        IEnumerable<MenuLink> GetByTemplateType(int templateType, bool @readonly = false, bool isCache = true);

        MenuLink GetBySeoUrl(string seoUrl, bool @readonly = false, bool isCache = true);

        MenuLink GetByParentId(int parentId, string currentVirtualId, bool isCache = true);

        MenuLink GetByCurrentVirtualId(string currentVirtualId, bool isCache = true);

        MenuLink GetByMenuName(string virtualCategoryId, string menuName, bool isCache = true);

        IEnumerable<MenuLink> PagedList(SortingPagingBuilder sortBuider, Paging page);

        IEnumerable<MenuLink> GetByOption(List<int> position = null
                , List<int> template = null
                , string virtualId = null
                , string seoUrl = null
             , List<int> parentId = null
                , bool? isDisplayHomePage = null
                , bool? isDisplaySearch = null
                , int status = 1
            , bool isCache = true);
    }
}