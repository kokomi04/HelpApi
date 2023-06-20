using HelpApi.Commons.GlobalObject;
using HelpApi.EF;

namespace HelpApi.Models
{
    public class GuideCateModel
    {
        public int? GuideCateId { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }

    }
}
