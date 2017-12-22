using App.Core.Common;
using App.Domain.Entities.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;

namespace App.Infra.Data.Mapping
{
    public class RepairGalleryConfiguration : EntityTypeConfiguration<RepairGallery>
    {
        public RepairGalleryConfiguration()
        {
            base.ToTable("RepairGallery");

            base.HasKey<int>((RepairGallery x) => x.Id).Property<int>((RepairGallery x) => x.Id).HasColumnName("Id").HasColumnType("int").HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity)).IsRequired();

            base.HasRequired<Repair>((RepairGallery x) => x.Repairs)
                .WithMany((Repair x) => x.RepairGalleries)
                .HasForeignKey<int>((RepairGallery x) => x.RepairId)
                .WillCascadeOnDelete(true);
        }
    }
}