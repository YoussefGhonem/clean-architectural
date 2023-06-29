using SendGrid;
using System.Net;
using System.Runtime.Serialization;

namespace Elearninig.Packages.Email.SendGrid.Extensions;

[Serializable]
internal class SendGridException : Exception
{
    public int StatusCode { get; }
    public string ResponseData { get; }
    public Dictionary<string, IEnumerable<string>> Headers { get; }

    public static async Task ThrowExceptionOnError(Response response)
    {
        if (response.StatusCode != HttpStatusCode.OK &&
            response.StatusCode != HttpStatusCode.Accepted &&
            response.StatusCode != HttpStatusCode.Created &&
            response.StatusCode != HttpStatusCode.NoContent)
        {
            var headers = response.Headers.ToDictionary(header => header.Key, header => header.Value);
            var responseData = response.Body == null ? null : await response.Body.ReadAsStringAsync();

            var errorMessage =
                $"The HTTP status code of the response was not expected ({response.StatusCode}-{(int)response.StatusCode})";

            throw new SendGridException(
                errorMessage,
                (int)response.StatusCode,
                responseData,
                headers);
        }
    }

    private SendGridException()
    {
    }

    private SendGridException(string message) : base(message)
    {
    }

    private SendGridException(string message, Exception innerException) : base(message, innerException)
    {
    }

    private SendGridException(string message, int statusCode, string responseData,
        Dictionary<string, IEnumerable<string>> headers) : base(message)
    {
        StatusCode = statusCode;
        ResponseData = responseData;
        Headers = headers;
    }

    private SendGridException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}