using System;

namespace Domain.Validations
{
    [Serializable]
    public class SuccessValidationResult : IValidationResult
    {
        public bool IsValid => true;
    }
}
