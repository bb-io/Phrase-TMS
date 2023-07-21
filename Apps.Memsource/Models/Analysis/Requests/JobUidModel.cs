namespace Apps.PhraseTMS.Models.Analysis.Requests;

public class JobUidModel
{
    public string Uid { get; set; }

    public JobUidModel(string uid)
    {
        Uid = uid;
    }
}