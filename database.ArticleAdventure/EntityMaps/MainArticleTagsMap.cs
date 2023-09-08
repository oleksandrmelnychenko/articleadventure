using domain.ArticleAdventure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace database.ArticleAdventure.EntityMaps
{
    public class MainArticleTagsMap : EntityBaseMap<MainArticleTags>
    {
        public override void Map(EntityTypeBuilder<MainArticleTags> entity)
        {
            base.Map(entity);
            entity.HasOne(p => p.SupTag)
                .WithMany(p => p.MainArticleTags)
                .HasForeignKey(p => p.SupTagId)
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired(false);
        }
    }
}
