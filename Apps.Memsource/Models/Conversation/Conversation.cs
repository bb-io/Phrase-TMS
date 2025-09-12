using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Apps.PhraseTMS.Models.Conversation
{
    public class ConversationsResponse
    {
        [JsonProperty("conversations")]
        public List<Conversation> Conversations { get; set; }
    }
    public class Conversation
    {
        [JsonProperty("id")]
        [Display("Conversation ID")]
        public string? Id { get; set; }

        [JsonProperty("type")]
        [Display("Conversation type")]
        public string? Type { get; set; }

        [JsonProperty("dateCreated")]
        [Display("Created date")]
        public DateTime? DateCreated { get; set; }

        [JsonProperty("dateModified")]
        [Display("Modified date")]
        public DateTime? DateModified { get; set; }

        [JsonProperty("dateEdited")]
        [Display("Edited date")]
        public DateTime? DateEdited { get; set; }

        [JsonProperty("createdBy")]
        [Display("Created by")]
        public UserInfo? CreatedBy { get; set; }

        [JsonProperty("comments")]
        public List<Comment> Comments { get; set; } = new();

        [JsonProperty("status")]
        public ConversationStatus? Status { get; set; }

        [JsonProperty("deleted")]
        public bool? Deleted { get; set; }

        [JsonProperty("references")]
        public ConversationReferences? References { get; set; }
    }

    public class UserInfo
    {
        [JsonProperty("firstName")]
        [Display("First name")]
        public string? FirstName { get; set; }

        [JsonProperty("lastName")]
        [Display("Last name")]
        public string? LastName { get; set; }

        [JsonProperty("userName")]
        [Display("User name")]
        public string? UserName { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("role")]
        public string? Role { get; set; }

        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("uid")]
        public string? Uid { get; set; }

        [JsonProperty("unavailable")]
        public bool? Unavailable { get; set; }

        [JsonProperty("jobRoles")]
        [Display("Job roles")]
        public JToken? JobRoles { get; set; }
    }

    public class Comment
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("text")]
        public string? Text { get; set; }

        [JsonProperty("createdBy")]
        [Display("Created by")]
        public UserInfo? CreatedBy { get; set; }

        [JsonProperty("dateCreated")]
        public DateTime? DateCreated { get; set; }

        [JsonProperty("dateModified")]
        [Display("Modified date")]
        public DateTime? DateModified { get; set; }

        [JsonProperty("mentions")]
        public List<Mention> Mentions { get; set; } = new();
    }

    public class Mention
    {
        [JsonProperty("mentionType")]
        [Display("Mention type")]
        public string? MentionType { get; set; }

        [JsonProperty("mentionGroupType")]
        [Display("Mention group type")]
        public string? MentionGroupType { get; set; }

        [JsonProperty("uidReference")]
        [Display("ID referencee")]
        public UidReference? UidReference { get; set; }

        [JsonProperty("userReferences")]
        [Display("User references")]
        public List<UserInfo> UserReferences { get; set; } = new();

        [JsonProperty("mentionableGroup")]
        [Display("Mentionable group")]
        public MentionableGroup? MentionableGroup { get; set; }

        [JsonProperty("tag")]
        public string? Tag { get; set; }
    }

    public class UidReference
    {
        [JsonProperty("uid")]
        public string? Uid { get; set; }
    }

    public class MentionableGroup
    {
        [JsonProperty("groupType")]
        [Display("Group type")]
        public string? GroupType { get; set; }

        [JsonProperty("groupName")]
        [Display("Group name")]
        public string? GroupName { get; set; }

        [JsonProperty("groupReference")]
        [Display("Group reference")]
        public GroupReference? GroupReference { get; set; }
    }

    public class GroupReference
    {
        [JsonProperty("uid")]
        public string? Uid { get; set; }
    }

    public class ConversationStatus
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("by")]
        public UserInfo? By { get; set; }

        [JsonProperty("date")]
        public DateTime? Date { get; set; }
    }

    public class ConversationReferences
    {
        [JsonProperty("taskId")]
        [Display("Task ID")]
        public string? TaskId { get; set; }

        [JsonProperty("jobPartUid")]
        [Display("Job part ID")]
        public string? JobPartUid { get; set; }

        [JsonProperty("transGroupId")]
        [Display("Trans-group ID")]
        public int? TransGroupId { get; set; }

        [JsonProperty("segmentId")]
        [Display("Segment ID")]
        public string? SegmentId { get; set; }

        [JsonProperty("conversationTitle")]
        [Display("Conversation title")]
        public string? ConversationTitle { get; set; }

        [JsonProperty("conversationTitleOffset")]
        [Display("Conversation title offset")]
        public int? ConversationTitleOffset { get; set; }

        [JsonProperty("commentedText")]
        [Display("Commented text")]
        public string? CommentedText { get; set; }

        [JsonProperty("correlation")]
        [Display("Correlation")]
        public CorrelationRef? Correlation { get; set; }
    }

    public class CorrelationRef
    {
        [JsonProperty("uid")]
        [Display("Correlation ID")]
        public string? Uid { get; set; }

        [JsonProperty("role")]
        [Display("Correlation role")]
        public string? Role { get; set; }
    }
}
