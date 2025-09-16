using Apps.PhraseTMS.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.PhraseTMS.Models.Conversations.Requests;

public class CommentRequest
{
    [DataSource(typeof(CommentDataHandler))]
    [Display("Comment ID")]
    public string CommentUId { get; set; }
}
