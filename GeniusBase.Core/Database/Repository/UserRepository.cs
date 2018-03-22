using System.Linq;

namespace GeniusBase.Dal.Repository
{
    public class UserRepository : IUserRepository
    {
        public Entities.KbUser Get(long id)
        {
            using (var db = new GeniusBaseContext())
            {
                return db.KbUsers.First(u => u.Id == id);
            }
        }
    }
}