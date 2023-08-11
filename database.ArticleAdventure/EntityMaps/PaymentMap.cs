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
    public class PaymentMap : EntityBaseMap<Payment>
    {
        public override void Map(EntityTypeBuilder<Payment> entity)
        {
            base.Map(entity);

            entity.Property(p => p.PaymentStatus)
                .IsRequired(false)
                .HasMaxLength(250);
            entity.Property(p => p.PaymentAmount)
                .IsRequired();
            entity.Property(p => p.OrderId)
                .IsRequired();
        }

    }
}
