using System;
using Domain.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataAccess.Validations
{
    [Serializable]
    public class NoContentValidationResult : SuccessValidationResult, ISuccessValidationResult, IValidationResult
    {
        public override IActionResult AsActionResult() => new NoContentResult();
        public override IActionResult AsActionResult(HttpRequest request) => AsActionResult();
    }
}
