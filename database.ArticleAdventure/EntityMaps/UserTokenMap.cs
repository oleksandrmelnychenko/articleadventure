using database.ArticleAdventure.MapConfigurations;
using domain.ArticleAdventure.IdentityEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace database.ArticleAdventure.EntityMaps
{
    public sealed class UserTokenMap : EntityTypeConfiguration<UserToken>
    {
        public override void Map(EntityTypeBuilder<UserToken> entity)
        {
            entity.ToTable("UserToken");

            entity.Property(e => e.Id).HasColumnName("ID").UseIdentityColumn();

            entity.Property(e => e.UserId)
                .HasColumnName("UserID")
                .HasMaxLength(450);
        }
    }
}
