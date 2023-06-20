using HelpApi.Commons.GlobalObject;

namespace HelpApi.Models
{
    public class GuideModel : GuideModelOutput
    {
        public string Description { get; set; }
    }

    public class GuideModelOutput
    {
        public int GuideId { get; set; }
        public int? GuideCateId { get; set; }
        public string GuideCode { get; set; }
        public string Title { get; set; }
        public int SortOrder { get; set; }
    }
}
