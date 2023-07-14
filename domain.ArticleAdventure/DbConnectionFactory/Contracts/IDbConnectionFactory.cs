using System.Data;

namespace domain.ArticleAdventure.DbConnectionFactory.Contracts
{
    public interface IDbConnectionFactory
    {
        IDbConnection NewSqlConnection();
    }
}
