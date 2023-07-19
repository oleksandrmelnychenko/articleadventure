using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace database.ArticleAdventure.MapConfigurations.Contracts
{
    public interface IEntityMappingConfiguration
    {
        void Map(ModelBuilder b);
    }
}
