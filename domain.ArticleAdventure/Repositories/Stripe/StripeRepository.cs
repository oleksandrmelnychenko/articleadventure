using domain.ArticleAdventure.Repositories.Stripe.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Repositories.Stripe
{
    public class StripeRepository:IStripeRepository
    {
        private readonly IDbConnection _connection;
        public StripeRepository(IDbConnection connection)
        {
            _connection = connection;
        }
    }
}
