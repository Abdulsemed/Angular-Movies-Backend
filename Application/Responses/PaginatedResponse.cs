namespace Application.Responses;
public class PaginatedResponse
{
    public bool Success { get; set; }
    public string Message {  get; set; }
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; } = 0;
    public int PageSize { get; set; } = 5;
    public object? ReturnedObject { get; set; }   

}
