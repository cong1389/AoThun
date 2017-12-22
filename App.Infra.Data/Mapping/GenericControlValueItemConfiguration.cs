using App.Core.Common;
using App.Domain.Entities.GenericControl;
using App.Domain.Entities.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using App.Domain.Entities.GlobalSetting;

namespace App.Infra.Data.Mapping
{
    public class GenericControlValueItemConfiguration : EntityTypeConfiguration<GenericControlValueItem>
    {
        public GenericControlValueItemConfiguration()
        {
            base.ToTable("GenericControlValueItem");

            base.HasKey<int>((GenericControlValueItem x) => x.Id).Property<int>((GenericControlValueItem x) => x.Id).HasColumnName("Id").HasColumnType("int")
                .HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity)).IsRequired();

            base.HasRequired<GenericControlValue>((GenericControlValueItem x) => x.GenericControlValue)
                           .WithMany((GenericControlValue x) => x.GenericControlValueItems)
                           .HasForeignKey<int>((GenericControlValueItem x) => x.GenericControlValueId)
                            .WillCascadeOnDelete(true); ;

        }
    }
}