using GeniusBase.Dal;
using GeniusBase.Dal.Repository;
using GeniusBase.Web.Business.Articles;
using GeniusBase.Web.Models.New;
using NLog;
using NLog.Fluent;
using System;
using System.Linq;
using System.Web.Mvc;

namespace GeniusBase.Web.Controllers
{
    public class ArticleController : Controller
    {
        public IArticleRepository ArticleRepository { get; set; }
        public IArticleFactory ArticleFactory { get; set; }
        protected Logger Log = LogManager.GetCurrentClassLogger();

        public ActionResult Show(string id)
        {
            using (var db = new GeniusBaseContext())
            {
                var article = db.PublishedArticles().FirstOrDefault(a => a.SefName == id);

                if (article == null)
                {
                    return View(new ArticleViewModel());
                }

                article.Views++;
                db.SaveChanges();

                var articleViewModel = new ArticleViewModel
                {
                    ArticleId = article.Id,
                    Title = article.Title,
                    Content = article.Content,
                    CategorySef = article.Category.SefName,
                    CategoryName = article.Category.Name,
                    Tags = article.ArticleTags.Select(a => a.Tag.Name).ToList()
                };

                return View(articleViewModel);                
            }
        }


        public ActionResult Edit(int id)
        {
            try
            {
                return View("Create", ArticleFactory.CreateArticleViewModel(ArticleRepository.Get(id)));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("Index", "Error");
            }
        }
    }
}