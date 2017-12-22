using App.Core.Common;
using Domain.Entities.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace App.Service.GenericAttribute
{
    public static class GenericAttributeExtentions
    {
        public static string GetAttribute(this Customer entity, string keyGroup, string key, IGenericAttributeService genericAttributeService, int storeId = 0)
        {
            if (genericAttributeService == null)
                genericAttributeService = DependencyResolver.Current.GetService<IGenericAttributeService>();

            var prop = genericAttributeService.GetByKey(entity.Id, keyGroup, key);
            if (prop!= null)
            {
                return prop.Value;
            }
            return null;
        }
    }
}
