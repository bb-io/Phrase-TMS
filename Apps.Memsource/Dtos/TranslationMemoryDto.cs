﻿using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Dtos;

public class TranslationMemoryDto
{
    [Display("TM ID")]
    public string UId { get; set; }

    public string Name { get; set; }

    [Display("Source language")]
    [DataSource(typeof(LanguageDataHandler))]
    public string SourceLang { get; set; }

    [Display("Target languages")]
    [DataSource(typeof(LanguageDataHandler))]
    public IEnumerable<string> TargetLangs { get; set; }

    [Display("Created at")]
    public DateTime DateCreated { get; set; }
}