using App.Core.Common;
using App.Core.Utils;
using App.Domain.Entities.Language;
using App.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace App.Service.LocalizedProperty
{
    public interface ILocalizedPropertyService : IBaseService<App.Domain.Entities.Language.LocalizedProperty>, IService
    {
        void CreateLocalizedProperty(App.Domain.Entities.Language.LocalizedProperty LocalizedProperty);

        App.Domain.Entities.Language.LocalizedProperty GetById(int id, bool isCache = true);

        IEnumerable<App.Domain.Entities.Language.LocalizedProperty> GetByEntityId(int entityId, bool isCache = true);

        App.Domain.Entities.Language.LocalizedProperty GetByKey(int languageId,int entityId, string localeKeyGroup, string localeKey, bool isCache = true);

        IEnumerable<App.Domain.Entities.Language.LocalizedProperty> PagedList(SortingPagingBuilder sortBuider, Paging page);

        int SaveLocalizedProperty();

        void SaveLocalizedValue<T>(
            T entity,
            Expression<Func<T, string>> keySelector,
            string localeValue,
            int languageId) where T : AuditableEntity<int>;

        void SaveLocalizedValueItem<T, TPropType>(
           T entity,
           Expression<Func<T, TPropType>> keySelector,
           string localeValue,
           int languageId) where T : AuditableEntity<int>;
        
    }
}