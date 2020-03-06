using System;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Validations
{
    [Serializable]
    public class NoContentValidationResult : ValidationResult
    {
        public override IActionResult AsActionResult() => new NoContentResult();
    }
}
