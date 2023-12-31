﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Entities
{
    //це Order
    public class StripePayment:EntityBase
    {
        public long MainArticleId { get; set; }
        [ForeignKey("MainArticleId")]
        public MainArticle MainArticle { get; set; }
        public long SupArticleId { get; set; }
        [ForeignKey("SupArticleId")]
        public AuthorArticle SupArticle { get; set; }
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public UserProfile UserProfile { get; set; }
        public string ReceiptEmail { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; } //валюта
        public string PaymentStatus { get; set; }
        public double Amount { get; set; }
        public StripePayment()
        {
            MainArticle = new MainArticle();
            SupArticle = new AuthorArticle();
        }
    }
}
