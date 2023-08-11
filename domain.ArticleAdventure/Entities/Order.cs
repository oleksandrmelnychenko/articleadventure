using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Entities
{
    public class Order : EntityBase
    {
        public long ArticleId { get; set; }
        public long UserId { get; set; }
        public int TotalAmount { get; set; }
        public string PaymentStatus { get; set; }
    }
}
 