using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeniusBase.Core.MVC.Authorization;
using GeniusBase.Web.Models.Public;
using GeniusBase.Dal;
using GeniusBase.Dal.Entities;
using GeniusBase.Dal.Repository;
using GeniusBase.Web.Models;
using MvcPaging;
using GeniusBase.Web.Helpers;
using Resources;

namespace GeniusBase.Web.Controllers
{
    public class OldHomeController : GeniusBasePublicController
    {
        private int ArticleCountPerPage = 20;

        public ITagRepository TagRepository { get; set; }
        public IArticleRepository ArticleRepository { get; set; }
        public ICategoryRepository CategoryRepository { get; set; }


        [HttpPost]
        public JsonResult Like(int articleId)
        {
            JsonOperationResponse result = new JsonOperationResponse();
            if (Request.IsAjaxRequest() )
            {
                using (var db = new GeniusBaseContext())
                {                    
                    var article = db.Articles.FirstOrDefault(a => a.Id == articleId);
                    if (article == null)
                    {
                        result.ErrorMessage = ErrorMessages.ArticleNotFound;
                    }
                    else
                    {
                        article.Likes++;
                        db.SaveChanges();
                        result.Successful = true;
                        result.ErrorMessage = UIResources.ArticleLikeSuccess;
                    }
                }
            }
            return Json(result);
        }

       

        public ActionResult Tags(string id, int page = 1)
        {
            try
            {                
                using (var db = new GeniusBaseContext())
                {
                    Tag tag = db.Tags.First(c => c.Name == id);
                    if (tag == null)
                    {
                        return View("TagNotFound");
                    }
                    ViewBag.Tag = tag;
                    IList<Article> articles = db.PublishedArticles().Where(a => a.ArticleTags.Any(t => t.Tag.Name == id) ).OrderBy(a => a.Title).ToPagedList(page, ArticleCountPerPage);
                    return View(articles);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("PublicError", "Error");
            }
        }

        public ActionResult Categories(string id, int page=1)
        {
            try
            {
                using (var db = new GeniusBaseContext())
                {
                    Category cat = db.Categories.Include("ChildCategories").Include("ParentCategory").First(c => c.SefName == id);
                    if (cat == null)
                    {
                        return View("CategoryNotFound");
                    }
                    ViewBag.Category = cat;                    
                    IList<Article> articles = db.PublishedArticles().Where(a => a.Category.SefName == id).OrderBy(a => a.Title).ToPagedList(page, ArticleCountPerPage);
                    return View(articles);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("PublicError", "Error");
            }
        }

        public ActionResult Detail(string id)
        {
            try
            {
                using (var db = new GeniusBaseContext())
                {                                        
                    Article article = db.PublishedArticles().FirstOrDefault(a => a.SefName == id);                                  
                    if (article != null)
                    {
                        article.Views++;                        
                        db.SaveChanges();                        
                        ViewBag.SimilarArticles = ArticleRepository.GetVisibleSimilarArticles((int)article.Id, DateTime.Today.Date);
                        return View(article);
                    }
                    else
                    {
                        return View("ArticleNotFound");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("PublicError", "Error");
            }
        }

        public ActionResult Index()
        {
            var settings = SettingsService.GetSettings();            
            var model = new LandingPageViewModel();
            if (settings.ShowTotalArticleCountOnFrontPage)
            {
                model.TotalArticleCountMessage = string.Format(UIResources.PublicTotalArticleCountMessage,ArticleRepository.GetTotalArticleCount());
            }
            model.HotCategories = CategoryRepository.GetHotCategories().ToList();
            var dateRangeToday = DateTime.Now.Date;
            ViewBag.Title = settings.CompanyName;
            model.FirstLevelCategories = CategoryRepository.GetFirstLevelCategories().ToList();
            model.LatestArticles = ArticleRepository.GetLatestArticles(settings.ArticleCountPerCategoryOnHomePage);
            model.PopularArticles = ArticleRepository.GetPopularArticles(settings.ArticleCountPerCategoryOnHomePage);
            model.PopularTags = TagRepository.GetTagCloud().Select(tag => new TagCloudItem(tag)).ToList();
            return View(model);            
            
        }

    }
}
