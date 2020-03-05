using Microsoft.AspNetCore.Mvc;

namespace Domain.Validations
{
    public class NoContentValidationResult : ValidationResult
    {
        public override IActionResult AsActionResult() => new NoContentResult();
    }
}
