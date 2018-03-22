using GeniusBase.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GeniusBase.Dal.Entities;
using GeniusBase.Dal.Types;

namespace GeniusBase.Web.Models.Public
{
    public class LandingPageViewModel
    {
        public List<Category> HotCategories { get; set; }
        public List<Category> FirstLevelCategories { get; set; }
        public List<Article> LatestArticles { get; set; }
        public List<Article> PopularArticles { get; set; }
        public List<TagCloudItem> PopularTags { get; set; }
        public string TotalArticleCountMessage { get; set; }
        
    }
}