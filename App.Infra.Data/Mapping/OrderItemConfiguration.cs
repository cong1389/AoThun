using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using App.Domain.Orders;

namespace App.Infra.Data.Mapping
{
    public class OrderItemConfiguration : EntityTypeConfiguration<OrderItem>
    {
        public OrderItemConfiguration()
        {
            base.ToTable("OrderItem");
            base.HasKey<int>((OrderItem x) => x.Id).Property<int>((OrderItem x) => x.Id)
                .HasColumnName("Id").HasColumnType("int")
                .HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity)).IsRequired();

            this.HasRequired(orderItem => orderItem.Order)
               .WithMany(o => o.OrderItems)
               .HasForeignKey(orderItem => orderItem.OrderId);

            this.HasRequired(orderItem => orderItem.Post)
                .WithMany()
                .HasForeignKey(orderItem => orderItem.PostId);
        }
    }
}