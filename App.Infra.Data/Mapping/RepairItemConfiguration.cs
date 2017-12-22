using App.Core.Common;
using App.Domain.Entities.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;

namespace App.Infra.Data.Mapping
{
    public class RepairItemConfiguration : EntityTypeConfiguration<RepairItem>
    {
        public RepairItemConfiguration()
        {
            base.ToTable("RepairItem");
            base.HasKey<int>((RepairItem x) => x.Id).Property<int>((RepairItem x) => x.Id).HasColumnName("Id").HasColumnType("int").HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity)).IsRequired();

            base.HasRequired<Repair>((RepairItem x) => x.Repair)
                .WithMany((Repair x) => x.RepairItems)
                .HasForeignKey<int>((RepairItem x) => x.RepairId)
                .WillCascadeOnDelete(true);
        }
    }
}