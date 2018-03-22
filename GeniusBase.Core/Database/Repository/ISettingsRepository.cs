using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GeniusBase.Dal.Entities;

namespace GeniusBase.Dal.Repository
{
    public interface ISettingsRepository
    {
        Settings Get();
        void Save(Settings settings);
    }
}
