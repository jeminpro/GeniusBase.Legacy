using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeniusBase.Dal.Entities;

namespace GeniusBase.Web.Business.ApplicationSettings
{
    public interface ISettingsService
    {
        Settings GetSettings();
        void ReloadSettings();
    }
}
