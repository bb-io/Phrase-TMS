﻿using Blackbird.Applications.Sdk.Common;

namespace Apps.PhraseTMS.Dtos;

public class ProjectDto
{
    public string? Name { get; set; }

    [Display("UID")]
    public string UId { get; set; }

    [Display("Creation date")]
    public DateTime? DateCreated { get; set; }

    [Display("Target languages")]
    public List<string> TargetLangs { get; set; }

    [Display("Due date")]
    public DateTime? DateDue { get; set; }

    [Display("Status")]
    public string Status { get; set; }

    [Display("Note")]
    public string Note { get; set; }
}