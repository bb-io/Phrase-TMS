namespace Apps.PhraseTMS.Helpers.Models;
public class MXLIFFUser(string baseUrl)
{
    public string? Id { get; set; }
    public string? Uid { get; set; }
    public string? Username { get; set; }
    public string? FullName { get; set; }
    public string? Url => Uid is not null ? $"{baseUrl.Trim('/')}/tms/user/show/{Uid}" : null;
}
