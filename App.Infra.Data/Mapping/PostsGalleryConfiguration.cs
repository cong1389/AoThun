using App.Core.Common;
using App.Domain.Entities.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq.Expressions;

namespace App.Infra.Data.Mapping
{
    public class PostsGalleryConfiguration : EntityTypeConfiguration<PostGallery>
    {
        public PostsGalleryConfiguration()
        {
            base.ToTable("PostGallery");
            base.HasKey<int>((PostGallery x) => x.Id)
                .Property<int>((PostGallery x) => x.Id).HasColumnName("Id")
                .HasColumnType("int").HasDatabaseGeneratedOption(new DatabaseGeneratedOption?(DatabaseGeneratedOption.Identity))
                .IsRequired();
            //base.HasRequired<Post>((PostGallery x) => x.Post).WithMany((Post x) => x.PostGallerys).HasForeignKey<int>((PostGallery x) => x.PostId).WillCascadeOnDelete(true);
        }
    }
}