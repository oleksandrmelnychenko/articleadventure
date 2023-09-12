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
            entity.Property(p => p.UserId)
               .IsRequired();
            entity.Property(p => p.MainArticleId)
               .IsRequired();
           
        }
    }
}
