using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace Tests.Helpers.Builders
{
    public sealed class ValidatorMockBuilder<T>
    {
        private readonly Mock<IValidator<T>> _mock;
        private ValidatorMockBuilder()
        {
            _mock = new Mock<IValidator<T>>();
        }

        public static ValidatorMockBuilder<T> Create()
        {
            return new ValidatorMockBuilder<T>();
        }

        public IValidator<T> Build()
        {
            return _mock.Object;
        }

        public ValidatorMockBuilder<T> ValidateTrue()
        {
            var validation = new ValidationResult();
            _mock.Setup(x => x.Validate(It.IsAny<T>())).Returns(validation);
            _mock.Setup(x => x.Validate(It.IsAny<ValidationContext<T>>())).Returns(validation);
            _mock.Setup(x => x.ValidateAsync(It.IsAny<T>(), default)).ReturnsAsync(validation);
            _mock.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<T>>(), default)).ReturnsAsync(validation);

            return this;
        }

        public ValidatorMockBuilder<T> ValidateFalse()
        {
            var validation = new ValidationResult(new[] { new ValidationFailure("", "") });
            _mock.Setup(x => x.Validate(It.IsAny<T>())).Returns(validation);
            _mock.Setup(x => x.Validate(It.IsAny<ValidationContext<T>>())).Returns(validation);
            _mock.Setup(x => x.ValidateAsync(It.IsAny<T>(), default)).ReturnsAsync(validation);
            _mock.Setup(x => x.ValidateAsync(It.IsAny<ValidationContext<T>>(), default)).ReturnsAsync(validation);

            return this;
        }
    }
}
