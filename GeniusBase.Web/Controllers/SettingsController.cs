using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeniusBase.Web.Models;
using GeniusBase.Dal;
using GeniusBase.Dal.Entities;
using Resources;
using GeniusBase.Web.Helpers;
using System.Reflection;
using GeniusBase.Web.Business.ApplicationSettings;
using GeniusBase.Dal.Repository;

namespace GeniusBase.Web.Controllers
{
    [Authorize]
    public class SettingsController : GeniusBaseAdminController
    {
        public ISettingsFactory SettingsFactory { get; set; }
        public ISettingsService SettingsService { get; set; }
        public ISettingsRepository SettingsRepository { get; set; }
        //private Logger Log = LogManager.GetCurrentClassLogger();
        //
        // GET: /Settings/
        [HttpPost]
        public ActionResult Index(SettingsViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {                    
                    var set = SettingsFactory.CreateModel(model);
                    if (set != null)
                    {
                        SettingsRepository.Save(set);                        
                        ConfigurationManager.AppSettings["Theme"] = model.SelectedTheme;
                        SettingsService.ReloadSettings();
                        ShowOperationMessage(UIResources.SettingsPageSaveSuccessfull);
                    }                    
                }
                model.Themes.AddRange(Directory.EnumerateDirectories(Server.MapPath("~/Views/Themes")).Select(e => Path.GetFileName(e)).ToList());
                return View(model);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                ShowErrorMessage(ex.Message);
                return RedirectToAction("Index", "Error");
            }
        }

        public ActionResult Index()
        {
            ViewBag.UpdateSuccessfull = false;
            var model = SettingsFactory.CreateViewModel(SettingsService.GetSettings());
            return View(model);             
        }

    }
}
