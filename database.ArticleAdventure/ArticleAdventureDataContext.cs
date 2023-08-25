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

        public DbSet<AuthorArticle> AuthorArticle { get; set; }
        public DbSet<SupTag> SubTags { get; set; }
        public DbSet<MainTag> MainTags { get; set; }
        public DbSet<MainArticleTags> ArticleTags { get; set; }
        public DbSet<MainArticle> MainArticle { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<FavoriteArticle> FavoriteArticles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.AddConfiguration(new AuthorArticleMap());

            builder.AddConfiguration(new TagMap());

            builder.AddConfiguration(new TagMainMap());

            builder.AddConfiguration(new OrderMap());

            builder.AddConfiguration(new PaymentMap());

            builder.AddConfiguration(new MainArticleTagsMap());
            
            builder.AddConfiguration(new MainArticleMap());

            builder.AddConfiguration(new AuthorArticleMap());

            builder.AddConfiguration(new FavoriteArticleMap());
        }
    }
}
