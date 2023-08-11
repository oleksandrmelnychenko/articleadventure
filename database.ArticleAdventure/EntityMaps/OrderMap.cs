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
    internal class OrderMap : EntityBaseMap<Order>
    {
        public override void Map(EntityTypeBuilder<Order> entity)
        {
            base.Map(entity);
            entity.Property(p => p.PaymentStatus)
                .IsRequired(false);
            entity.Property(p => p.ArticleId) 
                .IsRequired();
            entity.Property(p => p.UserId) 
                .IsRequired();
            entity.Property(p => p.TotalAmount) 
                .IsRequired();
        }

    }
}
