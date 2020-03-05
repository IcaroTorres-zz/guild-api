using System.Net;

namespace Domain.Validations
{
    public class ValidationPair : IValidationPair
    {
        public ValidationPair(HttpStatusCode status, string message)
        {
            Status = status;
            Message = message;
        }
        public HttpStatusCode Status { get; private set; }
        public string Message { get; private set; }
        public override string ToString() => $"{Status}: {Message}";
    }
}