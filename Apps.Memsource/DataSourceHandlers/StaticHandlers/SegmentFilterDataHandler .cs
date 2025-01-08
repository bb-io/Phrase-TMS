using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.PhraseTMS.DataSourceHandlers.StaticHandlers
{
    public class SegmentFilterDataHandler : IStaticDataSourceHandler
    {
        public Dictionary<string, string> GetData() => new()
        {
        { "LOCKED", "Locked segments" },
        { "NOT_LOCKED", "Not locked segments" }
        };
    }
}
