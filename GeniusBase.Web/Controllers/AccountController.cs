﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using GeniusBase.Core.MVC.Authorization;
using GeniusBase.Web.Models;

using NLog;
using GeniusBase.Dal;
using System.Collections;
using Resources;
using GeniusBase.Web.Helpers;
using KbUser = GeniusBase.Dal.Entities.KbUser;

namespace GeniusBase.Web.Controllers
{
    [Authorize]
    public class AccountController : GeniusBaseAdminController
    {
        //
        // GET: /Account/

        //private Logger Log = LogManager.GetCurrentClassLogger();

        [AllowAnonymous]
        public ActionResult Login()
        {
            LoginViewModel model = new LoginViewModel();
            return View(model);
        }

        [Authorize]
        public ActionResult Logout() 
        { 
            if (Request.IsAuthenticated) 
                FormsAuthentication.SignOut(); 
            return RedirectToAction("Index", "Home"); 
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginViewModel model )
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var kmp = new GeniusBaseMembershipProvider();
                    if (kmp.ValidateUser(model.UserName, model.Password))
                    {
                        FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                        if (String.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
                            return RedirectToAction("Index", "Dashboard");
                        else
                            return Redirect(Request.QueryString["ReturnUrl"]);
                    }
                    else
                    {
                        ModelState.AddModelError("LoginError", ErrorMessages.LoginFailed);
                    }
                    
                }
                return View(model);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                throw;
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult MyProfile(KbUserViewModel model)
        {
            try
            {
                                
                if (ModelState.IsValid)
                {
                    using (var db = new GeniusBaseContext())
                    {
                        string username = ControllerContext.RequestContext.HttpContext.User.Identity.Name;
                        KbUser usr = db.KbUsers.FirstOrDefault(u => u.UserName == username);
                        if (usr == null)
                        {
                            ModelState.AddModelError("UserNotFound", ErrorMessages.UserNotFound);
                            return View(model);
                        }
                        if (GeniusBaseAuthHelper.ValidateUser(username, model.OldPassword))
                        {
                            usr.Name = model.Name;
                            usr.LastName = model.LastName;
                            usr.Email = model.Email;
                            if (!String.IsNullOrEmpty(model.NewPassword) && model.NewPassword == model.NewPasswordAgain)
                            {
                                GeniusBaseAuthHelper.ChangePassword(model.UserName, model.OldPassword, model.NewPassword);
                            }
                            db.SaveChanges();
                            ShowOperationMessage(UIResources.UserProfileUpdateSuccessful);
                            return RedirectToAction("Index", "Dashboard");
                        }
                        else
                        {
                            ShowOperationMessage(ErrorMessages.WrongPassword);
                        }
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                ShowOperationMessage(ex.Message);
                return RedirectToAction("Index", "Error");
            }          
        }

        [Authorize]
        public ActionResult MyProfile()
        {
            try
            {
                using (var db = new GeniusBaseContext())
                {
                    string username = ControllerContext.RequestContext.HttpContext.User.Identity.Name;
                    KbUser usr = db.KbUsers.FirstOrDefault(u => u.UserName == username);
                    if (usr == null)
                        throw new ArgumentNullException(ErrorMessages.UserNotFound);
                    KbUserViewModel model = new KbUserViewModel(usr);
                    return View(model);
                }
                
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                ShowOperationMessage(ex.Message);
                return RedirectToAction("Index", "Error");
            }
        }

        public ActionResult Index()
        {
            return RedirectToAction("MyProfile");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult Remove(int id = -1 )
        {
            JsonOperationResponse result = new JsonOperationResponse()
            {
                Successful = false
            };
            try
            {
                using(var db = new GeniusBaseContext())
                {
                    db.KbUsers.Remove(db.KbUsers.First(u => u.Id == id));
                    db.SaveChanges();
                    result.Successful = true;
                }
                return Json(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                result.ErrorMessage = ex.Message;
                return Json(result);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult UserInfo(KbUserViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var db = new GeniusBaseContext())
                    {
                        KbUser usr = db.KbUsers.FirstOrDefault(u => u.Id == model.Id);
                        if (usr == null)
                        {
                            ModelState.AddModelError("UserNotFound", ErrorMessages.UserNotFound);
                            return View(model);
                        }
                        if( GeniusBaseAuthHelper.ValidateUser(model.UserName,model.OldPassword) )
                        {
                            usr.Name = model.Name;
                            usr.LastName = model.LastName;
                            usr.Role = model.Role;
                            usr.Email = model.Email;                            
                            if (!String.IsNullOrEmpty(model.NewPassword) && model.NewPassword == model.NewPasswordAgain)
                            {
                                GeniusBaseAuthHelper.ChangePassword(model.UserName, model.OldPassword, model.NewPassword);
                            }
                            db.SaveChanges();
                            ShowOperationMessage(UIResources.UserListUserEditSuccessful);
                            return RedirectToAction("Users");
                        }                                                
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                ShowOperationMessage(ex.Message);
                return RedirectToAction("Index", "Error");
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult UserInfo(int id = -1)
        {
            try
            {
                using (var db = new GeniusBaseContext())
                {
                    KbUser usr = db.KbUsers.FirstOrDefault(u => u.Id == id);
                    if (usr == null)
                    {
                        throw new Exception(ErrorMessages.UserNotFound);
                    }
                    KbUserViewModel model = new KbUserViewModel(usr);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                ShowOperationMessage(ErrorMessages.UserNotFound);
                return RedirectToAction("Index", "Error");
            }
        }
        
        [Authorize(Roles="Admin")]
        public ActionResult Users()
        {
            try
            {
                using (var db = new GeniusBaseContext())
                {
                    List<KbUser> Users = db.KbUsers.OrderBy(u => u.UserName).ToList();
                    return View(Users);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                ShowOperationMessage(ex.Message);
                return RedirectToAction("Index", "Error");
            }
        }
        /*
        [AllowAnonymous]
        public void CreateAdmin()
        {
            using (GeniusBaseEntities db = new GeniusBaseEntities())
            {
                KbUser usr = GeniusBaseAuthHelper.CreateUser("admin", "admin", "admin@GeniusBase.comx", "admin" ,1);
                usr = db.KbUsers.FirstOrDefault(u => u.Id == usr.Id);
                if (usr != null)
                {
                    usr.LastName = "User";
                    usr.Name = "Admin";
                    db.SaveChanges();
                }
            }
        }
        */
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create([Bind(Exclude = "Id")]KbUserViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var db = new GeniusBaseContext())
                    {
                        KbUser usr = GeniusBaseAuthHelper.CreateUser(model.UserName, model.OldPassword, model.Email, model.Role,HelperFunctions.UserAsKbUser(User).Id);
                        usr = db.KbUsers.FirstOrDefault(u => u.Id == usr.Id);
                        if (usr != null)
                        {
                            usr.LastName = model.LastName;
                            usr.Name = model.Name;
                            db.SaveChanges();
                        }
                        return RedirectToAction("Users");
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                AddGlobalException(ex);
                return RedirectToAction("Index", "Error");
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }
    
    }
}
