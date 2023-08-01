using domain.ArticleAdventure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace service.ArticleAdventure.MailSenderServices.Contracts
{
    public interface IMailSenderService
    {
        void SendTokenToEmail(string email, string tokenData, string baseUrl,string userId);
    }
}
