using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTms.Dtos
{
    public class JobDto
    {
        public string Filename { get; set; }

        public string Status { get; set; }

        public string TargetLang { get; set; }

        public string DateDue { get; set; }
    }
}
