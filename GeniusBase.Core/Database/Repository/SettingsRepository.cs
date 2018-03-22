using System;
using System.Linq;
using GeniusBase.Dal.Entities;

namespace GeniusBase.Dal.Repository
{
    public class SettingsRepository : ISettingsRepository
    {
        public Settings Get()
        {
            using(var db = new GeniusBaseContext())
            {
                return db.Settings.FirstOrDefault();
            }
        }

        public void Save(Settings settings)
        {
            using (var db = new GeniusBaseContext())
            {
                var set = db.Settings.FirstOrDefault();
                if(set != null)
                {
                    db.Settings.Remove(set);
                }
                db.Settings.Add(settings);
                db.SaveChanges();
            }
        }
    }
}
