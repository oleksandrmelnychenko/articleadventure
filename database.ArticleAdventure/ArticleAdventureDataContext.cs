using database.ArticleAdventure.EntityMaps;
using database.ArticleAdventure.MapConfigurations;
using domain.ArticleAdventure.Entities;
using Microsoft.EntityFrameworkCore;

namespace database.ArticleAdventure
{
    public class ArticleAdventureDataContext:DbContext
    {
        public ArticleAdventureDataContext(DbContextOptions<ArticleAdventureDataContext> options)
             : base(options) { }

        public DbSet<AuthorArticle> Blogs { get; set; }

        public DbSet<SupTag> SubTags { get; set; }

        public DbSet<MainTag> MainTags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.AddConfiguration(new BlogMap());

            builder.AddConfiguration(new TagMap());

            builder.AddConfiguration(new TagMainMap());
        }
    }
}
