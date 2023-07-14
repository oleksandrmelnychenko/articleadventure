using domain.ArticleAdventure.DbConnectionFactory.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service.ArticleAdventure.Services
{
    public class BaseService
    {
        protected readonly IDbConnectionFactory _connectionFactory;

        public BaseService(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
    }
}
