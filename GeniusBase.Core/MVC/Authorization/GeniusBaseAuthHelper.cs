using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeniusBase.Dal;
using GeniusBase.Core.Exceptions;
using GeniusBase.Core.Outlib;
using GeniusBase.Dal.Entities;
using NLog;

namespace GeniusBase.Core.MVC.Authorization
{
    public class GeniusBaseAuthHelper
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();
        private static string HashAlgoritm = "SHA1";

        public static string ROLE_ADMIN = "Admin";
        public static string ROLE_MANAGER = "Manager";
        public static string ROLE_EDITOR = "Editor";

        public static KbUser GetKbUser(string userName)
        {
            using (var db = new GeniusBaseContext())
            {
                return db.KbUsers.FirstOrDefault<KbUser>(ku => ku.UserName == userName);
            }

        }

        public static KbUser CreateUser(string username, string password, string email,string role, long author)
        {
            try
            {
                using (var db = new GeniusBaseContext())
                {
                    KbUser usr = new KbUser();
                    usr.Password = HashPassword(password, Guid.NewGuid().ToString().Replace("-", ""));
                    usr.UserName = username;
                    usr.Email = email;
                    usr.Role = role;
                    usr.Author = author;
                    db.KbUsers.Add(usr);
                    db.SaveChanges();
                    return usr;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }

        public static string HashPassword(string password, string salt)
        {
            try
            {
                return ObviexSimpleHash.ComputeHash(password, HashAlgoritm, Encoding.Default.GetBytes(salt));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }
        
        private static bool VerifyHash(string password, string passwordHash) 
        { 
            return ObviexSimpleHash.VerifyHash(password, HashAlgoritm, passwordHash); 
        }

        public static bool ValidateUser(string userName, string password) 
        { 
            try 
            {
                using (var db = new GeniusBaseContext())
                {
                    KbUser usr = GetKbUser(userName);
                    if (usr == null)
                        return false;
                    return VerifyHash(password, usr.Password); 
                }
                
            } 
            catch (Exception ex) 
            { 
                Log.Error(ex); 
                throw; 
            } 
        }

        public static void ChangePassword(string username, string oldPassword, string newPassword)
        {
            try
            {
                if (ValidateUser(username, oldPassword))
                {
                    using (var db = new GeniusBaseContext())
                    {
                        KbUser usr= db.KbUsers.FirstOrDefault(ku => ku.UserName == username);
                        if (usr != null)
                        {
                            usr.Password = HashPassword(newPassword, Guid.NewGuid().ToString().Replace("-", ""));
                            db.SaveChanges();
                        }
                        else throw new UserNotFoundException();
                    }                   
                }
                else
                {
                    throw new InvalidPasswordException();
                }
                
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }
    }
}
