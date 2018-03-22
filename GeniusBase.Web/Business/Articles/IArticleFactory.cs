using GeniusBase.Dal;
using GeniusBase.Dal.Entities;
using GeniusBase.Web.Models;

namespace GeniusBase.Web.Business.Articles
{
    public interface IArticleFactory
    {
        ArticleViewModel CreateArticleViewModel(Article article);
        Article CreateArticleFromViewModel(ArticleViewModel articleViewModel, long userId);
        ArticleViewModel CreateArticleViewModelWithDefValues(Category cat);
    }
}