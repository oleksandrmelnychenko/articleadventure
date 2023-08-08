using domain.ArticleAdventure.Repositories.Tag.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Repositories.Tag
{
    public class TagRepositoryFactory : ITagRepositoryFactory
    {
        public ITagRepository New(IDbConnection connection) =>
            new TagRepository(connection);
    }
}
