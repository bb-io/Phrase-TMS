namespace Apps.PhraseTMS.Models.Responses;

public class PaginationResponse<T> : ResponseWrapper<T>
{
    public int TotalPages { get; set; }
}