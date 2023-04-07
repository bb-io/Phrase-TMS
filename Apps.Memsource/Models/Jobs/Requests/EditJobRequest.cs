using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTms.Models.Jobs.Requests
{
    public class EditJobRequest
    {
        public string ProjectUId { get; set; }

        public string JobUId { get; set; }

        public string DateDue { get; set; }

        public string Status { get; set; } //"ACCEPTED" "CANCELLED" "COMPLETED" "DECLINED" "DELIVERED" "EMAILED" "NEW" "REJECTED"
    }
}
