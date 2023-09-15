using service.ArticleAdventure.MailSenderServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using domain.ArticleAdventure.Models;
using System.Web;
using Azure.Core;

namespace service.ArticleAdventure.MailSenderServices
{
    public class MailSenderService : IMailSenderService
    {
        private readonly string _senderUserName;

        private readonly string _senderPassword;

        public MailSenderService(string senderUserName, string senderPassword)
        {
            _senderUserName = senderUserName;
            _senderPassword = senderPassword;
        }

        public void SendTokenToEmail(string email, string AccessToken, string baseUrl, string userId)
        {
            try
            {
                using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.EnableSsl = true;

                    client.DeliveryMethod = SmtpDeliveryMethod.Network;

                    client.UseDefaultCredentials = false;

                    client.Credentials = new NetworkCredential(_senderUserName, _senderPassword);

                    MailMessage mssObj = new MailMessage();

                    mssObj.To.Add(email);

                    mssObj.From = new MailAddress(_senderUserName);

                    mssObj.Subject = "Fize sign up confirm";
                    string codeHtmlVersion = HttpUtility.UrlEncode(AccessToken);
                    var confirmUrl = baseUrl + "EmailConfirmation?access_token=" + codeHtmlVersion + "&userId="+ userId;

                    var confirmButton = "<a href=\"" + confirmUrl + "\">link</a>";

                    var firstParagraph = "<p>" + confirmButton + " </p>";

                    var bodyMessage = "<div>" + firstParagraph + "</div>";

                    mssObj.Body = bodyMessage;

                    mssObj.IsBodyHtml = true;

                    client.Send(mssObj);
                }
            }
            catch
            {
                throw new System.Exception("MailServiceError");
            }
        }
    }
}
