using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.DataSourceHandlers.StaticHandlers
{
    public class SegmentFilterDataHandler : IStaticDataSourceItemHandler
    {
        public IEnumerable<DataSourceItem> GetData() => new List<DataSourceItem>()
        {
            new DataSourceItem("LOCKED", "Locked segments"),
            new DataSourceItem("NOT_LOCKED", "Not locked segments"),
        };
    }
}
