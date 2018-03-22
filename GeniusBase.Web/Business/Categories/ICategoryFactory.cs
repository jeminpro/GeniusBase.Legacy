using GeniusBase.Dal;
using GeniusBase.Dal.Entities;
using GeniusBase.Web.Models;

namespace GeniusBase.Web.Business.Categories
{
    public interface ICategoryFactory
    {
        Category CreateCategory(string name, bool isHot, string sefName, string icon, long author, int? parent);
        CategoryViewModel CreateCategoryViewModel(Category cat);
    }
}