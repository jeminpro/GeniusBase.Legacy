using System;
using System.Linq;
using GeniusBase.Dal.Entities;
using GeniusBase.Dal.Repository;
using GeniusBase.Web.Models;
using GeniusBase.Web.Business.Categories;

namespace GeniusBase.Web.Business.Articles
{
    public class ArticleFactory : IArticleFactory
    {        
        public IUserRepository UserRepository { get; set; }
        public ICategoryFactory CategoryFactory{ get; set; }

        public ArticleViewModel CreateArticleViewModel(Article article)
        {
            var model = new ArticleViewModel();
            if (article != null)
            {
                model.Author = UserRepository.Get(article.Author);
                model.Category = new CategoryViewModel(article.Category);
                model.Content = article.Content;
                model.Created = article.Created ?? DateTime.Now;
                model.Edited = article.Edited ?? DateTime.Now;
                model.Id = article.Id;
                model.IsDraft = article.IsDraft == 1 ? true : false;
                model.Likes = article.Likes;
                model.Title = article.Title;
                model.Tags = String.Join(",", article.ArticleTags.Select(at => at.Tag.Name).ToArray());
                model.Attachments = article.Attachments.Select(t => new AttachmentViewModel(t)).ToList();
                model.SefName = article.SefName;
            }

            return model;
        }

        public Article CreateArticleFromViewModel(ArticleViewModel articleViewModel, long userId)
        {
            var article = new Article
            {
                CategoryId = articleViewModel.Category.Id,
                IsDraft = articleViewModel.IsDraft ? 1 : 0,
                Created = DateTime.Now,
                Edited = DateTime.Now,
                Title = articleViewModel.Title,
                Content = articleViewModel.Content,
                SefName = articleViewModel.SefName,
                Author = userId
            };
            return article;            
        }

        public ArticleViewModel CreateArticleViewModelWithDefValues(Category cat)
        {
            var model = new ArticleViewModel
            {
                Category = cat != null ? CategoryFactory.CreateCategoryViewModel(cat) : new CategoryViewModel()
            };

            return model;
        }
    }
}