using App.Domain.Common;
using App.Domain.Entities.Data;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Entities.Identity;
using App.Domain.Entities.Menu;
using App.Domain.Entities.Payments;
using App.Domain.Entities.Slide;
using App.Domain.Orders;
using App.FakeEntity.Common;
using App.FakeEntity.Menu;
using App.FakeEntity.Orders;
using App.FakeEntity.Payments;
using App.Service.Addresses;
using App.Service.Language;
using Domain.Entities.Customers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Aplication.Extensions
{
    public static class MappingExtensions
    {
        public static StaticContent ToModel(this StaticContent entity)
        {
            if (entity == null)
                return null;

            var model = new StaticContent
            {
                Id = entity.Id,
                MenuId = entity.MenuId,
                VirtualCategoryId = entity.VirtualCategoryId,
                Language = entity.Language,
                Status = entity.Status,
                SeoUrl = entity.SeoUrl,
                ImagePath = entity.ImagePath,

                Title = entity.GetLocalized(x => x.Title, entity.Id),
                ShortDesc = entity.GetLocalized(x => x.ShortDesc, entity.Id),
                Description = entity.GetLocalized(x => x.Description, entity.Id),
                MetaTitle = entity.GetLocalized(x => x.MetaTitle, entity.Id),
                MetaKeywords = entity.GetLocalized(x => x.MetaKeywords, entity.Id),
                MetaDescription = entity.GetLocalized(x => x.MetaDescription, entity.Id)

            };
            return model;
        }

        public static News ToModel(this News entity)
        {
            if (entity == null)
                return null;

            var model = new News
            {
                Id = entity.Id,
                MenuId = entity.MenuId,
                VirtualCategoryId = entity.VirtualCategoryId,
                Language = entity.Language,
                Status = entity.Status,
                SeoUrl = entity.SeoUrl,
                ImageBigSize = entity.ImageBigSize,
                ImageMediumSize = entity.ImageMediumSize,
                ImageSmallSize = entity.ImageSmallSize,
                CreatedDate = entity.CreatedDate,
                MenuLink = entity.MenuLink,
                OrderDisplay= entity.OrderDisplay,
                OtherLink= entity.OtherLink,
                SpecialDisplay= entity.SpecialDisplay,

                Title = entity.GetLocalized(x => x.Title, entity.Id),
                ShortDesc = entity.GetLocalized(x => x.ShortDesc, entity.Id),
                Description = entity.GetLocalized(x => x.Description, entity.Id),
                MetaTitle = entity.GetLocalized(x => x.MetaTitle, entity.Id),
                MetaKeywords = entity.GetLocalized(x => x.MetaKeywords, entity.Id),
                MetaDescription = entity.GetLocalized(x => x.MetaDescription, entity.Id)
            };

            return model;
        }

        public static Post ToModel(this Post entity)
        {
            if (entity == null)
                return null;

            var model = new Post
            {
                Id = entity.Id,
                MenuId = entity.MenuId,
                VirtualCategoryId = entity.VirtualCategoryId,
                Language = entity.Language,
                Status = entity.Status,
                SeoUrl = entity.SeoUrl,
                ImageBigSize = entity.ImageBigSize,
                ImageMediumSize = entity.ImageMediumSize,
                ImageSmallSize = entity.ImageSmallSize,
                Price = entity.Price,
                Discount = entity.Discount,
                ProductHot = entity.ProductHot,
                OutOfStock = entity.OutOfStock,
                ProductNew = entity.ProductNew,
                VirtualCatUrl = entity.VirtualCatUrl,
                StartDate = entity.StartDate,
                PostType = entity.PostType,
                OldOrNew = entity.OldOrNew,
                MenuLink = entity.MenuLink,
                CreatedDate=entity.CreatedDate,
                UpdatedDate=entity.UpdatedDate,
                UpdatedBy=entity.UpdatedBy,
                CreatedBy=entity.CreatedBy,

                PostGallerys = entity.PostGallerys,
                GalleryImages = entity.GalleryImages,
                AttributeValues = entity.AttributeValues,

                Title = entity.GetLocalized(x => x.Title, entity.Id),
                ProductCode = entity.GetLocalized(x => x.ProductCode, entity.Id),
                TechInfo = entity.GetLocalized(x => x.TechInfo, entity.Id),
                ShortDesc = entity.GetLocalized(x => x.ShortDesc, entity.Id),
                Description = entity.GetLocalized(x => x.Description, entity.Id),
                MetaTitle = entity.GetLocalized(x => x.MetaTitle, entity.Id),
                MetaKeywords = entity.GetLocalized(x => x.MetaKeywords, entity.Id),
                MetaDescription = entity.GetLocalized(x => x.MetaDescription, entity.Id),
            };

            return model;
        }

        public static MenuLink ToModel(this MenuLink entity)
        {
            if (entity == null)
                return null;

            var model = new MenuLink
            {
                Id = entity.Id,
                ParentId = entity.ParentId,
                Status = entity.Status,
                TypeMenu = entity.TypeMenu,
                Position = entity.Position,
                MenuName = entity.GetLocalized(x => x.MenuName, entity.Id),
                SeoUrl = entity.SeoUrl,
                OrderDisplay = entity.OrderDisplay,
                ImageUrl = entity.ImageUrl,
                Icon1 = entity.Icon1,
                Icon2 = entity.Icon2,
                CurrentVirtualId = entity.CurrentVirtualId,
                VirtualId = entity.VirtualId,
                TemplateType = entity.TemplateType,
                MetaTitle = entity.GetLocalized(x => x.MetaTitle, entity.Id),
                MetaKeywords = entity.GetLocalized(x => x.MetaKeywords, entity.Id),
                MetaDescription = entity.GetLocalized(x => x.MetaDescription, entity.Id),

                Language = entity.Language,
                SourceLink = entity.SourceLink,
                VirtualSeoUrl = entity.VirtualSeoUrl,
                DisplayOnHomePage = entity.DisplayOnHomePage,
                DisplayOnMenu = entity.DisplayOnMenu,
                DisplayOnSearch = entity.DisplayOnSearch,
            };

            return model;
        }

        public static ContactInformation ToModel(this ContactInformation entity)
        {
            if (entity == null)
                return null;

            var model = new ContactInformation
            {
                Id = entity.Id,
                Email = entity.Email,
                Fax = entity.Fax,
                Hotline = entity.Hotline,
                Lag = entity.Lag,
                Language = entity.Language,
                Lat = entity.Lat,
                MobilePhone = entity.MobilePhone,
                NumberOfStore = entity.NumberOfStore,
                OrderDisplay = entity.OrderDisplay,
                ProvinceId = entity.ProvinceId,
                Status = entity.Status,

                Title = entity.GetLocalized(x => x.Title, entity.Id),
                Address = entity.GetLocalized(x => x.Address, entity.Id)
            };
            return model;
        }

        public static SystemSetting ToModel(this SystemSetting entity)
        {
            if (entity == null)
                return null;

            var model = new SystemSetting
            {
                Id = entity.Id,
                Language = entity.Language,
                Status = entity.Status,
                FaviconImage = entity.FaviconImage,
                LogoImage = entity.LogoImage,
                LogoFooterImage = entity.LogoFooterImage,
                MaintanceSite = entity.MaintanceSite,
                Hotline = entity.Hotline,
                Email = entity.Email,
                TimeWork = entity.TimeWork,

                Title = entity.GetLocalized(x => x.Title, entity.Id),
                FooterContent = entity.GetLocalized(x => x.FooterContent, entity.Id),
                MetaTitle = entity.GetLocalized(x => x.MetaTitle, entity.Id),
                MetaDescription = entity.GetLocalized(x => x.MetaDescription, entity.Id),
                MetaKeywords = entity.GetLocalized(x => x.MetaKeywords, entity.Id),
                Description = entity.GetLocalized(x => x.Description, entity.Id)
            };

            return model;
        }

        public static Address ToEntity(this AddressViewModel model)
        {
            if (model == null)
                return null;

            var entity = new Address();
            return ToEntity(model, entity);
        }

        public static Address ToEntity(this AddressViewModel model, Address destination)
        {
            if (model == null)
                return destination;

            destination.Id = model.Id;
            destination.Salutation = model.Salutation;
            destination.Title = model.Title;
            destination.FirstName = model.FirstName;
            destination.LastName = model.LastName;
            destination.Email = model.Email;
            destination.Company = model.Company;
            destination.CountryId = model.CountryId;
            destination.StateProvinceId = model.StateProvinceId;
            destination.City = model.City;
            destination.Address1 = model.Address1;
            destination.Address2 = model.Address2;
            destination.ZipPostalCode = model.ZipPostalCode;
            destination.PhoneNumber = model.PhoneNumber;
            destination.FaxNumber = model.FaxNumber;

            return destination;
        }

        public static void PrepareModel(this Address model, Address address)
        {
            model.Id = address.Id;
            model.Salutation = address.Salutation;
            model.Title = address.Title;
            model.FirstName = address.FirstName;
            model.LastName = address.LastName;
            model.Email = address.Email;
            model.Company = address.Company;
            model.CountryId = address.CountryId;
            model.StateProvinceId = address.StateProvinceId;
            model.City = address.City;
            model.Address1 = address.Address1;
            model.Address2 = address.Address2;
            model.ZipPostalCode = address.ZipPostalCode;
            model.PhoneNumber = address.PhoneNumber;
            model.FaxNumber = address.FaxNumber;
        }

        public static PaymentMethodViewModel ToModel(this PaymentMethod model, PaymentMethodViewModel destination)
        {
            if (model == null)
                return null;

            destination.PaymentMethodSystemName = model.PaymentMethodSystemName;
            destination.FullDescription = model.FullDescription;

            return destination;
        }

        public static OrderViewModel ToModel(this Order model, OrderViewModel destination)
        {
            if (model == null)
                throw new ArgumentNullException("Order");

            if (destination == null)
                throw new ArgumentNullException("OrderViewModel");

            try
            {
                destination.Id = model.Id;
                destination.OrderNumber = model.OrderNumber;

                destination.OrderGuid = model.OrderGuid;
                destination.StoreId = model.StoreId;
                destination.CustomerId = model.CustomerId;
                destination.BillingAddressId = model.BillingAddressId;
                destination.ShippingAddressId = model.ShippingAddressId;
                destination.OrderStatusId = model.OrderStatusId;
                destination.ShippingStatusId = model.ShippingStatusId;
                destination.PaymentStatusId = model.PaymentStatusId;
                destination.PaymentMethodSystemName = model.PaymentMethodSystemName;
                destination.CustomerCurrencyCode = model.CustomerCurrencyCode;
                destination.CurrencyRate = model.CurrencyRate;

                destination.CustomerTaxDisplayTypeId = model.CustomerTaxDisplayTypeId;
                destination.VatNumber = model.VatNumber;
                destination.OrderSubtotalInclTax = model.OrderSubtotalInclTax;
                destination.OrderSubtotalExclTax = model.OrderSubtotalExclTax;

                destination.OrderSubTotalDiscountInclTax = model.OrderSubTotalDiscountInclTax;
                destination.OrderSubTotalDiscountExclTax = model.OrderSubTotalDiscountExclTax;
                destination.OrderShippingInclTax = model.OrderShippingInclTax;
                destination.OrderShippingExclTax = model.OrderShippingExclTax;

                destination.PaymentMethodAdditionalFeeInclTax = model.PaymentMethodAdditionalFeeInclTax;
                destination.PaymentMethodAdditionalFeeExclTax = model.PaymentMethodAdditionalFeeExclTax;
                destination.TaxRates = model.TaxRates;
                destination.OrderTax = model.OrderTax;

                destination.OrderDiscount = model.OrderDiscount;
                destination.OrderTotal = model.OrderTotal;
                destination.RefundedAmount = model.RefundedAmount;
                destination.RewardPointsWereAdded = model.RewardPointsWereAdded;

                destination.CheckoutAttributeDescription = model.CheckoutAttributeDescription;
                destination.CheckoutAttributesXml = model.CheckoutAttributesXml;
                destination.CustomerLanguageId = model.CustomerLanguageId;
                destination.AffiliateId = model.AffiliateId;

                destination.CustomerIp = model.CustomerIp;
                destination.AllowStoringCreditCardNumber = model.AllowStoringCreditCardNumber;
                destination.CardType = model.CardType;
                destination.CardName = model.CardName;

                destination.CardNumber = model.CardNumber;
                destination.MaskedCreditCardNumber = model.MaskedCreditCardNumber;
                destination.CardCvv2 = model.CardCvv2;
                destination.CardExpirationMonth = model.CardExpirationMonth;

                destination.CardExpirationYear = model.CardExpirationYear;
                destination.AllowStoringDirectDebit = model.AllowStoringDirectDebit;
                destination.DirectDebitAccountHolder = model.DirectDebitAccountHolder;
                destination.DirectDebitAccountNumber = model.DirectDebitAccountNumber;

                destination.DirectDebitBankCode = model.DirectDebitBankCode;
                destination.DirectDebitBankName = model.DirectDebitBankName;
                destination.DirectDebitBIC = model.DirectDebitBIC;
                destination.DirectDebitCountry = model.DirectDebitCountry;

                destination.DirectDebitIban = model.DirectDebitIban;
                destination.AuthorizationTransactionId = model.AuthorizationTransactionId;
                destination.AuthorizationTransactionCode = model.AuthorizationTransactionCode;
                destination.AuthorizationTransactionResult = model.AuthorizationTransactionResult;

                destination.CaptureTransactionId = model.CaptureTransactionId;
                destination.CaptureTransactionResult = model.CaptureTransactionResult;
                destination.SubscriptionTransactionId = model.SubscriptionTransactionId;
                destination.PurchaseOrderNumber = model.PurchaseOrderNumber;

                destination.PaidDateUtc = model.PaidDateUtc;
                destination.ShippingMethod = model.ShippingMethod;
                destination.ShippingRateComputationMethodSystemName = model.ShippingRateComputationMethodSystemName;
                destination.Deleted = model.Deleted;

                destination.CreatedOnUtc = model.CreatedOnUtc;
                destination.UpdatedOnUtc = model.UpdatedOnUtc;
                destination.RewardPointsRemaining = model.RewardPointsRemaining;
                destination.CustomerOrderComment = model.CustomerOrderComment;

                destination.OrderShippingTaxRate = model.OrderShippingTaxRate;
                destination.PaymentMethodAdditionalFeeTaxRate = model.PaymentMethodAdditionalFeeTaxRate;
                destination.HasNewPaymentNotification = model.HasNewPaymentNotification;
                destination.AcceptThirdPartyEmailHandOver = model.AcceptThirdPartyEmailHandOver;

                destination.CreatedOn = model.CreatedOnUtc;
                destination.UpdatedOn = model.UpdatedOnUtc;
                destination.OrderTotal = model.OrderTotal;

                destination.OrderStatus = ((OrderStatus)model.OrderStatusId).ToString();
                destination.PaymentStatus = ((PaymentStatus)model.PaymentStatusId).ToString();
                destination.ShippingStatus = ((ShippingStatus)model.ShippingStatusId).ToString();

                destination.BillingAddress = model.BillingAddress;
                destination.ShippingAddress = model.ShippingAddress;
                destination.CustomerName = model.BillingAddress.GetFullName();
                destination.CustomerEmail = model.BillingAddress.Email;
            }
            catch (Exception ex)
            {

                throw;
            }

            return destination;
        }

        public static MenuLinkViewModel ToModel(this MenuLink model, MenuLinkViewModel destination)
        {
            if (model == null)
                throw new ArgumentNullException("MenuLink");

            if (destination == null)
                throw new ArgumentNullException("MenuLinkViewModel");

            destination.Id = model.Id;
            destination.ParentId = model.ParentId;
            destination.Status = model.Status;
            destination.TypeMenu = model.TypeMenu;
            destination.Position = model.Position;
            destination.MenuName = model.MenuName;
            destination.SeoUrl = model.SeoUrl;
            destination.OrderDisplay = model.OrderDisplay;
            destination.ImageUrl = model.ImageUrl;
            destination.Icon1 = model.Icon1;
            destination.Icon2 = model.Icon2;
            destination.CurrentVirtualId = model.CurrentVirtualId;
            destination.VirtualId = model.VirtualId;
            destination.TemplateType = model.TemplateType;
            destination.MetaTitle = model.GetLocalized(x => x.MetaTitle, model.Id);
            destination.MetaKeywords = model.GetLocalized(x => x.MetaKeywords, model.Id);
            destination.MetaDescription = model.GetLocalized(x => x.MetaDescription, model.Id);

            destination.Language = model.Language;
            destination.SourceLink = model.SourceLink;
            destination.VirtualSeoUrl = model.VirtualSeoUrl;
            destination.DisplayOnHomePage = model.DisplayOnHomePage;
            destination.DisplayOnMenu = model.DisplayOnMenu;
            destination.DisplayOnSearch = model.DisplayOnSearch;
            destination.GenericControls = model.GenericControls;

            return destination;
        }

        public static FlowStep ToModel(this FlowStep entity)
        {
            if (entity == null)
                return null;

            var model = new FlowStep
            {
                Id = entity.Id,
                OtherLink = entity.OtherLink,
                Status = entity.Status,
                OrderDisplay = entity.OrderDisplay,
                ImageUrl = entity.ImageUrl,

                Title = entity.GetLocalized(x => x.Title, entity.Id),
                Description = entity.GetLocalized(x => x.Description, entity.Id)

            };
            return model;
        }

        public static SlideShow ToModel(this SlideShow entity)
        {
            if (entity == null)
                return null;

            var model = new SlideShow
            {
                Id = entity.Id,
                Status = entity.Status,
                WebsiteLink = entity.WebsiteLink,
                ImgPath = entity.ImgPath,
                Video = entity.Video,
                Width = entity.Width,
                Height = entity.Height,
                Target = entity.Target,
                FromDate = entity.FromDate,
                ToDate = entity.ToDate,
                OrderDisplay = entity.OrderDisplay,

                Title = entity.GetLocalized(x => x.Title, entity.Id),
                Description = entity.GetLocalized(x => x.Description, entity.Id),
            };

            return model;
        }

        public static Customer ToModel(this IdentityUser entity)
        {
            if (entity == null)
                return null;
           
            var model = new Customer
            {
                Username = entity.UserName,
                Email = entity.Email,
                Password = entity.PasswordHash,
                CreatedOnUtc = DateTime.UtcNow,
                LastLoginDateUtc = DateTime.UtcNow,
                LastActivityDateUtc = DateTime.UtcNow,

            };

            return model;
        }
    }

}
