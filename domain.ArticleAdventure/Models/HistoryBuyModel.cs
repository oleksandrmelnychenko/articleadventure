using domain.ArticleAdventure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Models
{
    public class HistoryBuyModel
    {
        public List<StripePayment> historyArticleBuy { get; set; }

    }
}
