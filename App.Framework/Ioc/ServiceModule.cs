using App.AsyncService.Post;
using App.Domain.Interfaces.Services;
using App.Infra.Data.Common;
using App.SeoSitemap;
using App.Service.Account;
using App.Service.Ads;
using App.Service.Attribute;
using App.Service.ContactInformation;
using App.Service.Gallery;
using App.Service.Language;
using App.Service.Locations;
using App.Service.MailSetting;
using App.Service.Menu;
using App.Service.News;
using App.Service.Other;
using App.Service.Post;
using App.Service.SeoSetting;
using App.Service.Slide;
using App.Service.Static;
using App.Service.Step;
using App.Service.SystemApp;
using App.Service.Brandes;
using Autofac;
using System;
using App.Service.Orders;
using App.Service.LocalizedProperty;
using App.Service.Common;
using App.Service.GenericAttribute;
using App.Service.LocaleStringResource;
using App.Service.GenericControl;
using App.Service.Customers;
using App.Aplication;
using App.Service.Addresses;
using App.Service.PaymentMethodes;
using App.Service.ShippingMethodes;
using App.Service.Repairs;
using App.Service.Repair;

namespace App.Framework.Ioc
{
    public class ServiceModule : Module
    {
        public ServiceModule()
        {
        }

        /// <summary>
        /// Đăng ký AutoFac
        /// </summary>
        /// <param name="builder"></param>
		protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(BaseService<>)).As(new Type[] { typeof(IBaseService<>) });
            builder.RegisterGeneric(typeof(BaseAsyncService<>)).As(new Type[] { typeof(IBaseAsyncService<>) });

            builder.RegisterType<LanguageService>().As<ILanguageService>().InstancePerRequest(new object[0]);
            builder.RegisterType<MailSettingService>().As<IMailSettingService>().InstancePerRequest(new object[0]);
            builder.RegisterType<ContactInfoService>().As<IContactInfoService>().InstancePerRequest(new object[0]);
            builder.RegisterType<SystemSettingService>().As<ISystemSettingService>().InstancePerRequest(new object[0]);
            builder.RegisterType<MenuLinkService>().As<IMenuLinkService>().InstancePerRequest(new object[0]);
            builder.RegisterType<ProvinceService>().As<IProvinceService>().InstancePerRequest(new object[0]);
            builder.RegisterType<DistrictService>().As<IDistrictService>().InstancePerRequest(new object[0]);

            builder.RegisterType<PostService>().As<IPostService>().InstancePerRequest(new object[0]);
            builder.RegisterType<PostGalleryService>().As<IPostGalleryService>().InstancePerRequest(new object[0]);

            builder.RegisterType<NewsService>().As<INewsService>().InstancePerRequest(new object[0]);
            builder.RegisterType<StaticContentService>().As<IStaticContentService>().InstancePerRequest(new object[0]);
            builder.RegisterType<SettingSeoGlobalService>().As<ISettingSeoGlobalService>().InstancePerRequest(new object[0]);

            builder.RegisterType<PageBannerService>().As<IPageBannerService>().InstancePerRequest(new object[0]);
            builder.RegisterType<BannerService>().As<IBannerService>().InstancePerRequest(new object[0]);

            builder.RegisterType<GalleryService>().As<IGalleryService>().InstancePerRequest(new object[0]);

            builder.RegisterType<AttributeService>().As<IAttributeService>().InstancePerRequest(new object[0]);
            builder.RegisterType<AttributeValueService>().As<IAttributeValueService>().InstancePerRequest(new object[0]);

            builder.RegisterType<SlideShowService>().As<ISlideShowService>().InstancePerRequest(new object[0]);
            builder.RegisterType<LandingPageService>().As<ILandingPageService>().InstancePerRequest(new object[0]);
            builder.RegisterType<FlowStepService>().As<IFlowStepService>().InstancePerRequest(new object[0]);
            builder.RegisterType<SitemapProvider>().As<ISitemapProvider>().InstancePerRequest(new object[0]);

            builder.RegisterType<PostAsynService>().As<IPostAsynService>().InstancePerRequest(new object[0]);

            builder.RegisterType<UserService>().As<IUserService>().InstancePerRequest(new object[0]);
            builder.RegisterType<RoleService>().As<IRoleService>().InstancePerRequest(new object[0]);
            builder.RegisterType<ImagePlugin>().As<IImagePlugin>().InstancePerRequest(new object[0]);
           
            builder.RegisterType<BrandService>().As<IBrandService>().InstancePerRequest(new object[0]);

            builder.RegisterType<RepairService>().As<IRepairService>().InstancePerRequest(new object[0]);
            builder.RegisterType<RepairGalleryService>().As<IRepairGalleryService>().InstancePerRequest(new object[0]);
            builder.RegisterType<RepairItemService>().As<IRepairItemService>().InstancePerRequest(new object[0]);

            builder.RegisterType<LocalizedPropertyService>().As<ILocalizedPropertyService>().InstancePerRequest(new object[0]);
            builder.RegisterType<CommonServices>().As<ICommonServices>().InstancePerRequest(new object[0]);
            builder.RegisterType<WebWorkContext>().As<IWorkContext>().InstancePerRequest(new object[0]);
            builder.RegisterType<GenericAttributeService>().As<IGenericAttributeService>().InstancePerRequest(new object[0]);
            builder.RegisterType<LocaleStringResourceService>().As<ILocaleStringResourceService>().InstancePerRequest(new object[0]);
            builder.RegisterType<TextService>().As<ITextService>().InstancePerRequest(new object[0]);

            builder.RegisterType<GenericControlService>().As<IGenericControlService>().InstancePerRequest(new object[0]);
            builder.RegisterType<GenericControlValueService>().As<IGenericControlValueService>().InstancePerRequest(new object[0]);
            builder.RegisterType<GenericControlValueItemService>().As<IGenericControlValueItemService>().InstancePerRequest(new object[0]);

            builder.RegisterType<ShoppingCartItemService>().As<IShoppingCartItemService>().InstancePerRequest(new object[0]);

            builder.RegisterType<CustomerService>().As<ICustomerService>().InstancePerRequest(new object[0]);

            builder.RegisterType<OrderTotalCalculationService>().As<IOrderTotalCalculationService>().InstancePerRequest(new object[0]);

            builder.RegisterType<AddressService>().As<IAddressService>().InstancePerRequest(new object[0]);

            builder.RegisterType<PaymentMethodService>().As<IPaymentMethodService>()
                .InstancePerRequest(new object[0]);

            builder.RegisterType<ShippingMethodService>().As<IShippingMethodService>().InstancePerRequest(new object[0]);

            builder.RegisterType<OrderService>().As<IOrderService>().InstancePerRequest(new object[0]);
            builder.RegisterType<OrderItemService>().As<IOrderItemService>().InstancePerRequest(new object[0]);

            builder.RegisterType<OrderProcessingService>().As<IOrderProcessingService>().InstancePerRequest(new object[0]);
            builder.RegisterType<PriceCalculationService>().As<IPriceCalculationService>().InstancePerRequest(new object[0]);

        }
    }
}