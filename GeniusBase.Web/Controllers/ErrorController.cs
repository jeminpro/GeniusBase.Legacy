using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeniusBase.Web.Controllers
{
    public class ErrorController : GeniusBaseAdminController
    {
        //
        // GET: /Error/

        public ActionResult Index()
        {
            ViewBag.ErrorMessage = "...";
            Exception ex = GetGlobalException();
            if( ex != null )
            {                
                ViewBag.ErrorMessage = ex.Message;
            }            
            return View();
        }

        public ActionResult PublicError()
        {
            return View();
        }

    }
}
