using Infrastructures;
using Infrastructures.EF.HelpDB;

namespace Applications.Models
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
