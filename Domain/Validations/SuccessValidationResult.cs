using System;
using System.Net;

namespace Domain.Validations
{
  [Serializable]
  public abstract class SuccessValidationResult : ValidationResult
  {
    public override IValidationResult AddValidationError(HttpStatusCode statusCode, string message) => this;
  }
}
