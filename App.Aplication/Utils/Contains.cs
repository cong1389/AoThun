using System.Configuration;
using System.IO;
using System.Web;

namespace App.Aplication
{
    public static class Contains
    {
        public static string AdsFolder
        {
            get
            {
                string item = ConfigurationManager.AppSettings["AdsFolder"] ?? "images/Ads/";
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(string.Concat("~/", item))))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(string.Concat("~/", item)));
                }
                return ConfigurationManager.AppSettings["AdsFolder"] ?? "images/Ads/";
            }
        }

        public static bool EnableSendEmai
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["EnableSendEmail"] ?? "false");
            }
        }

        public static bool EnableSendSMS
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["EnableSendSMS"] ?? "false");
            }
        }

        public static string FolderLanguage
        {
            get
            {
                string item = ConfigurationManager.AppSettings["LanguageFolder"] ?? "images/language/";
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(string.Concat("~/", item))))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(string.Concat("~/", item)));
                }
                return ConfigurationManager.AppSettings["LanguageFolder"] ?? "images/language/";
            }
        }

        public static string ImageNoExsits
        {
            get
            {
                return "/Areas/Admin/images/no_image.jpg";
            }
        }

        public static string ImageFolder
        {
            get
            {
                string item = ConfigurationManager.AppSettings["ImageFolder"] ?? "images/";
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(string.Concat("~/", item))))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(string.Concat("~/", item)));
                }
                return ConfigurationManager.AppSettings["ImageFolder"] ?? "images/";
            }
        }

        public static string NewsFolder
        {
            get
            {
                string item = ConfigurationManager.AppSettings["NewsFolder"] ?? "images/news/";
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(string.Concat("~/", item))))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(string.Concat("~/", item)));
                }
                return ConfigurationManager.AppSettings["NewsFolder"] ?? "images/news/";
            }
        }

        public static string PostFolder
        {
            get
            {
                string item = ConfigurationManager.AppSettings["PostFolder"] ?? "images/post/";
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(string.Concat("~/", item))))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(string.Concat("~/", item)));
                }
                return ConfigurationManager.AppSettings["PostFolder"] ?? "images/post/";
            }
        }

        public static string StaticContentFolder
        {
            get
            {
                string item = ConfigurationManager.AppSettings["StaticContentFolder"] ?? "images/staticcontent/";
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(string.Concat("~/", item))))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(string.Concat("~/", item)));
                }
                return ConfigurationManager.AppSettings["StaticContentFolder"] ?? "images/staticcontent/";
            }
        }

        public static string FlowStepFolder
        {
            get
            {
                string item = ConfigurationManager.AppSettings["FlowStepFolder"] ?? "images/flowstep/";
                if (!Directory.Exists(HttpContext.Current.Server.MapPath(string.Concat("~/", item))))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(string.Concat("~/", item)));
                }
                return ConfigurationManager.AppSettings["FlowStepFolder"] ?? "images/flowstep/";
            }
        }

        public static bool RequiredActiveAccount
        {
            get
            {
                return bool.Parse(ConfigurationManager.AppSettings["RequiredActiveAccount"] ?? "false");
            }
        }

        //SystemCustomerAttributeNames
        public static string SelectedPaymentMethod { get { return "SelectedPaymentMethod"; } }
        public static string SelectedShippingOption { get { return "SelectedShippingOption"; } }

       

    }

    public enum PaymentMethodType : int
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// All payment information is entered on the site
        /// </summary>
        Standard = 10,
        /// <summary>
        /// A customer is redirected to a third-party site in order to complete the payment
        /// </summary>
        Redirection = 15,
        /// <summary>
        /// Button
        /// </summary>
        Button = 20,
        /// <summary>
        /// All payment information is entered on the site and is available via button
        /// </summary>
        StandardAndButton = 25,
        /// <summary>
        /// Payment information is entered in checkout and customer is redirected to complete payment (e.g. 3D Secure) after order has been placed
        /// </summary>
        StandardAndRedirection = 30
    }

}