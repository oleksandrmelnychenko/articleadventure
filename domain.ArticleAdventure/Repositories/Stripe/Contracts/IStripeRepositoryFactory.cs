using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Repositories.Stripe.Contracts
{
    public interface IStripeRepositoryFactory
    {
        IStripeRepository New(IDbConnection connection);
    }
}
