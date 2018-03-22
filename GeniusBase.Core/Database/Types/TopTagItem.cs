using System.ComponentModel.DataAnnotations.Schema;

namespace GeniusBase.Dal.Types
{
    [NotMapped]
    public class TopTagItem
    {
        public int? Ratio { get; set; }
        public string Name { get; set; }
        public int? Id { get; set; }
        public int FontSize { get; set; }
    }
}
