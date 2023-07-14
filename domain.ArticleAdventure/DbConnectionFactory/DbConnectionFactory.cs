using domain.ArticleAdventure.DbConnectionFactory.Contracts;
using System.Data;
using Microsoft.Data.SqlClient;
using common.ArticleAdventure.Helpers;

namespace domain.ArticleAdventure.DbConnectionFactory
{
    public sealed class DbConnectionFactory : IDbConnectionFactory
    {
        public IDbConnection NewSqlConnection() =>
            new SqlConnection(ConfigurationManager.DatabaseConnectionString);
    }
}
