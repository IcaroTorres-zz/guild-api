using System.Net;

namespace Domain.Validations
{
    public interface IValidationPair
    {
        HttpStatusCode Status { get; }
        string Message { get; }
    }
}