using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GeniusBase.Dal.Entities;
using GeniusBase.Web.Models;

namespace GeniusBase.Web.Business.ApplicationSettings
{
    public interface ISettingsFactory
    {
        SettingsViewModel CreateViewModel(Settings settings);
        Settings CreateModel(SettingsViewModel settings);
    }
}
