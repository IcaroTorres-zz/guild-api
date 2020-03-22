using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Validations
{
    [Serializable]
    public abstract class ErrorValidationResult : IErrorValidationResult, IValidationResult
    {
        public ErrorValidationResult(string resourcePath)
        {
            ResourcePath = resourcePath;
        }
        protected string ResourcePath { get; set; }
        public bool IsValid => false;
        public HttpStatusCode Status { get; protected set; }
        public string Title { get; protected set; }
        public IReadOnlyDictionary<string, List<string>> Errors => errors;
        private Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
        public virtual object AsSerializableError() => new { Title, Status, Errors };
        public abstract IActionResult AsErrorActionResult();
        public virtual IErrorValidationResult AddValidationErrors(string keyPath, params string[] newMessages)
        {
            var formattedKey = string.IsNullOrWhiteSpace(keyPath) ? ResourcePath : $"{ResourcePath}.{keyPath}";

            if (errors.TryGetValue(formattedKey, out List<string> previousMessages))
            {
                previousMessages.AddRange(newMessages);
            }
            else
            {
                errors.Add(formattedKey, new List<string>(newMessages));
            }
            
            return this;
        }
    }
}
