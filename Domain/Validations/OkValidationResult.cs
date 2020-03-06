using System;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Validations
{
    [Serializable]
    public class OkValidationResult : SuccessValidationResult
    {
        public OkValidationResult(object returnedData) : base()
        {
            Data = returnedData;
        }
        public override IActionResult AsActionResult() => new OkObjectResult(Data);
    }
}
