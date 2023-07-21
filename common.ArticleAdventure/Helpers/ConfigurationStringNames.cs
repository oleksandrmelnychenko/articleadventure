using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.ArticleAdventure.Helpers
{
    public static class ConnectionStringNames
    {
        public const string DbConnectionString = "DbConnectionString";
        public static string ConnectionString { get; private set; } = "https://localhost:7261/";
    }
}
