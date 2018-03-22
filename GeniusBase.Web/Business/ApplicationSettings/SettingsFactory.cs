using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using GeniusBase.Dal.Entities;
using GeniusBase.Web.Helpers;
using GeniusBase.Web.Models;

namespace GeniusBase.Web.Business.ApplicationSettings
{
    public class SettingsFactory : ISettingsFactory
    {
        public Settings CreateModel(SettingsViewModel settings)
        {
            var set = new Settings
            {
                CompanyName = settings.CompanyName,
                ArticleCountPerCategoryOnHomePage = settings.ArticleCountPerCategoryOnHomePage,
                DisqusShortName = settings.DisqusShortName,
                JumbotronText = settings.JumbotronText,
                ShareThisPublicKey = settings.ShareThisPublicKey,
                TagLine = settings.TagLine,
                IndexFileExtensions = settings.IndexFileExtensions,
                ArticlePrefix = settings.ArticlePrefix,
                AnalyticsAccount = settings.AnalyticsAccount,
                Author = HelperFunctions.UserAsKbUser(HttpContext.Current.User).Id,
                BackupPath = settings.BackupPath,
                ShowTotalArticleCountOnFrontPage = settings.ShowTotalArticleCountOnFrontPage
            };

            if (!string.IsNullOrEmpty(set.BackupPath))
            {
                if (!set.BackupPath.EndsWith("\\") && !set.BackupPath.StartsWith("~"))
                    set.BackupPath += "\\";
                if (!set.BackupPath.EndsWith("/") && set.BackupPath.StartsWith("~"))
                    set.BackupPath += "/";
            }            

            return set;
        }

        public SettingsViewModel CreateViewModel(Settings settings)
        {

            var model = new SettingsViewModel(settings)
            {
                SelectedTheme = ConfigurationManager.AppSettings["Theme"]
            };

            var a = typeof(SettingsFactory).Assembly;
            model.ApplicationVersion = a.GetName().Version.Major + "." + a.GetName().Version.Minor;
            model.Themes.AddRange(Directory.EnumerateDirectories( HttpContext.Current.Server.MapPath("~/Views/Themes")).Select(e => Path.GetFileName(e)).ToList());

            return model;
        }
    }
}
