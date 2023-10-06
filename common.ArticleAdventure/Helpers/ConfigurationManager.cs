using Microsoft.Extensions.Configuration;

namespace common.ArticleAdventure.Helpers
{
    public static class ConfigurationManager
    {
        public static void SetAppSettingsProperties(IConfiguration configuration)
        {
            DatabaseConnectionString = configuration.GetConnectionString(ConnectionStringNames.DbConnectionString);
        }

        public static void SetAppEnvironmentRootPath(string path)
        {
            EnvironmentRootPath = path;
        }

        public static string EnvironmentRootPath { get; private set; }

        public static string DatabaseConnectionString { get; private set; }
    }
}
