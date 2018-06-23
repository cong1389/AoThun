using App.Domain.Entities.Ads;
using App.Domain.Entities.Attribute;
using App.Domain.Entities.Brandes;
using App.Domain.Entities.Data;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Entities.Identity;
using App.Domain.Entities.Language;
using App.Domain.Entities.Location;
using App.Domain.Entities.Other;
using App.Domain.Entities.Slide;
using App.FakeEntity.Ads;
using App.FakeEntity.Attribute;
using App.FakeEntity.Brandes;
using App.FakeEntity.ContactInformation;
using App.FakeEntity.Gallery;
using App.FakeEntity.Language;
using App.FakeEntity.Location;
using App.FakeEntity.News;
using App.FakeEntity.Other;
using App.FakeEntity.Post;
using App.FakeEntity.SeoGlobal;
using App.FakeEntity.ServerMail;
using App.FakeEntity.Slide;
using App.FakeEntity.Static;
using App.FakeEntity.Step;
using App.FakeEntity.System;
using App.FakeEntity.User;
using App.FakeEntity.Repairs;
using App.Aplication;
using AutoMapper;
using System;
using App.FakeEntity.GenericControl;
using App.Domain.Entities.GenericControl;
using App.Domain.Entities.Orders;
using App.Domain.Common;
using App.FakeEntity.Common;
using App.Domain.Entities.Payments;
using App.FakeEntity.Payments;
using App.Domain.Shippings;
using App.FakeEntity.Shippings;
using App.FakeEntity.Orders;
using App.Domain.Orders;
using App.FakeEntity.Menu;
using App.Domain.Entities.Menu;

