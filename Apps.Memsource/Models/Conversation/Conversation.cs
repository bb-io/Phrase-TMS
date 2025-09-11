using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Apps.PhraseTMS.Models.Conversation
{
    public class ConversationsResponse
    {
        [JsonProperty("conversations")]
        public List<Conversation> Conversations { get; set; } = new();
    }
    public class Conversation
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("dateCreated")]
        public DateTimeOffset? DateCreated { get; set; }

        [JsonProperty("dateModified")]
        public DateTimeOffset? DateModified { get; set; }

        [JsonProperty("dateEdited")]
        public DateTimeOffset? DateEdited { get; set; }

        [JsonProperty("createdBy")]
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
        public string? FirstName { get; set; }

        [JsonProperty("lastName")]
        public string? LastName { get; set; }

        [JsonProperty("userName")]
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
        public JToken? JobRoles { get; set; }
    }

    public class Comment
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("text")]
        public string? Text { get; set; }

        [JsonProperty("createdBy")]
        public UserInfo? CreatedBy { get; set; }

        [JsonProperty("dateCreated")]
        public DateTimeOffset? DateCreated { get; set; }

        [JsonProperty("dateModified")]
        public DateTimeOffset? DateModified { get; set; }

        [JsonProperty("mentions")]
        public List<Mention> Mentions { get; set; } = new();
    }

    public class Mention
    {
        [JsonProperty("mentionType")]
        public string? MentionType { get; set; }

        [JsonProperty("mentionGroupType")]
        public string? MentionGroupType { get; set; }

        [JsonProperty("uidReference")]
        public UidReference? UidReference { get; set; }

        [JsonProperty("userReferences")]
        public List<UserInfo> UserReferences { get; set; } = new();

        [JsonProperty("mentionableGroup")]
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
        public string? GroupType { get; set; }

        [JsonProperty("groupName")]
        public string? GroupName { get; set; }

        [JsonProperty("groupReference")]
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
        public DateTimeOffset? Date { get; set; }
    }

    public class ConversationReferences
    {
        [JsonProperty("taskId")]
        public string? TaskId { get; set; }

        [JsonProperty("jobPartUid")]
        public string? JobPartUid { get; set; }

        [JsonProperty("transGroupId")]
        public int? TransGroupId { get; set; }

        [JsonProperty("segmentId")]
        public string? SegmentId { get; set; }

        [JsonProperty("conversationTitle")]
        public string? ConversationTitle { get; set; }

        [JsonProperty("conversationTitleOffset")]
        public int? ConversationTitleOffset { get; set; }

        [JsonProperty("commentedText")]
        public string? CommentedText { get; set; }

        [JsonProperty("correlation")]
        public CorrelationRef? Correlation { get; set; }
    }

    public class CorrelationRef
    {
        [JsonProperty("uid")]
        public string? Uid { get; set; }

        [JsonProperty("role")]
        public string? Role { get; set; }
    }
}
