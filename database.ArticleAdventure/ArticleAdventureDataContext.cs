using domain.ArticleAdventure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace database.ArticleAdventure
{
    public class ArticleAdventureDataContext:DbContext
    {
        public ArticleAdventureDataContext(DbContextOptions<ArticleAdventureDataContext> options)
             : base(options) { }

        DbSet<Blogs> Blogs { get; set; }
    }
}
