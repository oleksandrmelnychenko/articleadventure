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
    public class TagMainMap : EntityBaseMap<MainTag>
    {
        public override void Map(EntityTypeBuilder<MainTag> entity)
        {
            base.Map(entity);
            entity.HasMany(e => e.SubTags)
               .WithOne(e=>e.MainTag)
               .HasForeignKey(e => e.IdMainTag)
               .OnDelete(DeleteBehavior.Cascade);
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
