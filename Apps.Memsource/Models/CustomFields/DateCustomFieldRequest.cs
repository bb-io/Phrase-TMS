﻿using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.CustomFields
{
    public class DateCustomFieldRequest
    {
        [Display("Field ID")]
        [DataSource(typeof(CustomFieldDateDataHandler))]
        public string FieldUId { get; set; }
    }
}
