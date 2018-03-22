using GeniusBase.Dal;
using GeniusBase.Dal.Entities;
using GeniusBase.Web.Models;

namespace GeniusBase.Web.Business.Categories
{
    public class CategoryFactory : ICategoryFactory
    {
        public Category CreateCategory(string name, bool isHot, string sefName, string icon, long author, int? parent)
        {
            return new Category
            {
                Name = name,
                Author = author,
                IsHot = isHot,
                SefName = sefName,
                Icon =  icon,
                Parent = parent
            };
        }

        public CategoryViewModel CreateCategoryViewModel(Category cat)
        {
            CategoryViewModel categoryModel = new CategoryViewModel
            {
                Id = cat.Id,
                IsHot = cat.IsHot,
                ParentId = cat.Parent.HasValue ? (int) cat.Parent : -1,
                Name = cat.Name,
                Icon = cat.Icon,
                SefName = cat.SefName
            };
            return categoryModel;
        }
    }
}