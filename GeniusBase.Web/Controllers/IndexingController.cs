using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeniusBase.Web.Controllers
{
    [Authorize]
    public class IndexingController : GeniusBaseAdminController
    {
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Index()
        {
            return View();
        }        
    }
}