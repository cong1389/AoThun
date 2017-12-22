using App.Domain.Entities.Brandes;
using App.Domain.Shippings;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace App.Infra.Data.Mapping
{
    public class ShippingMethodConfiguration : EntityTypeConfiguration<ShippingMethod>
    {
        public ShippingMethodConfiguration()
        {
            base.ToTable("ShippingMethod");
            base.HasKey<int>((ShippingMethod x) => x.Id)
                .Property<int>((ShippingMethod x) => x.Id).HasColumnName("Id")
                .HasColumnType("int").HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity)).IsRequired();
        }
    }
}