using System;
using Domain.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataAccess.Validations
{
    [Serializable]
    public class OkValidationResult : SuccessValidationResult, ISuccessValidationResult, IValidationResult
    {
        public OkValidationResult(object returnedData) { Data = returnedData; }
        public override IActionResult AsActionResult() => new OkObjectResult(Data);
        public override IActionResult AsActionResult(HttpRequest request) => AsActionResult();
    }
}
