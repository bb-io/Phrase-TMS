﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Memsource.Models.Responses
{
    public class BaseResponse
    {
        public int StatusCode { get; set; }

        public string Details { get; set; }
    }
}
