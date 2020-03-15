using System;
using Domain.Validations;

namespace DataAccess.Validations
{
    [Serializable]
    public class SuccessValidationResult : IValidationResult
    {
        public bool IsValid => true;
    }
}
