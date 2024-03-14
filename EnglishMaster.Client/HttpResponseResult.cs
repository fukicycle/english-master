using System.Net;

namespace EnglishMaster.Client
{
    public sealed class HttpResponseResult
    {
        public HttpResponseResult(string json, HttpStatusCode statusCode, string? message = null)
        {
            Json = json;
            StatusCode = statusCode;
            Message = message;
        }
        public string Json { get; }
        public HttpStatusCode StatusCode { get; }
        public string? Message { get; }
    }
}
