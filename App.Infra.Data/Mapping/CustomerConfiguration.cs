using App.Domain.Common;
using Domain.Entities.Customers;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;

namespace App.Infra.Data.Mapping
{
    public class CustomerConfiguration : EntityTypeConfiguration<Customer>
    {
        public CustomerConfiguration()
        {
            base.ToTable("Customer");
            base.HasKey<int>((Customer x) => x.Id).Property<int>((Customer x) => x.Id).HasColumnName("Id").HasColumnType("int").HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity)).IsRequired();

            this.HasMany<Address>(c => c.Addresses)
                .WithMany()
                .Map(m => m.ToTable("CustomerAddresses"));
            this.HasOptional<Address>(c => c.BillingAddress);
            this.HasOptional<Address>(c => c.ShippingAddress);

            //base.HasMany<Address>((Customer x) => x.Addresses)
            //  .WithMany((Address x) => x.Customers)
            //  .Map((ManyToManyAssociationMappingConfiguration x) =>
            //  {
            //      x.ToTable("CustomerAddresses");
            //      x.MapLeftKey(new string[] { "Customer_Id" });
            //      x.MapRightKey(new string[] { "Address_Id" });
            //  });
        }
    }
}