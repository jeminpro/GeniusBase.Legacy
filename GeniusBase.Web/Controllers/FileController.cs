using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeniusBase.Dal;
using GeniusBase.Dal.Entities;
using GeniusBase.Web.Helpers;
using GeniusBase.Web.Models;
using NLog;
using Resources;

namespace GeniusBase.Web.Controllers
{
    [Authorize]
    public class FileController : Controller
    {

        private Logger Log = LogManager.GetCurrentClassLogger();

        [HttpPost]
        public JsonResult Remove(string id)
        {
            JsonOperationResponse result = new JsonOperationResponse();
            result.Successful = false;
            try
            {
                var parts = id.Split(new[] {"|"}, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                {
                    var attachmentHash = parts[0];
                    var attachmentId = parts[1];

                    Attachment at = new Attachment() { Id = Convert.ToInt64(attachmentId) };
                    at.Author = HelperFunctions.UserAsKbUser(User).Id;
                    AttachmentHelper.RemoveAttachment(attachmentHash, HelperFunctions.UserAsKbUser(User).Id);
                    LuceneHelper.RemoveAttachmentFromIndex(at);
                    result.Successful = true;
                    return Json(result);
                }
                throw new ArgumentOutOfRangeException("Invalid file hash");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                result.ErrorMessage = ex.Message;
                return Json(result);
            }
        }

        [HttpPost]
        public JsonResult Upload()
        {
            JsonOperationResponse result = new JsonOperationResponse();
            result.Successful = false;
            try
            {
                if (Request.Params["ArticleId"] == null)
                {
                    result.ErrorMessage = ErrorMessages.FileUploadArticleNotFound;
                }                                
                else if (Request.Files.Count == 1)
                {
                    long articleId = Convert.ToInt64(Request.Params["ArticleId"]);
                    HttpPostedFileBase attachedFile = Request.Files[0];
                    Attachment attachment = AttachmentHelper.SaveAttachment(articleId, attachedFile, HelperFunctions.UserAsKbUser(User).Id);
                    attachment.Author = HelperFunctions.UserAsKbUser(User).Id;
                    result.Successful = true;
                    result.Data = new AttachmentViewModel(attachment);
                    using (var db = new GeniusBaseContext())
                    {
                        Settings sets = db.Settings.FirstOrDefault();
                        if (sets != null)
                        {
                            string[] extensions = sets.IndexFileExtensions.Split(new string[]{","},StringSplitOptions.RemoveEmptyEntries);

                            if (extensions.FirstOrDefault(a => a.ToLowerInvariant() == attachment.Extension.ToLowerInvariant()) != null )
                            {
                                LuceneHelper.AddAttachmentToIndex(attachment);
                            }
                        }
                        
                    }
                }
                else
                {
                    result.ErrorMessage = ErrorMessages.FileUploadTooManyFiles;
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

    }
}
