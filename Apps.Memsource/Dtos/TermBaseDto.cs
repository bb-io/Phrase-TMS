namespace Apps.PhraseTMS.Dtos;

public class TermBaseDto
{
    public string Id { get; set; }
    public string UId { get; set; }
    public string Name { get; set; }

    public List<string> Langs { get; set; }
}