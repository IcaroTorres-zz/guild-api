using System;
using Domain.Entities;
using Domain.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataAccess.Validations
{
    [Serializable]
    public class CreatedValidationResult : SuccessValidationResult, ISuccessValidationResult, IValidationResult
    {
        public CreatedValidationResult(object createdData) { Data = createdData; }
        public override IActionResult AsActionResult(HttpRequest request)
        {
            var dataId = Data.GetType().GetProperty(nameof(IBaseEntity.Id)).GetValue(Data);
            
            return new CreatedResult($"{request.Path.ToUriComponent()}{dataId}", Data);
        }

        public override IActionResult AsActionResult()
        {
            var dataType = Data.GetType();
            var dataId = dataType.GetProperty(nameof(IBaseEntity.Id)).GetValue(Data);
            return new CreatedResult($"api/{dataType.Name.ToLowerInvariant()}s/v1/{dataId}", Data);
        }
    }
}
