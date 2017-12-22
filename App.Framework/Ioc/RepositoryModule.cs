using App.Domain.Interfaces.Repository;
using App.Infra.Data.Common;
using App.Infra.Data.Repository.Account;
using App.Infra.Data.Repository.Ads;
using App.Infra.Data.Repository.Attribute;
using App.Infra.Data.Repository.ContactInformation;
using App.Infra.Data.Repository.Gallery;
using App.Infra.Data.Repository.Language;
using App.Infra.Data.Repository.Locations;
using App.Infra.Data.Repository.MailSetting;
using App.Infra.Data.Repository.Menu;
using App.Infra.Data.Repository.News;
using App.Infra.Data.Repository.Other;
using App.Infra.Data.Repository.Post;
using App.Infra.Data.Repository.SeoSetting;
using App.Infra.Data.Repository.Slide;
using App.Infra.Data.Repository.Static;
using App.Infra.Data.Repository.Step;
using App.Infra.Data.Repository.System;
using App.Infra.Data.RepositoryAsync.Post;
using App.Infra.Data.Repository.Brandes;
using Autofac;
using System;
using App.Infra.Data.Repository.Orders;
using App.Infra.Data.Repository.GenericAttribute;
using App.Infra.Data.Repository.LocaleStringResource;
using App.Infra.Data.Repository.GenericControl;
using App.Infra.Data.Repository.Customers;
using App.Infra.Data.Repository.Addresses;
using App.Infra.Data.Repository.PaymentMethodes;
using App.Infra.Data.Repository.ShippingMethods;
using App.Core.Caching;
using App.Infra.Data.Repository.Repairs;
using App.Infra.Data.Repository.Orderes;

namespace App.Framework.Ioc
{
    public class RepositoryModule : Module
    {
        public RepositoryModule()
        {
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(RepositoryBase<>)).As(new Type[] { typeof(IRepositoryBase<>) });
            builder.RegisterGeneric(typeof(RepositoryBaseAsync<>)).As(new Type[] { typeof(IRepositoryBaseAsync<>) });
            builder.RegisterType<LanguageRepository>().As<ILanguageRepository>().InstancePerRequest(new object[0]);
            builder.RegisterType<MailSettingRepository>().As<IMailSettingRepository>().InstancePerRequest(new object[0]);
            builder.RegisterType<ContactInfoRepository>().As<IContactInfoRepository>().InstancePerRequest(new object[0]);
            builder.RegisterType<SystemSettingRepository>().As<ISystemSettingRepository>().InstancePerRequest(new object[0]);
            builder.RegisterType<MenuLinkRepository>().As<IMenuLinkRepository>().InstancePerRequest(new object[0]);
            builder.RegisterType<ProvinceRepository>().As<IProvinceRepository>().InstancePerRequest(new object[0]);
            builder.RegisterType<DistrictRepository>().As<IDistrictRepository>().InstancePerRequest(new object[0]);

            builder.RegisterType<PostRepository>().As<IPostRepository>().InstancePerRequest(new object[0]);
            builder.RegisterType<PostGalleryRepository>().As<IPostGalleryRepository>().InstancePerRequest(new object[0]);

            builder.RegisterType<NewsRepository>().As<INewsRepository>().InstancePerRequest(new object[0]);
            builder.RegisterType<StaticContentRepository>().As<IStaticContentRepository>().InstancePerRequest(new object[0]);
            builder.RegisterType<SettingSeoGlobalRepository>().As<ISettingSeoGlobalRepository>().InstancePerRequest(new object[0]);
            builder.RegisterType<PageBannerRepository>().As<IPageBannerRepository>().InstancePerRequest(new object[0]);
            builder.RegisterType<LandingPageRepository>().As<ILandingPageRepository>().InstancePerRequest(new object[0]);
            builder.RegisterType<BannerRepository>().As<IBannerRepository>().InstancePerRequest(new object[0]);
            builder.RegisterType<GalleryRepository>().As<IGalleryRepository>().InstancePerRequest(new object[0]);
            builder.RegisterType<AttributeRepository>().As<IAttributeRepository>().InstancePerRequest(new object[0]);
            builder.RegisterType<AttributeValueRepository>().As<IAttributeValueRepository>().InstancePerRequest(new object[0]);
            builder.RegisterType<SlideShowRepository>().As<ISlideShowRepository>().InstancePerRequest(new object[0]);
            builder.RegisterType<FlowStepRepository>().As<IFlowStepRepository>().InstancePerRequest(new object[0]);
            builder.RegisterType<PostRepositoryAsync>().As<IPostRepositoryAsync>().InstancePerRequest(new object[0]);
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerRequest(new object[0]);
            builder.RegisterType<RoleRepository>().As<IRoleRepository>().InstancePerRequest(new object[0]);
            builder.RegisterType<ExternalLoginRepository>().As<IExternalLoginRepository>().InstancePerRequest(new object[0]);
         
            builder.RegisterType<BrandRepository>().As<IBrandRepository>().InstancePerRequest(new object[0]);
            builder.RegisterType<RepairRepository>().As<IRepairRepository>().InstancePerRequest(new object[0]);
            builder.RegisterType<RepairGalleryRepository>().As<IRepairGalleryRepository>().InstancePerRequest(new object[0]);
            builder.RegisterType<RepairItemRepository>().As<IRepairItemRepository>().InstancePerRequest(new object[0]);
            builder.RegisterType<LocalizedPropertyRepository>().As<ILocalizedPropertyRepository>().InstancePerRequest(new object[0]);
            builder.RegisterType<GenericAttributeRepository>().As<IGenericAttributeRepository>().InstancePerRequest(new object[0]);
            builder.RegisterType<LocaleStringResourceRepository>().As<ILocaleStringResourceRepository>().InstancePerRequest(new object[0]);

            builder.RegisterType<GenericControlRepository>().As<IGenericControlRepository>().InstancePerRequest(new object[0]);
            builder.RegisterType<GenericControlValueRepository>().As<IGenericControlValueRepository>().InstancePerRequest(new object[0]);
            builder.RegisterType<GenericControlValueItemRepository>().As<IGenericControlValueItemRepository>().InstancePerRequest(new object[0]);

            builder.RegisterType<ShoppingCartItemRepository>().As<IShoppingCartItemRepository>().InstancePerRequest(new object[0]);

            builder.RegisterType<CustomerRepository>().As<ICustomerRepository>().InstancePerRequest(new object[0]);

            builder.RegisterType<AddressRepository>().As<IAddressRepository>().InstancePerRequest(new object[0]);

            builder.RegisterType<PaymentMethodRepository>()
                .As<IPaymentMethodRepository>()
                .InstancePerRequest(new object[0]);

            builder.RegisterType<ShippingMethodRepository>().As<IShippingMethodRepository>().InstancePerRequest(new object[0]);

            builder.RegisterType<MemoryCacheManager>().As<ICacheManager>().SingleInstance();
            builder.RegisterType<MemoryCacheManager>().Named<ICacheManager>("memory").SingleInstance();

            // Request cache
            //builder.RegisterType<RequestCache>().As<IRequestCache>().InstancePerRequest();

            builder.RegisterType<OrderRepository>().As<IOrderRepository>().InstancePerRequest(new object[0]);
            builder.RegisterType<OrderItemRepository>().As<IOrderItemRepository>().InstancePerRequest(new object[0]);

        }
    }

}