using App.Core.Caching;
using App.Core.Common;
using App.Core.Extensions;
using App.Service.Common;
using App.Service.GenericAttribute;
using App.Service.LocalizedProperty;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

namespace App.Service.Language
{
    public static class LocalizationExtentions
    {
        private const string CACHE_LOCALIZA_KEY = "db.Localization.{0}";
       
        public static string GetLocalized<T>(this T entity, Expression<Func<T, string>> keySelector, int entityId)
        {
            var workContext = DependencyResolver.Current.GetService<IWorkContext>();
            return GetLocalized(entity, keySelector, entityId, workContext.WorkingLanguage.Id);
        }

        /// <summary>
        /// Get localized property of an entity
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="returnDefaultValue">A value indicating whether to return default value (if localized is not found)</param>
        /// <param name="ensureTwoPublishedLanguages">A value indicating whether to ensure that we have at least two published languages; otherwise, load only default value</param>
        /// <returns>Localized property</returns>
        public static string GetLocalized<T>(this T entity,
            Expression<Func<T, string>> keySelector, int entityId, int languageId,
            bool returnDefaultValue = true, bool ensureTwoPublishedLanguages = true)

        {
            return GetLocalized<T, string>(entity, keySelector, entityId, languageId, returnDefaultValue, ensureTwoPublishedLanguages);
        }

        public static string GetLocalized<T, TPropType>(this T entity,
           Expression<Func<T, TPropType>> keySelector,
           int entityId,
           int languageId,
           bool returnDefaultValue = true,
           bool ensureTwoPublishedLanguages = true)

        {

            var member = keySelector.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    keySelector));
            }

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException(string.Format(
                       "Expression '{0}' refers to a field, not a property.",
                       keySelector));
            }
            string result = null;

            // load localized value
            string localeKeyGroup = typeof(T).Name.Replace("ViewModel", "");
            string localeKey = propInfo.Name;

            if (languageId > 0)
            {
                var _cacheManager = DependencyResolver.Current.GetService<ICacheManager>();
                var _localizedPropertyService = DependencyResolver.Current.GetService<ILocalizedPropertyService>();

                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_LOCALIZA_KEY, "GetLocalized");
                Guid guid = entity.GetType().GUID;
                sbKey.AppendFormat("-{0}", guid);
                sbKey.AppendFormat("-{0}", entityId);
                sbKey.AppendFormat("-{0}", languageId);
                sbKey.AppendFormat("-{0}", localeKeyGroup);
                sbKey.AppendFormat("-{0}", localeKey);
                
                App.Domain.Entities.Language.LocalizedProperty localizedProperty = _localizedPropertyService.GetByKey(languageId
                    , entityId, localeKeyGroup, localeKey);

                result = localizedProperty != null ? localizedProperty.LocaleValue : null;
            }

            return result;
        }

        /// <summary>
        /// GetLocalizedByLocaleKey
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">Class name</param>
        /// <param name="fallBackValue">Value mặc định nếu không có localized</param>
        /// <param name="entityId">Id của thực thể cần set localized</param>
        /// <param name="languageId">Id ngôn ngữ</param>
        /// <param name="localeKeyGroup">Tên nhóm</param>
        /// <param name="localeKey">Tên key</param>
        /// <returns></returns>
        public static string GetLocalizedByLocaleKey<T>(this T entity, string fallBackValue, int entityId, int languageId, string localeKeyGroup, string localeKey)
        {
            string result = null;
            if (languageId > 0)
            {
                var _cacheManager = DependencyResolver.Current.GetService<ICacheManager>();
                var _localizedPropertyService = DependencyResolver.Current.GetService<ILocalizedPropertyService>();

                StringBuilder sbKey = new StringBuilder();
                sbKey.AppendFormat(CACHE_LOCALIZA_KEY, "GetLocalizedByLocaleKey");
                Guid guid = entity.GetType().GUID;
                sbKey.AppendFormat("-{0}", guid);
                sbKey.AppendFormat("-{0}", entityId);
                sbKey.AppendFormat("-{0}", languageId);
                sbKey.AppendFormat("-{0}", localeKeyGroup);
                sbKey.AppendFormat("-{0}", localeKey);

                string key = sbKey.ToString();
                Domain.Entities.Language.LocalizedProperty localizedProperty = _cacheManager.Get<Domain.Entities.Language.LocalizedProperty>(key);
                if (localizedProperty == null)
                {
                    localizedProperty = _localizedPropertyService.GetByKey(languageId
                    , entityId, localeKeyGroup, localeKey);
                    _cacheManager.Put(key, localizedProperty);
                }                
              
                //App.Domain.Entities.Language.LocalizedProperty localizedProperty = _localizedPropertyService.GetLocalizedPropertByKey(languageId
                //    , entityId, localeKeyGroup, localeKey);

                result = localizedProperty != null ? localizedProperty.LocaleValue : fallBackValue;
            }

            return result;
        }

    }
}
