using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeniusBase.Web.Models.Public
{
    public class SearchFormViewModel
    {
        
        public int ArticleId { get; set; }
        public string SearchKeyword { get; set; }
        public int CurrentPage { get; set; }
        public List<KbSearchResultItemViewModel> Results { get; set; }
    }
}