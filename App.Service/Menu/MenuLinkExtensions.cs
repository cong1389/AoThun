using App.Domain.Entities.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace App.Service.Menu
{
    public static class MenuLinkExtensions
    {
        public static bool SelectedMenu(this MenuLink menuLink, int id)
        {
            bool result = false;
            try
            {
                if (id == 0)
                    result = false;

                App.Service.GenericControl.IGenericControlService _genericControlService = DependencyResolver.Current.GetService<App.Service.GenericControl.IGenericControlService>();

                App.Domain.Entities.GenericControl.GenericControl genericControl = _genericControlService.GetById(id);
                if (genericControl == null)
                    return false;

                foreach (MenuLink mnu in genericControl.MenuLinks)
                {
                    if (menuLink.Id == mnu.Id)
                    {
                        result = true;
                    }
                }

            }
            catch 
            {
                return false;
               
            }

            return result;
        }
    }
}
