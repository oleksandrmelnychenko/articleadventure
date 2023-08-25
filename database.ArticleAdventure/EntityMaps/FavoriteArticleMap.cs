using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using domain.ArticleAdventure.Entities;

namespace database.ArticleAdventure.EntityMaps
{
    public class FavoriteArticleMap : EntityBaseMap<FavoriteArticle>
    {
        public override void Map(EntityTypeBuilder<FavoriteArticle> entity)
        {
            base.Map(entity);
            entity.Property(p => p.MainArticleId).IsRequired();
            entity.Property(p => p.UserId).IsRequired();
            //entity.HasMany(e => e.Articles)
            //   .WithOne(e => e.MainArticle)
            //   .IsRequired(false)
            //   .HasForeignKey(e => e.MainArticleId)
            //   .IsRequired(false)
            //   .OnDelete(DeleteBehavior.Cascade)
            //   .IsRequired(false);
            //entity.HasMany(e => e.ArticleTags)
            //   .WithOne(e => e.MainArticle)
            //   .IsRequired(false)
            //   .HasForeignKey(e => e.MainArticleId)
            //   .IsRequired(false)
            //   .OnDelete(DeleteBehavior.Cascade)
            //   .IsRequired(false);

            //entity.Property(p => p.Title)
            //    .IsRequired()
            //    .HasMaxLength(250);
            //entity.Property(p => p.Description)
            //    .IsRequired();
            //entity.Property(p => p.Image)
            //   .IsRequired(false)
            //   .HasMaxLength(250);
            //entity.Property(p => p.InfromationArticle)
            //   .IsRequired(false)
            //   .HasMaxLength(250);
            //entity.Property(p => p.Price)
            //  .IsRequired()
            //  .HasMaxLength(250);
            //entity.Property(p => p.ImageUrl)
            //    .IsRequired(false)
            //    .HasMaxLength(250);
            //entity.Property(p => p.WebImageUrl)
            //    .IsRequired(false)
            //    .HasMaxLength(250);

        }
    }
}
