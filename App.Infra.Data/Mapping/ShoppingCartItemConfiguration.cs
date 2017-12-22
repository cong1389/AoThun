using App.Domain.Entities.Orders;
using Domain.Entities.Customers;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace App.Infra.Data.Mapping
{
    public class ShoppingCartItemConfiguration : EntityTypeConfiguration<ShoppingCartItem>
    {
        public ShoppingCartItemConfiguration()
        {
            base.ToTable("ShoppingCartItem");

            base.HasKey<int>((ShoppingCartItem x) => x.Id).Property<int>((ShoppingCartItem x) => x.Id).HasColumnName("Id").HasColumnType("int").HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity)).IsRequired();

            base.HasRequired<Customer>((ShoppingCartItem x) => x.Customers)
             .WithMany((Customer x) => x.ShoppingCartItems)
             .HasForeignKey<int>((ShoppingCartItem x) => x.CustomerId);           
        }
    }
}