﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Memsource.Models.Jobs.Requests
{
    public class GetSegmentsRequest
    {
        public string ProjectUId { get; set; }

        public string JobUId { get; set; }

        public int BeginIndex { get; set; }

        public int EndIndex { get; set; }
    }
}
