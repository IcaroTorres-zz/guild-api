using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Domain.Validations
{
    [Serializable]
    public class ApiValidationResult : IApiValidationResult
    {
        private readonly ValidationResult FluentResult; 
        public ApiValidationResult(ValidationResult validationResult)
        {
            FluentResult = validationResult;
        }
        public bool IsValid => FluentResult.IsValid;
        public string Title { get; protected set; } = "Conflict in model validation.";
        public IReadOnlyDictionary<string, List<string>> Errors =>
            (from error in FluentResult.Errors
             group error by error.PropertyName into errors
             orderby errors.Key
             select errors)            
            .ToDictionary(e => e.Key, e => e.Select(error => error.ErrorMessage).Distinct().ToList());

        public virtual object AsSerializableError() => new { Title, Status = (int)HttpStatusCode.Conflict, Errors };
        public virtual IActionResult AsErrorActionResult() => new ConflictObjectResult(AsSerializableError());
    }
}
