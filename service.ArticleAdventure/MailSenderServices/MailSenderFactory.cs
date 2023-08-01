using service.ArticleAdventure.MailSenderServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service.ArticleAdventure.MailSenderServices
{
    public class MailSenderFactory : IMailSenderFactory
    {
        public IMailSenderService NewMailSenderService()
        {
            return new MailSenderService(
                "natusvincer77@gmail.com",
                "okkkmiqgurdpqqnw");
        }
    }

}
