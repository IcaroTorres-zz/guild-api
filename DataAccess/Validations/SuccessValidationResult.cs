using System;
using Domain.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataAccess.Validations
{
    [Serializable]
    public abstract class SuccessValidationResult : ISuccessValidationResult, IValidationResult
    {
        public bool IsValid => true;
        public object Data { get; protected set; }
        public abstract IActionResult AsActionResult(HttpRequest request);
        public abstract IActionResult AsActionResult();
    }
}
