﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using GeniusBase.Dal.Entities;
using GeniusBase.Dal.Types;

namespace GeniusBase.Dal.Repository
{
    public class TagRepository : ITagRepository
    {

        public IList<TopTagItem> GetTagCloud()
        {
            var PopularTags = GetTopTags().OrderBy(c => Guid.NewGuid()).ToList();
            var maxTagRatio = PopularTags.Max(t => t.Ratio).HasValue ? Convert.ToInt32(PopularTags.Max(t => t.Ratio).Value) : -1;
            var minTagRatio = PopularTags.Min(t => t.Ratio).HasValue ? Convert.ToInt32(PopularTags.Min(t => t.Ratio).Value) : -1;
            var ratioDiff = maxTagRatio - minTagRatio;
            var minRatio = minTagRatio;
            foreach (var item in PopularTags)
            {
                if (ratioDiff > 0)
                    item.FontSize = 80 + Convert.ToInt32(Math.Truncate((double)(item.Ratio - minRatio) * (100 / ratioDiff)));
                else
                    item.FontSize = 80;
            }

            return PopularTags;
        }

        public IList<TopTagItem> GetTopTags()
        {
            using (var db = new GeniusBaseContext())
            {
                return db.Database.SqlQuery<TopTagItem>("exec GetTopTags").ToList();                
            }
                
        }

        public void RemoveTagFromArticles(int tagId)
        {
            using (var db = new GeniusBaseContext())
            {
                var tagIdParam = new SqlParameter("TagId", tagId);
                db.Database.ExecuteSqlCommand("exec RemoveTagFromArticles @TagId", tagIdParam);
            }
        }        
    }
}