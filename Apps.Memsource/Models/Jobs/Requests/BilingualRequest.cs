using Apps.PhraseTMS.DataSourceHandlers.StaticHandlers;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Models.Jobs.Requests
{
    public class BilingualRequest
    {
        [StaticDataSource(typeof(BilingualFormatDataHandler))]
        public string? Format { get; set; }

        public bool? Preview { get; set; }
    }
}
