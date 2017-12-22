using App.Core.Common;
using App.Domain.Entities.Brandes;
using App.Domain.Entities.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;

namespace App.Infra.Data.Mapping
{
    public class RepairConfiguration : EntityTypeConfiguration<Repair>
    {
        public RepairConfiguration()
        {
            base.ToTable("Repair");
            base.HasKey<int>((Repair x) => x.Id).Property<int>((Repair x) => x.Id).HasColumnName("Id").HasColumnType("int").HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity)).IsRequired();

            HasRequired((Repair x) => x.Brand)
                .WithMany((Brand x) => x.Orders)
                .HasForeignKey((Repair x) => x.BrandId)
                .WillCascadeOnDelete(true);
        }
    }
}