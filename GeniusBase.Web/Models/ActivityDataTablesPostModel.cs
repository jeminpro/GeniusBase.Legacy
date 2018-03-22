using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeniusBase.Web.Models
{
    public class ActivityDataTablesPostModel
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
    }
}