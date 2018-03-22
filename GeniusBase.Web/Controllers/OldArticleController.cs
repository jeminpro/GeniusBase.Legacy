using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeniusBase.Dal;
using GeniusBase.Dal.Entities;
using GeniusBase.Dal.Repository;
using GeniusBase.Web.Business.Articles;
using GeniusBase.Web.Business.Categories;
using GeniusBase.Web.Helpers;
using GeniusBase.Web.Models;
using NLog;
using Resources;

namespace GeniusBase.Web.Controllers
{
    [Authorize]
    public class OldArticleController : GeniusBaseAdminController
    {        
        public IArticleRepository ArticleRepository { get; set; }
        public IArticleFactory ArticleFactory { get; set; }
        public ICategoryRepository CategoryRepository{ get; set; }
      
        [HttpPost]
        public JsonResult Remove(int id)
        {
            JsonOperationResponse result = new JsonOperationResponse();
            try
            {
                using (var db = new GeniusBaseContext())
                {
                    //Remove article and attachments                    
                    long CurrentUserId = HelperFunctions.UserAsKbUser(User).Id;
                    SqlParameter[] queryParams = new SqlParameter[] { new SqlParameter("ArticleId", id) };
                    db.Database.ExecuteSqlCommand("Delete from ArticleTag Where ArticleId = @ArticleId", queryParams);
                    Article article = db.Articles.Single(a => a.Id == id);
                    if (article == null)
                        throw new Exception(ErrorMessages.ArticleNotFound);
                    
                    while( article.Attachments.Count > 0 ) 
                    {
                        Attachment a = article.Attachments.First();
                        AttachmentHelper.RemoveLocalAttachmentFile(a);
                        LuceneHelper.RemoveAttachmentFromIndex(a);
                        article.Attachments.Remove(a);
                        /* 
                         * Also remove the attachment from db.attachments collection
                         * 
                         * http://stackoverflow.com/questions/17723626/entity-framework-remove-vs-deleteobject                        
                         * 
                         * If the relationship is required (the FK doesn't allow NULL values) and the relationship is not 
                         * identifying (which means that the foreign key is not part of the child's (composite) primary key) 
                         * you have to either add the child to another parent or you have to explicitly delete the child 
                         * (with DeleteObject then). If you don't do any of these a referential constraint is 
                         * violated and EF will throw an exception when you call SaveChanges - 
                         * the infamous "The relationship could not be changed because one or more of the foreign-key properties 
                         * is non-nullable" exception or similar.
                         */
                        db.Attachments.Remove(a);
                    }                      
                    article.Author = CurrentUserId;
                    LuceneHelper.RemoveArticleFromIndex(article);
                    db.Articles.Remove(article);
                    db.SaveChanges();
                    result.Data = id;
                    result.Successful = true;
                    return Json(result);
                }
                
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                result.Successful = false;
                result.ErrorMessage = ex.Message;
                return Json(result);
            }
        }
        
        [HttpPost]
        public ActionResult Edit([Bind(Exclude = "Category.Name,Category.SefName")]ArticleViewModel model)
        {
            try
            {                
                ModelState.Remove("Category.Name");
                ModelState.Remove("Category.SefName");
                if (ModelState.IsValid)
                {
                    var rawHtmlContent = System.Web.HttpUtility.HtmlDecode(model.Content);

                    var article = ArticleRepository.Get(model.Id);
                    article.CategoryId = model.Category.Id;
                    article.IsDraft = model.IsDraft ? 1 : 0;
                    article.Edited = DateTime.Now;
                    article.Title = model.Title;
                    article.Content = rawHtmlContent;
                    article.PlainTextContent = HelperFunctions.HtmlToPlainText(rawHtmlContent);
                    article.Author = HelperFunctions.UserAsKbUser(User).Id;
                    article.SefName = model.SefName;
                    ArticleRepository.Update(article, model.Tags);
                    if (article.IsDraft == 0)
                        LuceneHelper.AddArticleToIndex(article);
                    else
                        LuceneHelper.RemoveArticleFromIndex(article);
                    ShowOperationMessage(UIResources.ArticleCreatePageEditSuccessMessage);
                }                
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                ModelState.AddModelError("Exception", ex.Message);                
            }
            return View("Create", model);
        }

        public ActionResult Edit(int id)
        {
            try
            {
                return View("Create", ArticleFactory.CreateArticleViewModel(ArticleRepository.Get(id)));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("Index", "Error");
            }
        }

        [HttpPost]
        public ActionResult Create([Bind(Exclude = "Category.Name,Category.SefName")]ArticleViewModel model)
        {
            try
            {
                ModelState.Remove("Category.Name");
                ModelState.Remove("Category.SefName");
                if (ModelState.IsValid)
                {                    
                    Article article = ArticleFactory.CreateArticleFromViewModel(model, HelperFunctions.UserAsKbUser(User).Id);
                    var id = ArticleRepository.Add(article, model.Tags);                        
                    if( article.IsDraft == 0 )
                        LuceneHelper.AddArticleToIndex(article);
                    ShowOperationMessage(UIResources.ArticleCreatePageCreateSuccessMessage);
                    return RedirectToAction("Edit", "Article", new { id = article.Id});                    
                }                
                return View(model);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                ModelState.AddModelError("Exception",ex.Message);
                return View(model);
            }
        }
        public ActionResult Create(int id= -1)
        {
            try
            {
                Category category = null;
                try
                {
                    category = CategoryRepository.Get(id);
                }
                catch (ArgumentNullException)
                {                    
                }
                var model = ArticleFactory.CreateArticleViewModelWithDefValues(category);
                return View(model);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("Index", "Error");
            }
            
        }

    }
}
