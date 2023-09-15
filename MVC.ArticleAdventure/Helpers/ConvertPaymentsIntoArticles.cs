using domain.ArticleAdventure.Entities;
using System.Collections.Generic;

namespace MVC.ArticleAdventure.Helpers
{
    public static class ConvertPaymentsIntoArticles
    {

        public static List<MainArticle> Convert(List<StripePayment> stripePayments)
        {
            List<MainArticle> mainArticles = new List<MainArticle>(); 
            foreach (var stripePayment in stripePayments)
            {
                if (mainArticles.Any(x => x.Id.Equals(stripePayment.MainArticleId)))
                {
                    mainArticles.Where(x => x.Id == stripePayment.MainArticleId).ToList().ForEach(x => x.Articles.Add(stripePayment.SupArticle));
                }
                else
                {
                    mainArticles.Add(stripePayment.MainArticle);
                    mainArticles.Where(x => x.Id == stripePayment.MainArticleId).ToList().ForEach(x => x.Articles.Add(stripePayment.SupArticle));
                }
            }
            return mainArticles;
        }
    }
}
