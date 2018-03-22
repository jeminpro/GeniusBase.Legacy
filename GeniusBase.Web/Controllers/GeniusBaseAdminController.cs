using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using GeniusBase.Dal;
using GeniusBase.Dal.Entities;
using NLog;

namespace GeniusBase.Web.Controllers
{
    [Authorize]
    public class GeniusBaseAdminController : Controller
    {
        protected Logger Log = LogManager.GetCurrentClassLogger();
        private const string ExceptionObjectKey = "TEMPDATA_EXCEPTION_KEY";
        private string OperationMessageKey = "GeniusBase_OPERATION_MSG_KEY";
        private string ErrorMessageKey = "GeniusBase_ERROR_MSG_KEY";

        protected Settings Settings;

        public GeniusBaseAdminController()
        {
            using (var db = new GeniusBaseContext())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                Settings = db.Settings.FirstOrDefault(s => true);
            }
        }

        protected void AddGlobalException( Exception ex)
        {
            TempData[ExceptionObjectKey] = ex;
        }

        protected Exception GetGlobalException()
        {
            if (TempData[ExceptionObjectKey] != null)
            {
                return (TempData[ExceptionObjectKey] as Exception);
            }
            return null;
        }
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewBag.ShowOperationMessage = false;
            if (ControllerContext.HttpContext.Session[OperationMessageKey] != null)
            {
                ViewBag.ShowOperationMessage = true;
                ViewBag.OperationMessage = ControllerContext.HttpContext.Session[OperationMessageKey].ToString();
                ControllerContext.HttpContext.Session.Remove(OperationMessageKey);
            }
            if (ControllerContext.HttpContext.Session[ErrorMessageKey] != null)
            {
                ViewBag.ShowErrorMessage = true;
                ViewBag.ErrorMessage = ControllerContext.HttpContext.Session[ErrorMessageKey].ToString();
                ControllerContext.HttpContext.Session.Remove(ErrorMessageKey);
            }
            ViewBag.ThreadShortDateFormat = Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern;
            base.OnActionExecuted(filterContext);
        }

        protected void ShowOperationMessage(string msg)
        {
            ControllerContext.HttpContext.Session[OperationMessageKey] += msg;            
        }

        protected void ShowErrorMessage(string msg)
        {
            ControllerContext.HttpContext.Session[ErrorMessageKey] += msg;
        }
        

    }
}
