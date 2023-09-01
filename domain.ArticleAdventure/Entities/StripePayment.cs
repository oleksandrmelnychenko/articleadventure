using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Entities
{
    //це Order
    public class StripePayment:EntityBase
    {
        public long MainArticleId { get; set; } 
        public long SupArticleId { get; set; }
        public long UserId { get; set; }
        public string ReceiptEmail { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; } //валюта
        public string PaymentStatus { get; set; }
        public double Amount { get; set; }
    }
}
