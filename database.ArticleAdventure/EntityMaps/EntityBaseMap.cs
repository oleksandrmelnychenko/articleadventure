using database.ArticleAdventure.MapConfigurations;
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
    public abstract class EntityBaseMap<T> : EntityTypeConfiguration<T> where T : EntityBase
    {
        public override void Map(EntityTypeBuilder<T> entity)
        {
            entity.Property(e => e.Id).HasColumnName("ID");

            entity.Property(e => e.NetUid)
                .HasColumnName("NetUID")
                .HasDefaultValueSql("newid()");

            entity.Property(e => e.Created).HasDefaultValueSql("getutcdate()");

            entity.Property(e => e.Deleted).HasDefaultValueSql("0");
        }
    }
}
