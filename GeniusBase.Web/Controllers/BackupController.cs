﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GeniusBase.Backup;
using GeniusBase.Backup.Classes;
using GeniusBase.Backup.Interface;
using GeniusBase.Web.Models;
using Resources;

namespace GeniusBase.Web.Controllers
{
    public class BackupController : GeniusBaseAdminController
    {        

        public ActionResult Index()
        {
            string BackupDirectory = "";
            if (!string.IsNullOrEmpty(Settings.BackupPath) && Settings.BackupPath.StartsWith("~"))
            {
                BackupDirectory = Server.MapPath(Settings.BackupPath);
            }
            else
            {
                BackupDirectory = Settings.BackupPath;
            }
            List<BackupListViewModel> model = new List<BackupListViewModel>();            
            int i = 0;
            if (!string.IsNullOrEmpty(BackupDirectory))
            {
                foreach (var filePath in Directory.GetFiles(BackupDirectory, "*.bak"))
                {
                    FileInfo fo = new FileInfo(filePath);
                    model.Add(new BackupListViewModel()
                    {
                        Id = i,
                        FileName = Path.GetFileName(filePath),
                        FileDate = fo.CreationTime
                    });
                    i++;
                }
            }
            return View(model.OrderByDescending( f => f.FileDate));
        }


        [HttpPost]
        public JsonResult Restore(string file)
        {
            try
            {
                JsonOperationResponse result = new JsonOperationResponse();
                try
                {
                    string backupFile = "";
                    if (Settings.BackupPath.StartsWith("~"))
                    {
                        backupFile = Server.MapPath(Settings.BackupPath + file);
                    }
                    else
                    {
                        backupFile = Settings.BackupPath + file;
                    }
                    if (System.IO.File.Exists(backupFile))
                    {
                        string connectionString = ConfigurationManager.ConnectionStrings["GeniusBaseEntities"].ConnectionString;
                        EntityConnectionStringBuilder entityConnectionString = new EntityConnectionStringBuilder(connectionString);
                        System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(entityConnectionString.ProviderConnectionString);

                        IVaultBackup backup = new VaultMsSqlBackup();
                        backup.Connect(entityConnectionString.ProviderConnectionString);
                        result.Successful = backup.Restore(builder.InitialCatalog, backupFile);
                        if (result.Successful)
                            result.ErrorMessage = UIResources.BackupRestoreSuccessfull;
                    }
                    else
                    {
                        throw new FileNotFoundException(file);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                    result.ErrorMessage = ex.Message;
                }
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }

        [HttpPost]
        public JsonResult BackupNow()
        {
            try
            {
                JsonOperationResponse result = new JsonOperationResponse();
                if (string.IsNullOrEmpty(Settings.BackupPath))
                {
                    result.Successful = false;
                    result.ErrorMessage = ErrorMessages.BackupPathIsNotSet;
                }
                else
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["GeniusBaseContext"].ConnectionString;
                    System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(connectionString);
                    IVaultBackup backup = new VaultMsSqlBackup();
                    backup.Connect(connectionString);
                    string backupFile = string.Format("{0:yyyyMddhhmm}.bak", DateTime.Now);
                    if (!string.IsNullOrEmpty(Settings.BackupPath) && Settings.BackupPath.StartsWith("~"))
                    {
                        backupFile = Server.MapPath(Settings.BackupPath + backupFile);
                    }
                    else
                    {
                        backupFile = Settings.BackupPath + backupFile;
                    }
                    bool b = backup.Backup(builder.InitialCatalog, backupFile);
                    if (b)
                    {
                        if (!String.IsNullOrEmpty(backupFile))
                        {
                            result.Data = backupFile;
                            result.Successful = true;
                        }
                    }
                }
                
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }

    }
}
