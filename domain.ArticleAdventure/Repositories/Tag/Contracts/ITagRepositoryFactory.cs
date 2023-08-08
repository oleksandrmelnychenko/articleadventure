using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Repositories.Tag.Contracts
{
    public interface ITagRepositoryFactory
    {
        ITagRepository New(IDbConnection connection);
    }
}
