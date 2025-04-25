using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Models.Jobs.Requests;
public class TargetLanguageRequest
{
    [Display("Target language")]
    [DataSource(typeof(LanguageDataHandler))]
    public string TargetLang { get; set; }
}
