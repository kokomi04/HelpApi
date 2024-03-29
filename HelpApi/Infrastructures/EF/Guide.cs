﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Infrastructures.EF.HelpDB
{
    public partial class Guide
    {
        public int GuideId { get; set; }
        public string GuideCode { get; set; }
        public int? GuideCateId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int SortOrder { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDatetimeUtc { get; set; }
        public int UpdatedByUserId { get; set; }
        public DateTime UpdatedDatetimeUtc { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDatetimeUtc { get; set; }
    }
}
