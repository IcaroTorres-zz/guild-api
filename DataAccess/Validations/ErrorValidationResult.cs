using System;
using System.Collections.Generic;
using System.Net;
using Domain.Validations;
using Microsoft.AspNetCore.Mvc;

namespace DataAccess.Validations
{
    [Serializable]
    public abstract class ErrorValidationResult : IErrorValidationResult, IValidationResult
    {
        public bool IsValid => false;
        public HttpStatusCode Status { get; protected set; }
        public string Title { get; protected set; }
        public IReadOnlyDictionary<string, List<string>> Errors => errors;
        private Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
        public virtual object AsSerializableError() => new { Title, Status, Errors };
        public abstract IActionResult AsErrorActionResult();
        public virtual IErrorValidationResult AddValidationError(string key, string errorMessage)
        {
            if (errors.TryGetValue(key, out List<string> errorMessages))
            {
                errorMessages.Add(errorMessage);
            }
            else
            {
                errors.Add(key, new List<string>{ errorMessage });
            }
            return this;
        }
        public virtual IErrorValidationResult AddValidationErrors(string key, List<string> newErrorMessages)
        {
            if (errors.TryGetValue(key, out List<string> errorMessages))
            {
                errorMessages.AddRange(newErrorMessages);
            }
            else
            {
                errors.Add(key, newErrorMessages);
            }
            return this;
        }
    }
}
