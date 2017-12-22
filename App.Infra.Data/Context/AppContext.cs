using App.Core.Common;
using App.Domain.Common;
using App.Domain.Entities.Account;
using App.Domain.Entities.Ads;
using App.Domain.Entities.Attribute;
using App.Domain.Entities.Brandes;
using App.Domain.Entities.Data;
using App.Domain.Entities.GenericControl;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Entities.Language;
using App.Domain.Entities.Location;
using App.Domain.Entities.Menu;
using App.Domain.Entities.Other;
using App.Domain.Entities.Payments;
using App.Domain.Entities.Slide;
using App.Domain.Shippings;
using App.Infra.Data.Mapping;
using Domain.Entities.Customers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using App.Domain.Entities.Orders;
using App.Domain.Orders;

namespace App.Infra.Data.Context
{
    public class AppContext : DbContext
    {
        public virtual IDbSet<App.Domain.Entities.Attribute.Attribute> Attributes
        {
            get;
            set;
        }

        public virtual IDbSet<AttributeValue> AttributeValues
        {
            get;
            set;
        }

        public virtual IDbSet<Banner> Banners
        {
            get;
            set;
        }

        public virtual IDbSet<ContactInformation> ContactInformations
        {
            get;
            set;
        }

        public virtual IDbSet<FlowStep> FlowSteps
        {
            get;
            set;
        }

        public virtual IDbSet<GalleryImage> GalleryImages
        {
            get;
            set;
        }

        public virtual IDbSet<LandingPage> LandingPages
        {
            get;
            set;
        }

        public virtual IDbSet<Language> Languages
        {
            get;
            set;
        }

        public virtual IDbSet<ExternalLogin> Logins
        {
            get;
            set;
        }

        public virtual IDbSet<MenuLink> MenuLinks
        {
            get;
            set;
        }

        public virtual IDbSet<App.Domain.Entities.Data.News> News
        {
            get;
            set;
        }

        public virtual IDbSet<PageBanner> PageBanners
        {
            get;
            set;
        }

        public virtual IDbSet<Post> Posts
        {
            get;
            set;
        }

        public virtual IDbSet<PostGallery> PostsGallerys
        {
            get;
            set;
        }

        public virtual IDbSet<Province> Provinces
        {
            get;
            set;
        }

        public virtual IDbSet<Role> Roles
        {
            get;
            set;
        }

        public virtual IDbSet<ServerMailSetting> ServerMailSettings
        {
            get;
            set;
        }

        public virtual IDbSet<SettingSeoGlobal> SettingSeoGlobals
        {
            get;
            set;
        }

        public virtual IDbSet<SlideShow> SlideShows
        {
            get;
            set;
        }

        public virtual IDbSet<StaticContent> StaticContents
        {
            get;
            set;
        }

        public virtual IDbSet<SystemSetting> SystemSettings
        {
            get;
            set;
        }

        public virtual IDbSet<User> Users
        {
            get;
            set;
        }

        public virtual IDbSet<Brand> Brand
        {
            get;
            set;
        }

        public virtual IDbSet<Repair> Repair
        {
            get;
            set;
        }

        public virtual IDbSet<RepairGallery> RepairGallery
        {
            get;
            set;
        }

        public virtual IDbSet<RepairItem> RepairItem
        {
            get;
            set;
        }

        public virtual IDbSet<LocalizedProperty> LocalizedProperty
        {
            get;
            set;
        }

        public virtual IDbSet<GenericAttribute> GenericAttribute
        {
            get;
            set;
        }

        public virtual IDbSet<LocaleStringResource> LocaleStringResource
        {
            get;
            set;
        }

        public virtual IDbSet<GenericControl> GenericControls
        {
            get;
            set;
        }

        public virtual IDbSet<GenericControlValue> GenericControlValues
        {
            get;
            set;
        }
        public virtual IDbSet<GenericControlValueItem> GenericControlValueItems
        {
            get;
            set;
        }

        public virtual IDbSet<ShoppingCartItem> ShoppingCartItems
        {
            get;
            set;
        }

        public virtual IDbSet<Customer> Customers
        {
            get;
            set;
        }

        public virtual IDbSet<Address> Address
        {
            get;
            set;
        }

        public virtual IDbSet<PaymentMethod> PaymentMethods
        {
            get;
            set;
        }

        public virtual IDbSet<ShippingMethod> ShippingMethods
        {
            get;
            set;
        }

        public virtual IDbSet<Order> Order
        {
            get;
            set;
        }
        public virtual IDbSet<OrderItem> OrderItem
        {
            get;
            set;
        }

        public AppContext() : base("AppConnect")
        {
            base.Configuration.LazyLoadingEnabled = true;
            base.Configuration.ProxyCreationEnabled = true;
            base.Configuration.AutoDetectChangesEnabled = true;
        }

