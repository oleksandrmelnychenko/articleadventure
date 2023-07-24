using database.ArticleAdventure.EntityMaps;
using database.ArticleAdventure.MapConfigurations;
using domain.ArticleAdventure.Entities;
using domain.ArticleAdventure.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace database.ArticleAdventure
{
    public sealed class BusinessContext : IdentityDbContext<User>
    {
        public BusinessContext(DbContextOptions<BusinessContext> options)
                  : base(options) { }

        public DbSet<UserToken> UserToken { get; set; }

        public DbSet<UserProfile> UserProfile { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.AddConfiguration(new UserTokenMap());

            builder.AddConfiguration(new UserProfileMap());
        }
    }
}
