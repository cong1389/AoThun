using App.Domain.Entities.GenericControl;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace App.Service.GenericControl
{
    public static class GenericControlValueExtension
    {
        public static string GetValueItem(this GenericControlValue genericControlValue, int entityId)
        {
            var itemService = DependencyResolver.Current.GetService<IGenericControlValueItemService>();

            IEnumerable<GenericControlValueItem> valueItem = itemService.GetByOption(genericControlValueId: genericControlValue.Id, entity: entityId,isCache:false);

            if (valueItem.Any())
            {
              return  valueItem.FirstOrDefault().Value;
            }

            return null;
        }
    }
}
