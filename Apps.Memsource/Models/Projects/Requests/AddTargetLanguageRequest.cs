﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Memsource.Models.Projects.Requests
{
    public class AddTargetLanguageRequest
    {
        public string ProjectUId { get; set; }

        public IEnumerable<string> TargetLanguages { get; set; }
    }
}