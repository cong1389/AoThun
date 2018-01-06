using App.Framework.Theme;
using App.Service.Customers;
using App.Aplication;
using Autofac.Engine;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace App.Front
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Initialize engine context
            //var engine = EngineContext.Initialize(false);

            Bootstrapper.Run();

            ConfigureViewEngines();

            ModelBinders.Binders.Add(typeof(DateTime), new DateTimeModelBinder());
            ModelBinders.Binders.Add(typeof(DateTime?), new DateTimeModelBinder());

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }

        private static void ConfigureAntiForgeryTokens()
        {
            AntiForgeryConfig.CookieName = "f";
        }

        private static void ConfigureViewEngines()
        { // Remove all view engines
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            
            ViewEngines.Engines.Add(new ThemeableRazorViewEngine());
        }

        //public void AnonymousIdentification_Creating(object sender, AnonymousIdentificationEventArgs args)
        //{
        //    try
        //    {
        //        //var customerService = DependencyResolver.Current.GetService<ICustomerService>();
        //        //var customer = customerService.GetByGuid(args.AnonymousID.ToString());
        //        args.AnonymousID = Guid.NewGuid().ToString();
        //    }
        //    catch { }
        //}
    }
}
