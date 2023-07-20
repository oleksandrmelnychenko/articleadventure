using domain.ArticleAdventure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.ArticleAdventure.Repositories.Blog.Contracts
{
    public interface IBlogRepository
    {
        long AddBlog(Blogs blog);

        List<Blogs> GetAllBlogs();

    }
}
