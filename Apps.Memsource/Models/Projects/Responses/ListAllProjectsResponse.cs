﻿using Apps.Memsource.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Memsource.Models.Projects.Responses
{
    public class ListAllProjectsResponse
    {
        public IEnumerable<ProjectDto> Projects { get; set; }
    }
}