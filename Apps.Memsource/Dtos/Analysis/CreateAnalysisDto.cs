using Apps.PhraseTMS.Dtos.Async;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Dtos.Analysis;
public class CreateAnalysisDto : AsyncRequestArrayItem
{
    public Analyse Analyse { get; set; }
}

public class Analyse
{
    public string Id { get; set; }
}
