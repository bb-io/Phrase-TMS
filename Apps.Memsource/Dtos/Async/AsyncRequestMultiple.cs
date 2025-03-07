namespace Apps.PhraseTMS.Dtos.Async;

public class AsyncRequestMultipleResponse<T> where T : AsyncRequestArrayItem
{
    public IEnumerable<T> AsyncRequests { get; set; }
}

public class AsyncRequestArrayItem
{
    public AsyncRequest AsyncRequest { get; set; }
}