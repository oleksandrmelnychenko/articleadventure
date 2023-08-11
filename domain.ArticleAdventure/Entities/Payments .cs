using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Entities
{
    public class Payment:EntityBase
    {
        public long OrderId { get; set; }
        public int PaymentAmount { get; set; }
        public string PaymentStatus { get; set; }
    }
}
