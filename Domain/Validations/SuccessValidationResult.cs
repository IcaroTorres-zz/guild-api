using System.Net;

namespace Domain.Validations
{
  public class SuccessValidationResult : ValidationResult
  {
    public override IValidationResult AddValidationError(HttpStatusCode statusCode, string message) => this;
  }
}
