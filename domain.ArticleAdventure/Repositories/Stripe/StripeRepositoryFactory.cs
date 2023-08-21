using domain.ArticleAdventure.Repositories.Stripe.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Repositories.Stripe
{
    public class StripeRepositoryFactory : IStripeRepositoryFactory
    {
        public IStripeRepository New(IDbConnection connection)
            => new StripeRepository(connection);
    }
}
