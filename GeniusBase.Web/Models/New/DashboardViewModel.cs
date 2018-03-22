using System.Collections.Generic;

namespace GeniusBase.Web.Models.New
{
    public class DashboardViewModel
    {
        public DashboardViewModel()
        {
            Categories = new List<CategoryViewModel>();
            Tags = new List<TagViewModel>();
        }

        public List<CategoryViewModel> Categories { get; set; }
        public List<TagViewModel> Tags { get; set; }
    }
}