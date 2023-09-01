using domain.ArticleAdventure.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace database.ArticleAdventure.EntityMaps
{
    public class StripeCustomerMap : EntityBaseMap<StripeCustomer>
    {
        public override void Map(EntityTypeBuilder<StripeCustomer> entity)
        {
            base.Map(entity);

            entity.Property(p => p.Name)
                .IsRequired(false)
                .HasMaxLength(250);
            entity.Property(p => p.Email)
                .IsRequired();
            entity.Property(p => p.UserId)
                .IsRequired();
        }
    }
}
