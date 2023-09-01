using domain.ArticleAdventure.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace database.ArticleAdventure.EntityMaps
{
    public class StripePaymentMap : EntityBaseMap<StripePayment>
    {
        public override void Map(EntityTypeBuilder<StripePayment> entity)
        {
            base.Map(entity);

            entity.Property(p => p.PaymentStatus)
                .IsRequired(false)
                .HasMaxLength(250);
            entity.Property(p => p.Amount)
                .IsRequired();
            entity.Property(p => p.Currency)
                .IsRequired();
            entity.Property(p => p.Description)
               .IsRequired();
            entity.Property(p => p.SupArticleId)
               .IsRequired();
            entity.Property(p => p.MainArticleId)
               .IsRequired();
            entity.Property(p => p.UserId)
               .IsRequired();
            entity.Property(p => p.ReceiptEmail)
              .IsRequired();

        }

    }
}
