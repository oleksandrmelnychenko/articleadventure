using domain.ArticleAdventure.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace database.ArticleAdventure.EntityMaps
{
    public sealed class BlogMap : EntityBaseMap<Blogs>
    {
        public override void Map(EntityTypeBuilder<Blogs> entity)
        {
            base.Map(entity);

            entity.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(250);
            entity.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(250);
            entity.Property(p=>p.Body)
                .IsRequired()
                .HasMaxLength(250);
            entity.Property(p => p.Image)
                .IsRequired(false)
                .HasMaxLength(250);
            entity.Property(p => p.ImageUrl)
                .IsRequired(false)
                .HasMaxLength(250);
            entity.Property(p => p.WebImageUrl)
                .IsRequired(false)
                .HasMaxLength(250);
            entity.Property(p => p.EditorValue)
                .IsRequired(false)
                .HasMaxLength(250);
            entity.Property(p => p.MetaKeywords)
                .IsRequired(false)
                .HasMaxLength(250);
            entity.Property(p => p.MetaDescription)
                .IsRequired(false)
                .HasMaxLength(250);
            entity.Property(p => p.Url)
                .IsRequired(false)
                .HasMaxLength(250);
        }

    }
}
