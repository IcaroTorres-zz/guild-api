using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        public string Title { get; protected set; } = "One or more validation errors occurred.";

        public IReadOnlyDictionary<string, List<string>> Errors => FluentResult.Errors
            .GroupBy(e => e.PropertyName)
            .OrderBy(e => e.FirstOrDefault().PropertyName)
            .ToDictionary(e => e.Key, e => e.Select(error => error.ErrorMessage).Distinct().ToList());

        public int Status => FluentResult.Errors
            .GroupBy(e => e.Severity)
            .OrderBy(e => e.FirstOrDefault().Severity)
            .Select(e => e.Select(vl => int.TryParse(
                vl.ErrorCode, out int code) ? code : (int)HttpStatusCode.Conflict).Max())
            .FirstOrDefault();

        public virtual object AsSerializableError()
        {
            return new { Title, Status, Errors };
        }

        public virtual IActionResult AsErrorActionResult()
        {
            return new ContentResult
            {
                ContentType = "application/json",
                Content = JsonConvert.SerializeObject(AsSerializableError()),
                StatusCode = Status
            };
        }
    }
}
