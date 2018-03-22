using GeniusBase.Dal;
using GeniusBase.Web.Models.New;
using System.Linq;
using System.Web.Mvc;

namespace GeniusBase.Web.Controllers
{
    public class ArticleController : Controller
    {
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
    }
}