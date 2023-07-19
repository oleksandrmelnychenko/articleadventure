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
    public sealed class TagMap : EntityBaseMap<Tag>
    {
        public override void Map(EntityTypeBuilder<Tag> entity)
        {
            base.Map(entity);

            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(250);
            entity.Property(p => p.Color)
                .IsRequired()
                .HasMaxLength(250);
            entity.Property(p => p.IsSelected)
                .IsRequired()
                .HasDefaultValueSql("0");
        }

    }
}
