using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GeniusBase.Dal.Entities;

namespace GeniusBase.Dal.Repository
{
    public interface ICategoryRepository
    {
        int Add(Category category);
        void Update(Category category);
        Category Get(int categoryId);        
        bool Remove(Category category);

        Category GetFirstCategory();
        IList<Category> GetAllCategories();
        IList<Category> GetHotCategories();
        IList<Category> GetFirstLevelCategories();
        bool HasArticleInCategory(int categoryId);
        IList<Article> GetArticles(int categoryId);
    }
}