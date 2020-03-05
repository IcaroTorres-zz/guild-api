using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Validations
{
    public class CreatedValidationResult : SuccessValidationResult
    {
        public CreatedValidationResult(object createdData) : base()
        {
            Data = createdData;
        }
        public override IActionResult AsActionResult(HttpRequest request)
        {
            var dataId = Data.GetType().GetProperty(nameof(IBaseEntity.Id)).GetValue(Data);
            
            return new CreatedResult($"{request.Path.ToUriComponent()}{dataId}", Data);
        }
    }
}
