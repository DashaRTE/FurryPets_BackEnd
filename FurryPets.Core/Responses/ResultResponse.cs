using System.Net;

namespace FurryPets.Core.Responses;
public class ResultResponse<T>
{
    public HttpStatusCode StatusCode { get; set; }
    public string Message { get; set; } = "";
    public T? Data { get; set; }
}

public class ResultResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public string Message { get; set; } = "";
}