namespace App.Framework.Mappings
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get
            {
                return "ViewModelToDomainMappings";
            }
        }

        public ViewModelToDomainMappingProfile()
        {
            CreateMap<RegisterFormViewModel, IdentityUser>().ForAllMembers(_ => _.Ignore());

            CreateMap<LanguageFormViewModel, Language>()
                .ForMember((Language x) => x.LanguageName, map => map.MapFrom<string>((LanguageFormViewModel vm) => vm.LanguageName))
                .ForMember((Language x) => (object)x.Id, map
                => map.MapFrom<int>((LanguageFormViewModel vm) => vm.Id))
                .ForMember((Language x) => x.LanguageCode, map
                => map.MapFrom<string>((LanguageFormViewModel vm) => vm.LanguageCode))
                .ForMember((Language x) => (object)x.Status, map
                => map.MapFrom<int>((LanguageFormViewModel vm) => vm.Status))
                .ForMember((Language x) => x.Flag, map
                => map.Condition((LanguageFormViewModel source) => !string.IsNullOrEmpty(source.Flag)));

            CreateMap<LocalizedPropertyViewModel, LocalizedProperty>()
            .ForMember((LocalizedProperty x) => x.Id, map
            => map.MapFrom<int>((LocalizedPropertyViewModel vm) => vm.Id))
            .ForMember((LocalizedProperty x) => x.EntityId, map
            => map.MapFrom<int>((LocalizedPropertyViewModel vm) => vm.EntityId))
            .ForMember((LocalizedProperty x) => (object)x.LanguageId, map
            => map.MapFrom<int>((LocalizedPropertyViewModel vm) => vm.LanguageId))
            .ForMember((LocalizedProperty x) => x.LocaleKeyGroup, map
            => map.MapFrom<string>((LocalizedPropertyViewModel vm) => vm.LocaleKeyGroup))
            .ForMember((LocalizedProperty x) => (object)x.LocaleKey, map
            => map.MapFrom<string>((LocalizedPropertyViewModel vm) => vm.LocaleKey))
            .ForMember((LocalizedProperty x) => (object)x.LocaleValue, map
            => map.MapFrom<string>((LocalizedPropertyViewModel vm) => vm.LocaleValue));

            CreateMap<ServerMailSettingViewModel, ServerMailSetting>().ForMember((ServerMailSetting x) => x.FromAddress, map => map.MapFrom<string>((ServerMailSettingViewModel vm) => vm.FromAddress)).ForMember((ServerMailSetting x) => (object)x.Id, map => map.MapFrom<int>((ServerMailSettingViewModel vm) => vm.Id)).ForMember((ServerMailSetting x) => x.SmtpClient, map => map.MapFrom<string>((ServerMailSettingViewModel vm) => vm.SmtpClient)).ForMember((ServerMailSetting x) => (object)x.Status, map => map.MapFrom<int>((ServerMailSettingViewModel vm) => vm.Status)).ForMember((ServerMailSetting x) => x.UserID, map => map.MapFrom<string>((ServerMailSettingViewModel vm) => vm.UserId)).ForMember((ServerMailSetting x) => x.Password, map => map.MapFrom<string>((ServerMailSettingViewModel vm) => vm.Password)).ForMember((ServerMailSetting x) => x.SMTPPort, map => map.MapFrom<string>((ServerMailSettingViewModel vm) => vm.SMTPPort)).ForMember((ServerMailSetting x) => (object)x.EnableSSL, map => map.MapFrom<bool>((ServerMailSettingViewModel vm) => vm.EnableSSL)).ForMember((ServerMailSetting x) => (object)x.Status, map => map.MapFrom<int>((ServerMailSettingViewModel vm) => vm.Status));

            CreateMap<LandingPageViewModel, LandingPage>().ForMember((LandingPage x) => x.FullName, map => map.MapFrom<string>((LandingPageViewModel vm) => vm.FullName)).ForMember((LandingPage x) => (object)x.ShopId, map => map.MapFrom<int>((LandingPageViewModel vm) => vm.ShopId)).ForMember((LandingPage x) => x.DateOfBith, map => map.MapFrom<string>((LandingPageViewModel vm) => vm.DateOfBith)).ForMember((LandingPage x) => (object)x.Status, map => map.MapFrom<int>((LandingPageViewModel vm) => vm.Status)).ForMember((LandingPage x) => x.PhoneNumber, map => map.MapFrom<string>((LandingPageViewModel vm) => vm.PhoneNumber)).ForMember((LandingPage x) => x.Email, map => map.MapFrom<string>((LandingPageViewModel vm) => vm.Email));

            CreateMap<ContactInformationViewModel, ContactInformation>()
                .ForMember((ContactInformation x) => (object)x.Id, map => map.MapFrom<int>((ContactInformationViewModel vm) => vm.Id))
                .ForMember((ContactInformation x) => x.Language, map => map.MapFrom<string>((ContactInformationViewModel vm) => vm.Language))
                .ForMember((ContactInformation x) => (object)x.Status, map => map.MapFrom<int>((ContactInformationViewModel vm) => vm.Status))
                .ForMember((ContactInformation x) => x.Lag, map => map.MapFrom<string>((ContactInformationViewModel vm) => vm.Lag))
                .ForMember((ContactInformation x) => x.Lat, map => map.MapFrom<string>((ContactInformationViewModel vm) => vm.Lat))
                .ForMember((ContactInformation x) => x.NumberOfStore, map => map.MapFrom<string>((ContactInformationViewModel vm) => vm.NumberOfStore))
                .ForMember((ContactInformation x) => (object)x.Type, map => map.MapFrom<int>((ContactInformationViewModel vm) => vm.Type))
                .ForMember((ContactInformation x) => (object)x.OrderDisplay, map => map.MapFrom<int>((ContactInformationViewModel vm) => vm.OrderDisplay))
                .ForMember((ContactInformation x) => (object)x.Status, map => map.MapFrom<int>((ContactInformationViewModel vm) => vm.Status))
                .ForMember((ContactInformation x) => x.Email, map => map.MapFrom<string>((ContactInformationViewModel vm) => vm.Email))
                .ForMember((ContactInformation x) => x.Hotline, map => map.MapFrom<string>((ContactInformationViewModel vm) => vm.Hotline))
                .ForMember((ContactInformation x) => x.Address, map => map.MapFrom<string>((ContactInformationViewModel vm) => vm.Address))
                .ForMember((ContactInformation x) => x.MobilePhone, map => map.MapFrom<string>((ContactInformationViewModel vm) => vm.MobilePhone))
                .ForMember((ContactInformation x) => x.Fax, map => map.MapFrom<string>((ContactInformationViewModel vm) => vm.Fax))
                .ForMember((ContactInformation x) => x.Province, map => map.Ignore())
                .ForMember((ContactInformation x) => (object)x.ProvinceId, map => map.MapFrom<int>((ContactInformationViewModel vm) => vm.ProvinceId));

            CreateMap<SystemSettingViewModel, SystemSetting>()
                .ForMember((SystemSetting x) => x.Title, map => map.MapFrom<string>((SystemSettingViewModel vm) => vm.Title))
                .ForMember((SystemSetting x) => (object)x.Id, map => map.MapFrom<int>((SystemSettingViewModel vm) => vm.Id))
                .ForMember((SystemSetting x) => (object)x.Status, map => map.MapFrom<int>((SystemSettingViewModel vm) => vm.Status))
                .ForMember((SystemSetting x) => x.MetaTitle, map => map.MapFrom<string>((SystemSettingViewModel vm) => vm.MetaTitle))
                .ForMember((SystemSetting x) => x.MetaDescription, map => map.MapFrom<string>((SystemSettingViewModel vm) => vm.MetaDescription))
                .ForMember((SystemSetting x) => x.MetaKeywords, map => map.MapFrom<string>((SystemSettingViewModel vm) => vm.MetaKeywords))
                .ForMember((SystemSetting x) => x.Description, map => map.MapFrom<string>((SystemSettingViewModel vm) => vm.Description))
                .ForMember((SystemSetting x) => x.TimeWork, map => map.MapFrom<string>((SystemSettingViewModel vm) => vm.TimeWork))
                .ForMember((SystemSetting x) => x.Hotline, map => map.MapFrom<string>((SystemSettingViewModel vm) => vm.Hotline))
                .ForMember((SystemSetting x) => x.Email, map => map.MapFrom<string>((SystemSettingViewModel vm) => vm.Email))
                .ForMember((SystemSetting x) => x.LogoImage, map => map.MapFrom<string>((SystemSettingViewModel vm) => vm.LogoImage))
                 .ForMember((SystemSetting x) => x.LogoFooterImage, map => map.MapFrom<string>((SystemSettingViewModel vm) => vm.LogoFooterImage))
                .ForMember((SystemSetting x) => x.FaviconImage, map => map.MapFrom<string>((SystemSettingViewModel vm) => vm.FaviconImage))
                .ForMember((SystemSetting x) => x.Slogan, map => map.MapFrom<string>((SystemSettingViewModel vm) => vm.Slogan))
                .ForMember((SystemSetting x) => x.FooterContent, map => map.MapFrom<string>((SystemSettingViewModel vm) => vm.FooterContent))
                .ForMember((SystemSetting x) => (object)x.MaintanceSite, map => map.MapFrom<bool>((SystemSettingViewModel vm) => vm.MaintanceSite));

            CreateMap<MenuLinkViewModel, MenuLink>()
                   .ForMember((MenuLink x) => x.MenuName, map => map.MapFrom<string>((MenuLinkViewModel vm) => vm.MenuName))
                   .ForMember((MenuLink x) => (object)x.Id, map => map.MapFrom<int>((MenuLinkViewModel vm) => vm.Id))
                   .ForMember((MenuLink x) => (object)x.Status, map => map.MapFrom<int>((MenuLinkViewModel vm) => vm.Status))
                   .ForMember((MenuLink x) => x.MetaTitle, map => map.MapFrom<string>((MenuLinkViewModel vm) => vm.MetaTitle))
                   .ForMember((MenuLink x) => x.MetaDescription, map => map.MapFrom<string>((MenuLinkViewModel vm) => vm.MetaDescription))
                   .ForMember((MenuLink x) => x.MetaKeywords, map => map.MapFrom<string>((MenuLinkViewModel vm) => vm.MetaKeywords))
                   .ForMember((MenuLink x) => (object)x.ParentId, map => map.MapFrom<int?>((MenuLinkViewModel vm) => vm.ParentId))
                   .ForMember((MenuLink x) => x.CurrentVirtualId, map => map.Condition((MenuLinkViewModel source) => !string.IsNullOrEmpty(source.CurrentVirtualId)))
                   .ForMember((MenuLink x) => x.VirtualId, map => map.Condition((MenuLinkViewModel source) => !string.IsNullOrEmpty(source.VirtualId)))
                   .ForMember((MenuLink x) => (object)x.TypeMenu, map => map.MapFrom<int>((MenuLinkViewModel vm) => vm.TypeMenu))
                   .ForMember((MenuLink x) => (object)x.Position, map => map.MapFrom<int>((MenuLinkViewModel vm) => vm.Position))
                   .ForMember((MenuLink x) => (object)x.TemplateType, map => map.MapFrom<int>((MenuLinkViewModel vm) => vm.TemplateType))
                   .ForMember((MenuLink x) => (object)x.DisplayOnMenu, map => map.MapFrom<bool>((MenuLinkViewModel vm) => vm.DisplayOnMenu))
                   .ForMember((MenuLink x) => (object)x.OrderDisplay, map => map.MapFrom<int>((MenuLinkViewModel vm) => vm.OrderDisplay))
                   .ForMember((MenuLink x) => x.SourceLink, map => map.MapFrom<string>((MenuLinkViewModel vm) => vm.SourceLink))
                   .ForMember((MenuLink x) => x.SeoUrl, map => map.MapFrom<string>((MenuLinkViewModel vm) => vm.SeoUrl))
                   .ForMember((MenuLink x) => x.VirtualSeoUrl, map => map.Condition((MenuLinkViewModel source) => !string.IsNullOrEmpty(source.VirtualSeoUrl)))
                   .ForMember((MenuLink x) => x.Language, map => map.MapFrom<string>((MenuLinkViewModel vm) => vm.Language))
                   .ForMember((MenuLink x) => x.ImageUrl, map => map.Condition((MenuLinkViewModel source) => !string.IsNullOrEmpty(source.ImageUrl)))
                   .ForMember((MenuLink x) => x.Icon1, map => map.Condition((MenuLinkViewModel source) => !string.IsNullOrEmpty(source.Icon1)))
                   .ForMember((MenuLink x) => x.Icon2, map => map.Condition((MenuLinkViewModel source) => !string.IsNullOrEmpty(source.Icon2)))
                   .ForMember((MenuLink x) => x.DisplayOnHomePage, map => map.MapFrom<bool>((MenuLinkViewModel vm) => vm.DisplayOnHomePage))
                   .ForMember((MenuLink x) => x.ParentMenu, map => map.Ignore())
                    .ForMember((MenuLink x) => x.GenericControls, map => map.Condition((MenuLinkViewModel source) => source.GenericControls.IsAny()));

            CreateMap<ProvinceViewModel, Province>()
                .ForMember((Province x) => x.Name, map => map.MapFrom<string>((ProvinceViewModel vm) => vm.Name))
                .ForMember((Province x) => (object)x.Id, map => map.MapFrom<int>((ProvinceViewModel vm) => vm.Id)).ForMember((Province x) => (object)x.OrderDisplay, map => map.MapFrom<int>((ProvinceViewModel vm) => vm.OrderDisplay)).ForMember((Province x) => (object)x.Status, map => map.MapFrom<int>((ProvinceViewModel vm) => vm.Status));


            CreateMap<AttributeViewModel, App.Domain.Entities.Attribute.Attribute>()
                .ForMember((App.Domain.Entities.Attribute.Attribute x) => x.AttributeName, map => map.MapFrom<string>((AttributeViewModel vm) => vm.AttributeName))
                .ForMember((App.Domain.Entities.Attribute.Attribute x) => (object)x.Id, map
                => map.MapFrom<int>((AttributeViewModel vm) => vm.Id))
                .ForMember((App.Domain.Entities.Attribute.Attribute x) => (object)x.OrderDisplay, map
                => map.MapFrom<int?>((AttributeViewModel vm) => vm.OrderDisplay))
                .ForMember((App.Domain.Entities.Attribute.Attribute x) => x.Description, map
                => map.MapFrom<string>((AttributeViewModel vm) => vm.Description))
                .ForMember((App.Domain.Entities.Attribute.Attribute x) => (object)x.Status, map
                => map.MapFrom<int>((AttributeViewModel vm) => vm.Status))
                .ForMember((App.Domain.Entities.Attribute.Attribute x) => x.AttributeValues, map => map.Ignore());

            CreateMap<AttributeValueViewModel, AttributeValue>()
                .ForMember((AttributeValue x) => x.ValueName, map => map.MapFrom<string>((AttributeValueViewModel vm) => vm.ValueName))
                .ForMember((AttributeValue x) => (object)x.Id, map => map.MapFrom<int>((AttributeValueViewModel vm) => vm.Id)).ForMember((AttributeValue x) => (object)x.OrderDisplay, map => map.MapFrom<int?>((AttributeValueViewModel vm) => vm.OrderDisplay))
                .ForMember((AttributeValue x) => x.ColorHex, map => map.MapFrom<string>((AttributeValueViewModel vm) => vm.ColorHex))
                .ForMember((AttributeValue x) => x.Description, map => map.MapFrom<string>((AttributeValueViewModel vm) => vm.Description)).ForMember((AttributeValue x) => (object)x.AttributeId, map => map.MapFrom<int>((AttributeValueViewModel vm) => vm.AttributeId))
                .ForMember((AttributeValue x) => x.Attribute, map => map.Ignore())
                .ForMember((AttributeValue x) => (object)x.Status, map => map.MapFrom<int>((AttributeValueViewModel vm) => vm.Status));

            CreateMap<GalleryImageViewModel, GalleryImage>();

            CreateMap<PostViewModel, Post>()
                    .ForMember(dest => dest.Title, opt => opt.MapFrom<string>(src => src.Title))
                    .ForMember(dest => (object)dest.Id, opt => opt.MapFrom<int>(src => src.Id))
                    .ForMember(dest => (object)dest.Status, opt
                    => opt.MapFrom<int>(src => src.Status))
                    .ForMember(dest => dest.MetaTitle, opt
                    => opt.MapFrom<string>(src => src.MetaTitle))
                    .ForMember(dest => dest.MetaDescription, opt
                    => opt.MapFrom<string>(src => src.MetaDescription))
                    .ForMember(dest => dest.MetaKeywords, opt
                    => opt.MapFrom<string>(src => src.MetaKeywords))
                    .ForMember(dest => (object)dest.MenuId, opt
                    => opt.MapFrom<int>(src => (int)src.MenuId))
                    .ForMember(dest => dest.TechInfo, opt
                    => opt.MapFrom<string>(src => src.TechInfo))
                    .ForMember(dest => (object)dest.PostType, opt
                    => opt.MapFrom<int>(src => src.PostType))
                    .ForMember(dest => (object)dest.OldOrNew, opt
                    => opt.MapFrom<bool>(src => src.OldOrNew))
                    .ForMember(dest => (object)dest.Price, opt
                    => opt.MapFrom<double?>(src => src.Price))
                    .ForMember(dest => (object)dest.Discount, opt
                    => opt.MapFrom<double?>(src => src.Discount))

                    .ForMember(dest => (object)dest.ShowOnHomePage, opt
                    => opt.MapFrom<bool>(src => src.ShowOnHomePage))

                    .ForMember(dest => (object)dest.ProductHot, opt
                    => opt.MapFrom<bool>(src => src.ProductHot))
                    .ForMember(dest => (object)dest.OutOfStock, opt
                    => opt.MapFrom<bool>(src => src.OutOfStock))
                    .ForMember(dest => dest.Description, opt
                    => opt.MapFrom<string>(src => src.Description))
                    .ForMember(dest => dest.ProductCode, opt
                    => opt.MapFrom<string>(src => src.ProductCode))
                    .ForMember(dest => (object)dest.ProductNew, opt
                    => opt.MapFrom<bool>(src => src.ProductNew))
                    .ForMember(dest => dest.ShortDesc, opt
                    => opt.MapFrom<string>(src => src.ShortDesc))
                    .ForMember(dest => (object)dest.OrderDisplay, opt
                    => opt.MapFrom<int>(src => src.OrderDisplay))
                    .ForMember(dest => dest.SeoUrl, opt
                    => opt.MapFrom<string>(src => src.SeoUrl))
                    .ForMember(dest => dest.VirtualCatUrl, opt
                    => opt.Condition((PostViewModel source) => !string.IsNullOrEmpty(source.VirtualCatUrl)))
                    .ForMember(dest => dest.Language, opt
                    => opt.MapFrom<string>(src => src.Language))
                    .ForMember(dest => dest.ImageBigSize, opt
                    => opt.Condition((PostViewModel source) => !string.IsNullOrEmpty(source.ImageBigSize)))
                    .ForMember(dest => dest.ImageMediumSize, opt
                    => opt.Condition((PostViewModel source) => !string.IsNullOrEmpty(source.ImageMediumSize)))
                    .ForMember(dest => dest.ImageSmallSize, opt
                    => opt.Condition((PostViewModel source) => !string.IsNullOrEmpty(source.ImageSmallSize)))
                    .ForMember(dest => (object)dest.StartDate, opt
                    => opt.MapFrom<DateTime?>(src => src.StartDate))
                    .ForMember(dest => (object)dest.EndDate, opt
                    => opt.MapFrom<DateTime?>(src => src.EndDate))

                    //.ForMember(dest => dest.AttributeValues, opt => opt.Condition((PostViewModel source) => source.AttributeValues.IsAny<AttributeValueViewModel>()))
                    //.ForMember(dest => dest.PostGallerys, opt => opt.Condition((PostViewModel source) => source.PostGallerys.IsAny<PostGalleryViewModel>()))

                    .ForMember(dest => dest.VirtualCategoryId, opt => opt.Condition((PostViewModel source) => !string.IsNullOrEmpty(source.VirtualCategoryId)))
                    .ForMember(dest => dest.AttributeValues, opt => opt.Ignore())
                    .ForMember(dest => dest.GalleryImages, opt => opt.Ignore())
                    .ForMember(dest => dest.PostGallerys, opt => opt.Ignore())
                    .ForMember(dest => dest.MenuLink, opt => opt.Ignore());



            CreateMap<PostGalleryViewModel, PostGallery>()
            .ForMember((PostGallery x) => x.Title, map => map.MapFrom<string>((PostGalleryViewModel vm) => vm.Title))
            .ForMember((PostGallery x) => x.ImageSmallSize, map => map.MapFrom<string>((PostGalleryViewModel vm) => vm.ImageSmallSize))
            .ForMember((PostGallery x) => x.ImageBigSize, map => map.MapFrom<string>((PostGalleryViewModel vm) => vm.ImageBigSize))
            .ForMember((PostGallery x) => x.ImageMediumSize, map => map.MapFrom<string>((PostGalleryViewModel vm) => vm.ImageMediumSize))
             .ForMember((PostGallery x) => (object)x.PostId, map => map.MapFrom<int>((PostGalleryViewModel vm) => vm.PostId))
             .ForMember((PostGallery x) => (object)x.OrderDisplay, map => map.MapFrom<int>((PostGalleryViewModel vm) => vm.OrderDisplay));

            CreateMap<NewsViewModel, News>()
                .ForMember((News x) => x.Title, map => map.MapFrom<string>((NewsViewModel vm) => vm.Title))
                .ForMember((News x) => (object)x.Id, map => map.MapFrom<int>((NewsViewModel vm) => vm.Id))
                .ForMember((News x) => (object)x.Status, map => map.MapFrom<int>((NewsViewModel vm) => vm.Status))
                .ForMember((News x) => x.MetaTitle, map => map.MapFrom<string>((NewsViewModel vm) => vm.MetaTitle))
                .ForMember((News x) => x.MetaDescription, map => map.MapFrom<string>((NewsViewModel vm) => vm.MetaDescription))
                .ForMember((News x) => x.MetaKeywords, map => map.MapFrom<string>((NewsViewModel vm) => vm.MetaKeywords))
                .ForMember((News x) => (object)x.MenuId, map => map.MapFrom<int>((NewsViewModel vm) => vm.MenuId))
                .ForMember((News x) => x.Description, map => map.MapFrom<string>((NewsViewModel vm) => vm.Description))
                .ForMember((News x) => x.ShortDesc, map => map.MapFrom<string>((NewsViewModel vm) => vm.ShortDesc))
                .ForMember((News x) => (object)x.Video, map => map.MapFrom<bool>((NewsViewModel vm) => vm.Video))
                .ForMember((News x) => x.VideoLink, map => map.MapFrom<string>((NewsViewModel vm) => vm.VideoLink))
                .ForMember((News x) => x.OtherLink, map => map.MapFrom<string>((NewsViewModel vm) => vm.OtherLink))
                .ForMember((News x) => (object)x.OrderDisplay, map => map.MapFrom<string>((NewsViewModel vm) => vm.OrderDisplay))
                .ForMember((News x) => (object)x.SpecialDisplay, map => map.MapFrom<bool>((NewsViewModel vm) => vm.SpecialDisplay))
                .ForMember((News x) => (object)x.HomeDisplay, map => map.MapFrom<bool>((NewsViewModel vm) => vm.HomeDisplay))
                .ForMember((News x) => x.SeoUrl, map => map.MapFrom<string>((NewsViewModel vm) => vm.SeoUrl))
                .ForMember((News x) => x.VirtualCatUrl, map => map.Condition((NewsViewModel source) => !string.IsNullOrEmpty(source.VirtualCatUrl)))
                .ForMember((News x) => x.Language, map => map.MapFrom<string>((NewsViewModel vm) => vm.Language))
                .ForMember((News x) => x.ImageBigSize, map => map.Condition((NewsViewModel source) => !string.IsNullOrEmpty(source.ImageBigSize)))
                .ForMember((News x) => x.ImageMediumSize, map => map.Condition((NewsViewModel source) => !string.IsNullOrEmpty(source.ImageMediumSize)))
                .ForMember((News x) => x.ImageSmallSize, map => map.Condition((NewsViewModel source) => !string.IsNullOrEmpty(source.ImageSmallSize)))
                .ForMember((News x) => x.VirtualCategoryId, map => map.Condition((NewsViewModel source) => !string.IsNullOrEmpty(source.VirtualCategoryId)))
                .ForMember((News x) => x.MenuLink, map => map.Ignore());

            CreateMap<FlowStepViewModel, FlowStep>()
                .ForMember((FlowStep x) => x.Title, map => map.MapFrom<string>((FlowStepViewModel vm) => vm.Title))
                .ForMember((FlowStep x) => (object)x.Id, map => map.MapFrom<int>((FlowStepViewModel vm) => vm.Id))
                .ForMember((FlowStep x) => (object)x.Status, map => map.MapFrom<int>((FlowStepViewModel vm) => vm.Status))
                .ForMember((FlowStep x) => x.Description, map => map.MapFrom<string>((FlowStepViewModel vm) => vm.Description))
                .ForMember((FlowStep x) => x.OtherLink, map => map.MapFrom<string>((FlowStepViewModel vm) => vm.OtherLink))
                .ForMember((FlowStep x) => (object)x.OrderDisplay, map => map.MapFrom<int>((FlowStepViewModel vm) => vm.OrderDisplay))
                .ForMember((FlowStep x) => x.ImageUrl, map => map.Condition((FlowStepViewModel source) => !string.IsNullOrEmpty(source.ImageUrl)));

            CreateMap<StaticContentViewModel, StaticContent>().
                ForMember((StaticContent x) => x.Title, map => map.MapFrom<string>((StaticContentViewModel vm) => vm.Title)).ForMember((StaticContent x) => (object)x.Id, map
                    => map.MapFrom<int>((StaticContentViewModel vm)
                    => vm.Id)).ForMember((StaticContent x) => (object)x.Status, map
                     => map.MapFrom<int>((StaticContentViewModel vm) => vm.Status)).ForMember((StaticContent x)
                     => x.MetaTitle, map => map.MapFrom<string>((StaticContentViewModel vm)
                      => vm.MetaTitle)).ForMember((StaticContent x) => x.MetaDescription, map
                       => map.MapFrom<string>((StaticContentViewModel vm) => vm.MetaDescription)).ForMember((StaticContent x)
                       => x.MetaKeywords, map
                        => map.MapFrom<string>((StaticContentViewModel vm) => vm.MetaKeywords)).ForMember((StaticContent x)
                        => (object)x.MenuId, map => map.MapFrom<int>((StaticContentViewModel vm) => vm.MenuId)).ForMember((StaticContent x) => x.Description, map => map.MapFrom<string>((StaticContentViewModel vm) => vm.Description)).ForMember((StaticContent x) => x.ShortDesc, map => map.MapFrom<string>((StaticContentViewModel vm) => vm.ShortDesc)).ForMember((StaticContent x) => x.SeoUrl, map => map.MapFrom<string>((StaticContentViewModel vm) => vm.SeoUrl)).ForMember((StaticContent x) => x.Language, map => map.MapFrom<string>((StaticContentViewModel vm) => vm.Language)).ForMember((StaticContent x) => x.ImagePath, map => map.Condition((StaticContentViewModel source) => !string.IsNullOrEmpty(source.ImagePath))).ForMember((StaticContent x) => (object)x.ViewCount, map => map.MapFrom<int>((StaticContentViewModel vm) => vm.ViewCount)).ForMember((StaticContent x) => x.MenuLink, map => map.Ignore());


            CreateMap<SettingSeoGlobalViewModel, SettingSeoGlobal>()
                .ForMember((SettingSeoGlobal x) => x.FbAppId, map => map.MapFrom<string>((SettingSeoGlobalViewModel vm) => vm.FbAppId))
                .ForMember((SettingSeoGlobal x) => (object)x.Id, map => map.MapFrom<int>((SettingSeoGlobalViewModel vm) => vm.Id))
                .ForMember((SettingSeoGlobal x) => x.FbAdminsId, map => map.MapFrom<string>((SettingSeoGlobalViewModel vm) => vm.FbAdminsId))
                .ForMember((SettingSeoGlobal x) => x.SnippetGoogleAnalytics, map => map.MapFrom<string>((SettingSeoGlobalViewModel vm) => vm.SnippetGoogleAnalytics))
                .ForMember((SettingSeoGlobal x) => x.MetaTagMasterTool, map => map.MapFrom<string>((SettingSeoGlobalViewModel vm) => vm.MetaTagMasterTool))
                .ForMember((SettingSeoGlobal x) => x.PublisherGooglePlus, map => map.MapFrom<string>((SettingSeoGlobalViewModel vm) => vm.PublisherGooglePlus))
                .ForMember((SettingSeoGlobal x) => x.FacebookRetargetSnippet, map => map.MapFrom<string>((SettingSeoGlobalViewModel vm) => vm.FacebookRetargetSnippet))
                .ForMember((SettingSeoGlobal x) => x.GoogleRetargetSnippet, map => map.MapFrom<string>((SettingSeoGlobalViewModel vm) => vm.GoogleRetargetSnippet))
                .ForMember((SettingSeoGlobal x) => (object)x.Status, map => map.MapFrom<int>((SettingSeoGlobalViewModel vm) => vm.Status))

                .ForMember((SettingSeoGlobal x) => x.FbLink, map => map.MapFrom<string>((SettingSeoGlobalViewModel vm) => vm.FbLink))
                .ForMember((SettingSeoGlobal x) => x.GooglePlusLink, map => map.MapFrom<string>((SettingSeoGlobalViewModel vm) => vm.GooglePlusLink))
                .ForMember((SettingSeoGlobal x) => x.TwitterLink, map => map.MapFrom<string>((SettingSeoGlobalViewModel vm) => vm.TwitterLink))
                .ForMember((SettingSeoGlobal x) => x.PinterestLink, map => map.MapFrom<string>((SettingSeoGlobalViewModel vm) => vm.PinterestLink))
                .ForMember((SettingSeoGlobal x) => x.YoutubeLink, map => map.MapFrom<string>((SettingSeoGlobalViewModel vm) => vm.YoutubeLink))
                ;

            CreateMap<PageBannerViewModel, PageBanner>()
                .ForMember((PageBanner x) => x.PageName, map => map.MapFrom<string>((PageBannerViewModel vm) => vm.PageName))
                .ForMember((PageBanner x) => (object)x.Id, map => map.MapFrom<int>((PageBannerViewModel vm) => vm.Id))
                .ForMember((PageBanner x) => x.Language, map => map.MapFrom<string>((PageBannerViewModel vm) => vm.Language))
                .ForMember((PageBanner x) => (object)x.OrderDisplay, map => map.MapFrom<int>((PageBannerViewModel vm) => vm.OrderDisplay))
                .ForMember((PageBanner x) => (object)x.Position, map => map.MapFrom<int>((PageBannerViewModel vm) => vm.Position))
                .ForMember((PageBanner x) => (object)x.Status, map => map.MapFrom<int>((PageBannerViewModel vm) => vm.Status));

            CreateMap<BannerViewModel, Banner>()
                .ForMember((Banner x) => x.Title, map
                => map.MapFrom<string>((BannerViewModel vm) => vm.Title))
                .ForMember((Banner x) => (object)x.Id, map
                => map.MapFrom<int>((BannerViewModel vm) => vm.Id))
                .ForMember((Banner x) => (object)x.Status, map
                => map.MapFrom<int>((BannerViewModel vm) => vm.Status))
                .ForMember((Banner x) => x.WebsiteLink, map
                => map.MapFrom<string>((BannerViewModel vm) => vm.WebsiteLink))
                .ForMember((Banner x) => x.ImgPath, map
                => map.Condition((BannerViewModel source) => !string.IsNullOrEmpty(source.ImgPath)))
                .ForMember((Banner x) => x.Width, map
                => map.MapFrom<string>((BannerViewModel vm) => vm.Width))
                .ForMember((Banner x) => x.Language, map
                => map.MapFrom<string>((BannerViewModel vm) => vm.Language))
                .ForMember((Banner x) => x.Height, map
                => map.MapFrom<string>((BannerViewModel vm) => vm.Height))
                .ForMember((Banner x) => x.Target, map
                => map.MapFrom<string>((BannerViewModel vm) => vm.Target))
                .ForMember((Banner x) => (object)x.OrderDisplay, map
                => map.MapFrom<int>((BannerViewModel vm) => vm.OrderDisplay))
                .ForMember((Banner x) => (object)x.FromDate, map
                => map.MapFrom<TimeSpan?>((BannerViewModel vm) => vm.FromDate))
                .ForMember((Banner x) => (object)x.ToDate, map
                => map.MapFrom<TimeSpan?>((BannerViewModel vm) => vm.ToDate))
                .ForMember((Banner x) => (object)x.PageId, map
                => map.MapFrom<int>((BannerViewModel vm) => vm.PageId))
                .ForMember((Banner x) => (object)x.MenuId, map
                => map.MapFrom<int?>((BannerViewModel vm) => vm.MenuId))
                .ForMember((Banner x) => x.Language, map
                => map.MapFrom<string>((BannerViewModel vm) => vm.Language))
                .ForMember((Banner x) => x.PageBanner, map => map.Ignore())
                .ForMember((Banner x) => x.MenuLink, map => map.Ignore());

            CreateMap<SlideShowViewModel, SlideShow>().ForMember((SlideShow x) => x.Title, map => map.MapFrom<string>((SlideShowViewModel vm) => vm.Title)).ForMember((SlideShow x) => (object)x.Id, map => map.MapFrom<int>((SlideShowViewModel vm) => vm.Id)).ForMember((SlideShow x) => (object)x.Status, map => map.MapFrom<int>((SlideShowViewModel vm) => vm.Status)).ForMember((SlideShow x) => x.WebsiteLink, map => map.MapFrom<string>((SlideShowViewModel vm) => vm.WebsiteLink)).ForMember((SlideShow x) => x.ImgPath, map => map.Condition((SlideShowViewModel source) => !string.IsNullOrEmpty(source.ImgPath))).ForMember((SlideShow x) => x.Width, map => map.MapFrom<string>((SlideShowViewModel vm) => vm.Width)).ForMember((SlideShow x) => x.Description, map => map.MapFrom<string>((SlideShowViewModel vm) => vm.Description)).ForMember((SlideShow x) => (object)x.Video, map => map.MapFrom<bool>((SlideShowViewModel vm) => vm.Video)).ForMember((SlideShow x) => x.Height, map => map.MapFrom<string>((SlideShowViewModel vm) => vm.Height)).ForMember((SlideShow x) => x.Target, map => map.MapFrom<string>((SlideShowViewModel vm) => vm.Target)).ForMember((SlideShow x) => (object)x.OrderDisplay, map => map.MapFrom<int>((SlideShowViewModel vm) => vm.OrderDisplay)).ForMember((SlideShow x) => (object)x.FromDate, map => map.MapFrom<TimeSpan?>((SlideShowViewModel vm) => vm.FromDate)).ForMember((SlideShow x) => (object)x.ToDate, map => map.MapFrom<TimeSpan?>((SlideShowViewModel vm) => vm.ToDate)).ForMember((SlideShow x) => (object)x.OrderDisplay, map => map.MapFrom<int>((SlideShowViewModel vm) => vm.OrderDisplay));

            CreateMap<BrandViewModel, Brand>()
                .ForMember((Brand x) => x.Name, map => map.MapFrom<string>((BrandViewModel vm) => vm.Name))
                .ForMember((Brand x) => (object)x.Id, map => map.MapFrom<int>((BrandViewModel vm) => vm.Id))
                .ForMember((Brand x) => (object)x.OrderDisplay, map => map.MapFrom<int>((BrandViewModel vm) => vm.OrderDisplay))
                .ForMember((Brand x) => (object)x.Status, map => map.MapFrom<int>((BrandViewModel vm) => vm.Status));

            CreateMap<RepairViewModel, Repair>()
                .ForMember((Repair x) => x.Model, map
                => map.MapFrom<string>((RepairViewModel vm) => vm.Model))
                .ForMember((Repair x) => (object)x.Id, map => map.MapFrom<int>((RepairViewModel vm) => vm.Id))
                .ForMember((Repair x) => (object)x.Status, map => map.MapFrom<int>((RepairViewModel vm) => vm.Status))
                .ForMember((Repair x) => x.ModelBrand, map => map.MapFrom<string>((RepairViewModel vm) => vm.ModelBrand))
                .ForMember((Repair x) => x.SerialNumber, map => map.MapFrom<string>((RepairViewModel vm) => vm.SerialNumber))
                .ForMember((Repair x) => (object)x.BrandId, map => map.MapFrom<int>((RepairViewModel vm) => vm.BrandId))
                .ForMember((Repair x) => x.RepairCode, map => map.MapFrom<string>((RepairViewModel vm) => vm.RepairCode))
                .ForMember((Repair x) => x.CustomerCode, map => map.Condition((RepairViewModel source) => !string.IsNullOrEmpty(source.CustomerCode)))
                .ForMember((Repair x) => x.StoreName, map => map.Condition((RepairViewModel source) => !string.IsNullOrEmpty(source.StoreName)))
                .ForMember((Repair x) => x.CustomerName, map => map.MapFrom<string>((RepairViewModel vm) => vm.CustomerName))
                .ForMember((Repair x) => x.PhoneNumber, map => map.MapFrom<string>((RepairViewModel vm) => vm.PhoneNumber))
                .ForMember((Repair x) => x.CustomerIdNumber, map => map.MapFrom<string>((RepairViewModel vm) => vm.CustomerIdNumber))
                .ForMember((Repair x) => x.Address, map => map.MapFrom<string>((RepairViewModel vm) => vm.Address))
                .ForMember((Repair x) => x.Accessories, map => map.MapFrom<string>((RepairViewModel vm) => vm.Accessories))
                .ForMember((Repair x) => x.PasswordPhone, map => map.MapFrom<string>((RepairViewModel vm) => vm.PasswordPhone))
                .ForMember((Repair x) => x.AppleId, map => map.MapFrom<string>((RepairViewModel vm) => vm.AppleId))
                .ForMember((Repair x) => x.IcloudPassword, map => map.Condition((RepairViewModel source) => !string.IsNullOrEmpty(source.IcloudPassword)))
                .ForMember((Repair x) => (object)x.OldWarranty, map => map.MapFrom<string>((RepairViewModel source) => source.OldWarranty))
                .ForMember((Repair x) => x.PhoneStatus, map => map.Condition((RepairViewModel source) => !string.IsNullOrEmpty(source.PhoneStatus)))
                .ForMember((Repair x) => x.Note, map => map.MapFrom<string>((RepairViewModel vm) => vm.Note)).
                ForMember((Repair x) => x.RepairGalleries, map => map.Ignore())
                .ForMember((Repair x) => x.RepairItems, map => map.Ignore()).ForMember((Repair x) => x.Brand, map => map.Ignore());

            CreateMap<RepairGalleryViewModel, RepairGallery>();

            CreateMap<RepairItemViewModel, RepairItem>().ForMember((RepairItem x) => (object)x.Id, map => map.MapFrom<int>((RepairItemViewModel vm) => vm.Id)).ForMember((RepairItem x) => (object)x.RepairId, map => map.MapFrom<int>((RepairItemViewModel vm) => vm.RepairId)).ForMember((RepairItem x) => (object)x.FixedFee, map => map.MapFrom<decimal?>((RepairItemViewModel vm) => vm.FixedFee)).ForMember((RepairItem x) => (object)x.WarrantyFrom, map => map.MapFrom<DateTime?>((RepairItemViewModel vm) => vm.WarrantyFrom))
                .ForMember((RepairItem x) => x.Repair, map => map.Ignore()).ForMember((RepairItem x) => (object)x.WarrantyTo, map => map.MapFrom<DateTime?>((RepairItemViewModel vm) => vm.WarrantyTo));

            CreateMap<LocaleStringResourceViewModel, LocaleStringResource>()
              .ForMember((LocaleStringResource x) => x.LanguageId, map => map.MapFrom<int>((LocaleStringResourceViewModel vm) => vm.LanguageId))
              .ForMember((LocaleStringResource x) => x.ResourceName, map => map.MapFrom<string>((LocaleStringResourceViewModel vm) => vm.ResourceName))
              .ForMember((LocaleStringResource x) => x.ResourceValue, map => map.MapFrom<string>((LocaleStringResourceViewModel vm) => vm.ResourceValue))
              .ForMember((LocaleStringResource x) => x.IsFromPlugin, map => map.MapFrom<bool>((LocaleStringResourceViewModel vm) => vm.IsFromPlugin))
              .ForMember((LocaleStringResource x) => x.IsTouched, map => map.MapFrom<bool>((LocaleStringResourceViewModel vm) => vm.IsTouched));

            CreateMap<GenericControlViewModel, GenericControl>()
                 .ForMember((GenericControl x)
                 => x.Name, map
                 => map.MapFrom<string>((GenericControlViewModel vm) => vm.Name))
                 .ForMember((App.Domain.Entities.GenericControl.GenericControl x)
                 => (object)x.Id, map
                 => map.MapFrom<int>((GenericControlViewModel vm) => vm.Id))
                 .ForMember((App.Domain.Entities.GenericControl.GenericControl x) => (object)x.OrderDisplay
                 , map => map.MapFrom<int?>((GenericControlViewModel vm) => vm.OrderDisplay))
                 .ForMember((App.Domain.Entities.GenericControl.GenericControl x) => x.Description
                 , map => map.MapFrom<string>((GenericControlViewModel vm) => vm.Description))
                 .ForMember((App.Domain.Entities.GenericControl.GenericControl x) => (object)x.Status, map
                 => map.MapFrom<int>((GenericControlViewModel vm) => vm.Status))
                 .ForMember((App.Domain.Entities.GenericControl.GenericControl x) => (object)x.MenuId, map
                 => map.MapFrom<int>((GenericControlViewModel vm) => vm.MenuId))
                 .ForMember((App.Domain.Entities.GenericControl.GenericControl x) => (object)x.ControlTypeId, map
                 => map.MapFrom<int?>((GenericControlViewModel vm) => vm.ControlTypeId))
                 .ForMember((GenericControl x) => x.GenericControlValues, map => map.Ignore())
                 .ForMember((GenericControl x) => x.MenuLinks, map => map.Ignore());

            CreateMap<GenericControlValueViewModel, GenericControlValue>()
                .ForMember((GenericControlValue x) => x.ValueName, map
                => map.MapFrom<string>((GenericControlValueViewModel vm) => vm.ValueName))
                .ForMember((GenericControlValue x) => (object)x.Id, map
                => map.MapFrom<int>((GenericControlValueViewModel vm) => vm.Id))
                .ForMember((GenericControlValue x) => (object)x.OrderDisplay, map
                => map.MapFrom<int?>((GenericControlValueViewModel vm) => vm.OrderDisplay))
                .ForMember((GenericControlValue x) => x.ColorHex, map
                => map.MapFrom<string>((GenericControlValueViewModel vm) => vm.ColorHex))
                .ForMember((GenericControlValue x) => x.Description, map
                => map.MapFrom<string>((GenericControlValueViewModel vm) => vm.Description))
                .ForMember((GenericControlValue x) => (object)x.GenericControlId, map
                => map.MapFrom<int>((GenericControlValueViewModel vm) => vm.GenericControlId))
                .ForMember((GenericControlValue x) => x.GenericControl, map=> map.Ignore())
                .ForMember((GenericControlValue x) => (object)x.Status, map=> map.MapFrom<int>((GenericControlValueViewModel vm) => vm.Status))
                .ForMember((GenericControlValue x) => (object)x.EntityId, map
                => map.MapFrom<int>((GenericControlValueViewModel vm) => vm.EntityId));

            CreateMap<GenericControlValueItemViewModel, GenericControlValueItem>();

            CreateMap<ShoppingCartItemViewModel, ShoppingCartItem>()
             .ForMember((ShoppingCartItem x) => x.StoreId, map
             => map.MapFrom<int>((ShoppingCartItemViewModel vm) => vm.StoreId))
             .ForMember((ShoppingCartItem x) => (object)x.ParentItemId, map
             => map.MapFrom<int>((ShoppingCartItemViewModel vm) => vm.ParentItemId))
             .ForMember((ShoppingCartItem x) => (object)x.BundleItemId, map
             => map.MapFrom<int?>((ShoppingCartItemViewModel vm) => vm.BundleItemId))
             .ForMember((ShoppingCartItem x) => x.ShoppingCartTypeId, map
             => map.MapFrom<int>((ShoppingCartItemViewModel vm) => vm.ShoppingCartTypeId))
             .ForMember((ShoppingCartItem x) => x.CustomerId, map
             => map.MapFrom<int>((ShoppingCartItemViewModel vm) => vm.CustomerId))
             .ForMember((ShoppingCartItem x) => (object)x.AttributesXml, map
             => map.MapFrom<string>((ShoppingCartItemViewModel vm) => vm.AttributesXml))
             .ForMember((ShoppingCartItem x) => (decimal)x.CustomerEnteredPrice, map
             => map.MapFrom<decimal>((ShoppingCartItemViewModel vm) => vm.CustomerEnteredPrice))
             .ForMember((ShoppingCartItem x) => (int)x.Quantity, map
             => map.MapFrom<int>((ShoppingCartItemViewModel vm) => vm.Quantity));

            CreateMap<AddressViewModel, Address>()
              .ForMember((Address x) => x.FirstName, map
             => map.MapFrom<string>((AddressViewModel vm) => vm.FirstName))
             .ForMember((Address x) => (object)x.LastName, map
             => map.MapFrom<string>((AddressViewModel vm) => vm.LastName))
             .ForMember((Address x) => (object)x.Email, map
             => map.MapFrom<string>((AddressViewModel vm) => vm.Email))
             .ForMember((Address x) => x.Company, map
             => map.MapFrom<string>((AddressViewModel vm) => vm.Company))
             .ForMember((Address x) => x.CountryId, map
             => map.MapFrom<int>((AddressViewModel vm) => vm.CountryId))
             .ForMember((Address x) => (object)x.StateProvinceId, map
             => map.MapFrom<int>((AddressViewModel vm) => vm.StateProvinceId))
             .ForMember((Address x) => (string)x.City, map
             => map.MapFrom<string>((AddressViewModel vm) => vm.City))
             .ForMember((Address x) => (string)x.Address1, map
             => map.MapFrom<string>((AddressViewModel vm) => vm.Address1))
             .ForMember((Address x) => (string)x.Address2, map
             => map.MapFrom<string>((AddressViewModel vm) => vm.Address2))
             .ForMember((Address x) => (string)x.ZipPostalCode, map
             => map.MapFrom<string>((AddressViewModel vm) => vm.ZipPostalCode))
             .ForMember((Address x) => (string)x.PhoneNumber, map
             => map.MapFrom<string>((AddressViewModel vm) => vm.PhoneNumber))
             .ForMember((Address x) => (string)x.FaxNumber, map
             => map.MapFrom<string>((AddressViewModel vm) => vm.FaxNumber))
              .ForMember((Address x) => (string)x.Salutation, map
             => map.MapFrom<string>((AddressViewModel vm) => vm.Salutation))
             .ForMember((Address x) => (string)x.Title, map
             => map.MapFrom<string>((AddressViewModel vm) => vm.Title))
             .ForMember((Address x) => (string)x.Title, map
             => map.MapFrom<string>((AddressViewModel vm) => vm.Title));

            CreateMap<PaymentMethodViewModel, PaymentMethod>()
               .ForMember((PaymentMethod x) => x.PaymentMethodSystemName, map => map.MapFrom<string>((PaymentMethodViewModel vm) => vm.PaymentMethodSystemName))
               .ForMember((PaymentMethod x) => x.FullDescription, map => map.MapFrom<string>((PaymentMethodViewModel vm) => vm.FullDescription));

            CreateMap<ShippingMethodViewModel, ShippingMethod>()
              .ForMember((ShippingMethod x) => x.Name, map => map.MapFrom<string>((ShippingMethodViewModel vm) => vm.Name))
              .ForMember((ShippingMethod x) => x.Description, map => map.MapFrom<string>((ShippingMethodViewModel vm) => vm.Description))
              .ForMember((ShippingMethod x) => x.DisplayOrder, map => map.MapFrom<int>((ShippingMethodViewModel vm) => vm.DisplayOrder));

            CreateMap<OrderViewModel, Order>()
            .ForMember((Order x) => x.OrderNumber, map => map.MapFrom<string>((OrderViewModel vm) => vm.OrderNumber))
            .ForMember((Order x) => x.OrderGuid, map => map.MapFrom<Guid>((OrderViewModel vm) => vm.OrderGuid))
            .ForMember((Order x) => x.CreatedOnUtc, map => map.MapFrom<DateTime>((OrderViewModel vm) => vm.CreatedOn))
            .ForMember((Order x) => x.BillingAddress, map => map.MapFrom<Address>((OrderViewModel vm) => vm.BillingAddress));

            CreateMap<OrderItemViewModel, OrderItem>()
          .ForMember((OrderItem x) => x.OrderId, map => map.MapFrom<int>((OrderItemViewModel vm) => vm.OrderId))
          .ForMember((OrderItem x) => x.OrderItemGuid, map => map.MapFrom<Guid>((OrderItemViewModel vm) => vm.OrderItemGuid))
          .ForMember((OrderItem x) => x.PostId, map => map.MapFrom<int>((OrderItemViewModel vm) => vm.PostId));
        }
    }
}