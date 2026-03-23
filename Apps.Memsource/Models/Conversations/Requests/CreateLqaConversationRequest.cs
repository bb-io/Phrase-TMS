using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Conversations.Requests;

public class CreateLqaConversationRequest
{
    [Display("LQA description")]
    public string? LqaDescription { get; set; }

    [Display("Trans-group ID")]
    public int? TransGroupId { get; set; }

    [Display("Segment ID")]
    [DataSource(typeof(SegmentDataHandler))]
    public string? SegmentId { get; set; }

    [Display("Conversation title")]
    public string? ConversationTitle { get; set; }

    [Display("Conversation title offset")]
    public int? ConversationTitleOffset { get; set; }

    [Display("Commented text")]
    public string? CommentedText { get; set; }

    [Display("Correlation ID")]
    public string? CorrelationUid { get; set; }

    [Display("Correlation role")]
    public string? CorrelationRole { get; set; }

    [Display("Error category IDs")]
    [DataSource(typeof(LqaErrorCategoryDataHandler))]
    public IEnumerable<string> ErrorCategoryIds { get; set; } = Array.Empty<string>();

    [Display("Severity IDs")]
    [DataSource(typeof(LqaSeverityDataHandler))]
    public IEnumerable<string> SeverityIds { get; set; } = Array.Empty<string>();

    [Display("User IDs")]
    public IEnumerable<string>? UserIds { get; set; }

    [Display("Repeated values")]
    public IEnumerable<string>? RepeatedValues { get; set; }

    [Display("Origin values")]
    public IEnumerable<string>? OriginValues { get; set; }
}
