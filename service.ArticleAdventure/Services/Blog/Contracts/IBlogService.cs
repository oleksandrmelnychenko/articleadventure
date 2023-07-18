using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using domain.ArticleAdventure.Entities;

namespace service.ArticleAdventure.Services.Blog.Contracts
{
    public interface IBlogService
    {
        Task<long> GetTestBlog();
    }
}
