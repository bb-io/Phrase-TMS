namespace Apps.PhraseTMS.Webhooks.Handlers.Models;

public class WebhookDto
{
    public const string EnabledStatus = "ENABLED";

    public string UId { get; set; }

    public string Name { get; set; }

    public string Url { get; set; }

    public string? Status { get; set; }

    public List<string> Events { get; set; } = [];

    public bool IsEnabled => string.Equals(Status, EnabledStatus, StringComparison.OrdinalIgnoreCase);
}
