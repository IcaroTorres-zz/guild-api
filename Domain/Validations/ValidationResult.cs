using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Domain.Validations
{
  [Serializable]
  public abstract class ValidationResult : IValidationResult
  {
    public virtual bool IsValid => !Errors.Any();
    public object Data { get; protected set; }
    protected List<IValidationPair> errors = new List<IValidationPair>();
    public virtual HttpStatusCode Status => (HttpStatusCode)Errors.Max(e => (int)e.Status);
    public IReadOnlyList<IValidationPair> Errors => errors;
    public virtual IValidationResult AddValidationError(HttpStatusCode statusCode, string message)
    {
      errors.Add(new ValidationPair(statusCode, message) as IValidationPair);
      return this;
    }
    public virtual object AsSerializableError() => new { Status, Errors = Errors.Select(e => e.ToString()) };
    public virtual string AsSerializedError() => JsonConvert.SerializeObject(AsSerializableError(), Formatting.Indented);
    public virtual IActionResult AsActionResult()
    {
      IActionResult actionResult = new OkObjectResult(Data);

      if (errors.Any() || !IsValid)
      {
        actionResult = new ContentResult
        {
          Content = AsSerializedError(),
          StatusCode = (int)Status
        };
      }
      return actionResult;
    }
    public virtual IActionResult AsActionResult(HttpRequest request) => AsActionResult();
  }
}
