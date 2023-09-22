namespace Apps.PhraseTMS.Models.Async;

public class AsyncRequestMultipleResponse
{
    public IEnumerable<AsyncRequestArrayItem> AsyncRequests { get; set; }
}

public class AsyncRequestArrayItem
{
    public AsyncRequest AsyncRequest { get; set; }
}