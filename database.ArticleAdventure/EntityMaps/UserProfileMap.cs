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
    public sealed class UserProfileMap : EntityBaseMap<UserProfile>
    {
        public override void Map(EntityTypeBuilder<UserProfile> entity)
        {
            base.Map(entity);

            entity.ToTable("UserProfile");
            entity.Property(e => e.UserName).HasMaxLength(150);
            entity.Property(e => e.Email).HasMaxLength(150);
        }
    }
}
