using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeniusBase.Backup.Interface
{    
    
    public interface IVaultBackup
    {
        void Connect(string connectionString);
        bool Backup(string databaseName, string physicalPath);
        bool Restore(string databaseName, string physicalPath);
    }
}
