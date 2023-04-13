using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Models.QualityAssurance.Requests
{
    public class AddIgnoredWarningRequest
    {
        public string ProjectUId { get; set; }

        public string JobUId { get; set; }

        public string SegmentUId { get; set; }

        public string WarningId { get; set; }
    }
}
