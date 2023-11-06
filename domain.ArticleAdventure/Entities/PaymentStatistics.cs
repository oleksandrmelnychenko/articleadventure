using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Entities
{
    public class PaymentStatistics
    {
       public Dictionary<string, double> Amount { get; set; }
       public Dictionary<string, int> Products { get; set; }
       public Dictionary<string, int> Customers { get; set; }
       public double AllAmount { get; set; }
       public int AllProducts { get; set; }
       public int AllCustomers { get; set; }
        public double MaxAmount { get; set; }
        public int MaxProducts { get; set; }

        public int MaxCustomers { get; set; }

        public PaymentStatistics()
        {
            Amount = new Dictionary<string, double>();
            Products = new Dictionary<string, int>();
            Customers = new Dictionary<string, int>();
        }
    }
}
