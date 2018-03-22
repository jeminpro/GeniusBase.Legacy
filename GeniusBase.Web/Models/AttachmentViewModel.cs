﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GeniusBase.Dal;
using GeniusBase.Dal.Entities;
using Resources;

namespace GeniusBase.Web.Models
{
    public class AttachmentViewModel
    {
        public AttachmentViewModel()
        {
        }
        public AttachmentViewModel(Attachment attachment)
        {
            this.ArticleId = attachment.ArticleId;
            this.Downloads = attachment.Downloads;
            this.Extension = attachment.Extension;
            this.FileName = attachment.FileName;
            this.Id = attachment.Id;
            this.Path = attachment.Path;
            this.Hash = attachment.Hash;
        }

        public long Id { get; set; }
        public long ArticleId { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public long Downloads { get; set; }
        public string Hash { get; set; }

        public string RemoveConfirmMessage
        {
            get
            {
                return UIResources.ArticleAttachmentRemoveConfirmation;
            }
        }
        public string RemoveLink
        {
            get
            {
                UrlHelper linkHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
                return linkHelper.Action("Remove", "File", new { id = this.Hash+"|"+this.Id });                
            }
        }
        public string DownloadLink
        {
            get
            {
                UrlHelper linkHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
                return linkHelper.Content(Path + FileName);
            }
        }
        
    }
}