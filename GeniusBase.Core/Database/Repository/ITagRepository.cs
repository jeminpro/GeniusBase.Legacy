using System.Collections.Generic;
using GeniusBase.Dal.Entities;
using GeniusBase.Dal.Types;

namespace GeniusBase.Dal.Repository
{
    public interface ITagRepository
    {
        void RemoveTagFromArticles(int tagId);
        IList<TopTagItem> GetTopTags();
        IList<TopTagItem> GetTagCloud();
    }
}