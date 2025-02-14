using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Projects.Requests;

public class CreateProjectRequest
{
    public string Name { get; set; }

    [Display("Source language")]
    [DataSource(typeof(LanguageDataHandler))]
    public string SourceLanguage { get; set; }

    [Display("Target languages")]
    [DataSource(typeof(LanguageDataHandler))]
    public IEnumerable<string> TargetLanguages { get; set; }
    
    [Display("Due date")]
    public DateTime? DateDue { get; set; }

    //Client object idreference
    //BusinessUnit object idreference
    //Domain object idreference
    //SubDomain object idreference
    //costCenter object idreference
    //purchaseOrder string
    //workflowSteps array of objects idreference
    //dateDue already done
    //note string
    //lqaProfiles Array of objects (LqaProfilesForWsV2Dto) [ items ]
    //customFieldsArray of objects (CustomFieldInstanceApiDto) [ items ]
    //FileHandover bool
    //propagateTranslationsToLowerWfDuringUpdateSource bool
}