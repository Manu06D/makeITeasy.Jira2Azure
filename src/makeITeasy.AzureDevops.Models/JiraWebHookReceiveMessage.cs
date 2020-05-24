using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace makeITeasy.AzureDevops.Models
{
    public enum JiraIssueEventType
    {
        unknown = 99,
        issue_updated = 1,
        issue_created = 0,
    }

    public partial class JiraWebHookReceiveMessage
    {
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("webhookEvent")]
        public string WebhookEvent { get; set; }

        [JsonProperty("issue_event_type_name")]
        [JsonConverter(typeof(StringEnumConverter))]
        public JiraIssueEventType IssueEventTypeName { get; set; }

        //[JsonProperty("user")]
        //public User User { get; set; }

        [JsonProperty("issue")]
        public JiraIssue Issue { get; set; }

        [JsonProperty("changelog")]
        public Changelog Changelog { get; set; }
    }

    public partial class Changelog
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("items")]
        public JiraItem[] Items { get; set; }
    }

    public partial class JiraItem
    {
        [JsonProperty("field")]
        public string Field { get; set; }

        [JsonProperty("fieldtype")]
        public string Fieldtype { get; set; }

        [JsonProperty("fieldId")]
        public string FieldId { get; set; }

        [JsonProperty("from")]
        public object From { get; set; }

        [JsonProperty("fromString")]
        public object FromString { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("toString")]
        public string ItemToString { get; set; }

        [JsonProperty("tmpFromAccountId")]
        public object TmpFromAccountId { get; set; }

        [JsonProperty("tmpToAccountId", NullValueHandling = NullValueHandling.Ignore)]
        public string TmpToAccountId { get; set; }
    }

    public partial class JiraIssue
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("self")]
        public Uri Self { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("fields")]
        public Fields Fields { get; set; }
    }

    public partial class Fields
    {
        [JsonProperty("statuscategorychangedate")]
        public string Statuscategorychangedate { get; set; }

        [JsonProperty("issuetype")]
        public Issuetype Issuetype { get; set; }

        [JsonProperty("timespent")]
        public object Timespent { get; set; }

        [JsonProperty("project")]
        public Project Project { get; set; }

        [JsonProperty("fixVersions")]
        public object[] FixVersions { get; set; }

        [JsonProperty("aggregatetimespent")]
        public object Aggregatetimespent { get; set; }

        [JsonProperty("resolution")]
        public object Resolution { get; set; }

        [JsonProperty("resolutiondate")]
        public object Resolutiondate { get; set; }

        [JsonProperty("workratio")]
        public long Workratio { get; set; }

        [JsonProperty("watches")]
        public Watches Watches { get; set; }

        [JsonProperty("lastViewed")]
        public string LastViewed { get; set; }

        [JsonProperty("created")]
        public string Created { get; set; }

        [JsonProperty("customfield_10020")]
        public object Customfield10020 { get; set; }

        [JsonProperty("customfield_10021")]
        public object Customfield10021 { get; set; }

        [JsonProperty("customfield_10022")]
        public object Customfield10022 { get; set; }

        [JsonProperty("priority")]
        public Priority Priority { get; set; }

        [JsonProperty("customfield_10023")]
        public object Customfield10023 { get; set; }

        [JsonProperty("labels")]
        public string[] Labels { get; set; }

        [JsonProperty("customfield_10016")]
        public object Customfield10016 { get; set; }

        [JsonProperty("customfield_10017")]
        public object Customfield10017 { get; set; }

        [JsonProperty("customfield_10018")]
        public Customfield10018 Customfield10018 { get; set; }

        [JsonProperty("customfield_10019")]
        public string Customfield10019 { get; set; }

        [JsonProperty("timeestimate")]
        public object Timeestimate { get; set; }

        [JsonProperty("aggregatetimeoriginalestimate")]
        public object Aggregatetimeoriginalestimate { get; set; }

        [JsonProperty("versions")]
        public object[] Versions { get; set; }

        [JsonProperty("issuelinks")]
        public object[] Issuelinks { get; set; }

        [JsonProperty("assignee")]
        public object Assignee { get; set; }

        [JsonProperty("updated")]
        public string Updated { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("components")]
        public object[] Components { get; set; }

        [JsonProperty("timeoriginalestimate")]
        public object Timeoriginalestimate { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("customfield_10010")]
        public object Customfield10010 { get; set; }

        [JsonProperty("customfield_10014")]
        public object Customfield10014 { get; set; }

        [JsonProperty("timetracking")]
        public Timetracking Timetracking { get; set; }

        [JsonProperty("customfield_10015")]
        public object Customfield10015 { get; set; }

        [JsonProperty("customfield_10005")]
        public object Customfield10005 { get; set; }

        [JsonProperty("customfield_10006")]
        public object Customfield10006 { get; set; }

        [JsonProperty("customfield_10007")]
        public object Customfield10007 { get; set; }

        [JsonProperty("security")]
        public object Security { get; set; }

        [JsonProperty("customfield_10008")]
        public object Customfield10008 { get; set; }

        [JsonProperty("customfield_10009")]
        public object Customfield10009 { get; set; }

        [JsonProperty("aggregatetimeestimate")]
        public object Aggregatetimeestimate { get; set; }

        [JsonProperty("attachment")]
        public object[] Attachment { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("creator")]
        public User Creator { get; set; }

        [JsonProperty("subtasks")]
        public object[] Subtasks { get; set; }

        [JsonProperty("reporter")]
        public User Reporter { get; set; }

        [JsonProperty("customfield_10000")]
        public string Customfield10000 { get; set; }

        [JsonProperty("aggregateprogress")]
        public Progress Aggregateprogress { get; set; }

        [JsonProperty("customfield_10001")]
        public object Customfield10001 { get; set; }

        [JsonProperty("customfield_10002")]
        public object Customfield10002 { get; set; }

        [JsonProperty("customfield_10003")]
        public object Customfield10003 { get; set; }

        [JsonProperty("customfield_10004")]
        public object Customfield10004 { get; set; }

        [JsonProperty("environment")]
        public object Environment { get; set; }

        [JsonProperty("duedate")]
        public object Duedate { get; set; }

        [JsonProperty("progress")]
        public Progress Progress { get; set; }

        [JsonProperty("votes")]
        public Votes Votes { get; set; }
    }

    public partial class Progress
    {
        [JsonProperty("progress")]
        public long ProgressProgress { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }
    }

    public partial class User
    {
        [JsonProperty("self")]
        public Uri Self { get; set; }

        [JsonProperty("accountId")]
        public string AccountId { get; set; }

        [JsonProperty("avatarUrls")]
        public AvatarUrls AvatarUrls { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("timeZone")]
        public string TimeZone { get; set; }

        [JsonProperty("accountType")]
        public string AccountType { get; set; }
    }

    public partial class AvatarUrls
    {
        [JsonProperty("48x48")]
        public Uri The48X48 { get; set; }

        [JsonProperty("24x24")]
        public Uri The24X24 { get; set; }

        [JsonProperty("16x16")]
        public Uri The16X16 { get; set; }

        [JsonProperty("32x32")]
        public Uri The32X32 { get; set; }
    }

    public partial class Customfield10018
    {
        [JsonProperty("hasEpicLinkFieldDependency")]
        public bool HasEpicLinkFieldDependency { get; set; }

        [JsonProperty("showField")]
        public bool ShowField { get; set; }

        [JsonProperty("nonEditableReason")]
        public NonEditableReason NonEditableReason { get; set; }
    }

    public partial class NonEditableReason
    {
        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public partial class Issuetype
    {
        [JsonProperty("self")]
        public Uri Self { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("iconUrl")]
        public Uri IconUrl { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("subtask")]
        public bool Subtask { get; set; }

        [JsonProperty("avatarId")]
        public long AvatarId { get; set; }
    }

    public partial class Priority
    {
        [JsonProperty("self")]
        public Uri Self { get; set; }

        [JsonProperty("iconUrl")]
        public Uri IconUrl { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }
    }

    public partial class Project
    {
        [JsonProperty("self")]
        public Uri Self { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("projectTypeKey")]
        public string ProjectTypeKey { get; set; }

        [JsonProperty("simplified")]
        public bool Simplified { get; set; }

        [JsonProperty("avatarUrls")]
        public AvatarUrls AvatarUrls { get; set; }
    }

    public partial class Status
    {
        [JsonProperty("self")]
        public Uri Self { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("iconUrl")]
        public Uri IconUrl { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("statusCategory")]
        public StatusCategory StatusCategory { get; set; }
    }

    public partial class StatusCategory
    {
        [JsonProperty("self")]
        public Uri Self { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("colorName")]
        public string ColorName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class Timetracking
    {
    }

    public partial class Votes
    {
        [JsonProperty("self")]
        public Uri Self { get; set; }

        [JsonProperty("votes")]
        public long VotesVotes { get; set; }

        [JsonProperty("hasVoted")]
        public bool HasVoted { get; set; }
    }

    public partial class Watches
    {
        [JsonProperty("self")]
        public Uri Self { get; set; }

        [JsonProperty("watchCount")]
        public long WatchCount { get; set; }

        [JsonProperty("isWatching")]
        public bool IsWatching { get; set; }
    }
}
