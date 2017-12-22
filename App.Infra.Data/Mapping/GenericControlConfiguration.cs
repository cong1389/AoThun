using App.Core.Common;
using App.Domain.Entities.Brandes;
using App.Domain.Entities.Data;
using App.Domain.Entities.GenericControl;
using App.Domain.Entities.GlobalSetting;
using App.Domain.Entities.Menu;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;

namespace App.Infra.Data.Mapping
{
    public class GenericControlConfiguration : EntityTypeConfiguration<GenericControl>
    {
        public GenericControlConfiguration()
        {
            base.ToTable("GenericControl");

            base.HasKey<int>((GenericControl x) => x.Id)
                .Property<int>((GenericControl x) => x.Id)
                .HasColumnName("Id").HasColumnType("int")
                .HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity)).IsRequired();
            
            base.HasMany<MenuLink>((GenericControl x) => x.MenuLinks)
                 .WithMany((MenuLink x) => x.GenericControls)
                 .Map((ManyToManyAssociationMappingConfiguration x) =>
                 {
                     x.ToTable("GenericControlMenuLink");
                     x.MapLeftKey(new string[] { "GenericControlId" });
                     x.MapRightKey(new string[] { "MenuLinkId" });
                 });
        }
    }
}