        public virtual int Commit()
        {
            IEnumerable<DbEntityEntry> dbEntityEntries =
                from x in base.ChangeTracker.Entries()
                where (!(x.Entity is IAuditableEntity) ? false : (x.State == EntityState.Added ? true : x.State == EntityState.Modified))
                select x;
            foreach (DbEntityEntry dbEntityEntry in dbEntityEntries)
            {
                IAuditableEntity entity = dbEntityEntry.Entity as IAuditableEntity;
                if (entity != null)
                {
                    string name = Thread.CurrentPrincipal.Identity.Name;
                    DateTime utcNow = DateTime.UtcNow;
                    if (dbEntityEntry.State != EntityState.Added)
                    {
                        base.Entry<IAuditableEntity>(entity).Property<string>((IAuditableEntity x) => x.CreatedBy).IsModified = false;
                        base.Entry<IAuditableEntity>(entity).Property<DateTime>((IAuditableEntity x) => x.CreatedDate).IsModified = false;
                    }
                    else
                    {
                        entity.CreatedBy = name;
                        entity.CreatedDate = utcNow;
                    }
                    entity.UpdatedBy = name;
                    entity.UpdatedDate = new DateTime?(utcNow);
                }
            }
            return this.SaveChanges();
        }

        public virtual Task<int> CommitAsync()
        {
            IEnumerable<DbEntityEntry> dbEntityEntries =
                from x in base.ChangeTracker.Entries()
                where (!(x.Entity is IAuditableEntity) ? false : (x.State == EntityState.Added ? true : x.State == EntityState.Modified))
                select x;
            foreach (DbEntityEntry dbEntityEntry in dbEntityEntries)
            {
                IAuditableEntity entity = dbEntityEntry.Entity as IAuditableEntity;
                if (entity != null)
                {
                    string name = Thread.CurrentPrincipal.Identity.Name;
                    DateTime utcNow = DateTime.UtcNow;
                    if (dbEntityEntry.State != EntityState.Added)
                    {
                        base.Entry<IAuditableEntity>(entity).Property<string>((IAuditableEntity x) => x.CreatedBy).IsModified = false;
                        base.Entry<IAuditableEntity>(entity).Property<DateTime>((IAuditableEntity x) => x.CreatedDate).IsModified = false;
                    }
                    else
                    {
                        entity.CreatedBy = name;
                        entity.CreatedDate = utcNow;
                    }
                    entity.UpdatedBy = name;
                    entity.UpdatedDate = new DateTime?(utcNow);
                }
            }
            return this.SaveChangesAsync();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Properties().Where((PropertyInfo x) => x.Name == string.Concat(x.ReflectedType.Name, "Id")).Configure((ConventionPrimitivePropertyConfiguration x) => x.IsKey().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity));

            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add<Role>(new RoleConfiguration());
            modelBuilder.Configurations.Add<ExternalLogin>(new ExternalLoginConfiguration());
            modelBuilder.Configurations.Add<Claim>(new ClaimConfiguration());
            modelBuilder.Configurations.Add<MenuLink>(new MenuLinkConfiguration());
            modelBuilder.Configurations.Add<Banner>(new BannerConfiguration());
            modelBuilder.Configurations.Add<ContactInformation>(new ContactInformationConfiguration());
            modelBuilder.Configurations.Add<LandingPage>(new LandingPageConfiguration());

            modelBuilder.Configurations.Add<Post>(new PostsConfiguration());
            modelBuilder.Configurations.Add<PostGallery>(new PostsGalleryConfiguration());

            modelBuilder.Configurations.Add<AttributeValue>(new AttribureValueConfiguration());
            modelBuilder.Configurations.Add<App.Domain.Entities.Data.News>(new NewsConfiguration());
            modelBuilder.Configurations.Add<GalleryImage>(new GalleryImageConfiguration());
            modelBuilder.Configurations.Add<StaticContent>(new StaticContentConfiguration());
            modelBuilder.Configurations.Add<FlowStep>(new FlowStepConfiguration());
            modelBuilder.Configurations.Add<Brand>(new BrandConfiguration());
            modelBuilder.Configurations.Add<Repair>(new RepairConfiguration());
            modelBuilder.Configurations.Add<RepairGallery>(new RepairGalleryConfiguration());
            modelBuilder.Configurations.Add<RepairItem>(new RepairItemConfiguration());
            modelBuilder.Configurations.Add<LocalizedProperty>(new LocalizedPropertyConfiguration());

            modelBuilder.Configurations.Add<GenericAttribute>(new GenericAttributeConfiguration());
            modelBuilder.Configurations.Add<LocaleStringResource>(new LocaleStringResourceConfiguration());

            modelBuilder.Configurations.Add(new GenericControlConfiguration());
            modelBuilder.Configurations.Add(new GenericControlValueConfiguration());
            modelBuilder.Configurations.Add(new GenericControlValueItemConfiguration());

            modelBuilder.Configurations.Add<ShoppingCartItem>(new ShoppingCartItemConfiguration());

            modelBuilder.Configurations.Add<Customer>(new CustomerConfiguration());

            modelBuilder.Configurations.Add<Address>(new AddressConfiguration());

            modelBuilder.Configurations.Add<PaymentMethod>(new PaymentMethodConfiguration());
            modelBuilder.Configurations.Add<ShippingMethod>(new ShippingMethodConfiguration());

            modelBuilder.Configurations.Add(new OrderConfiguration());
            modelBuilder.Configurations.Add(new OrderItemConfiguration());
            

        }
    }
}