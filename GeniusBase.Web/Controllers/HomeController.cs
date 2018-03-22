using GeniusBase.Dal;
using GeniusBase.Dal.Entities;
using GeniusBase.Web.Models.New;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeniusBase.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: NewHome
        public ActionResult Index()
        {
            var dashboardViewModel = new DashboardViewModel()
            {
                Categories = GetCategories(),
                Tags = GetTags()
            };
            return View(dashboardViewModel);
        }

        private static List<CategoryViewModel> GetCategories()
        {
            var CategoryList = new List<CategoryViewModel>();
            using (var db = new GeniusBaseContext())
            {
                var categories = db.Categories.ToList();
                foreach (var cat in categories)
                {
                    var categoryItem = new CategoryViewModel
                    {
                        CategoryName = cat.Name,
                        CategorySefName = cat.SefName,
                        CategoryArticleCount = new Random().Next(cat.Id, cat.Id*10)    //todo
                    };
                    CategoryList.Add(categoryItem);                    
                }
            }
            return CategoryList;
        }

        private static List<TagViewModel> GetTags()
        {
            var TagList = new List<TagViewModel>();
            using (var db = new GeniusBaseContext())
            {
                var tags = db.Tags.ToList();
                foreach (var tag in tags)
                {
                    var categoryItem = new TagViewModel
                    {
                        TagName = tag.Name,
                        TagArticleCount = new Random().Next(tag.Id, tag.Id*10)    //todo
                    };
                    TagList.Add(categoryItem);
                }
            }
            return TagList;
        }
    }
}