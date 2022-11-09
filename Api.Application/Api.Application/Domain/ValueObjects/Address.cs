namespace Api.Application.Domain.ValueObjects
{

    using FluentValidation;
    using FluentValidation.Results;

    public class Address : AbstractValidator<Address>
    {
        public Address(string _Publicplace,
            string _Number,
            string _City,
            string _State,
            string _ZipCode)
        {
            Publicplace = _Publicplace;
            Number = _Number;
            City = _City;
            State = _State;
            ZipCode = _ZipCode;
        }

        public string Publicplace { get; private set; }
        public string Number { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string ZipCode { get; private set; }

        public ValidationResult ValidationResult { get; set; }

        public virtual bool _Validate()
        {
            ValidatePublicPlace();
            ValidateCity();
            ValidateState();
            ValidateZipCode();

            ValidationResult = Validate(this);

            return ValidationResult.IsValid;
        }

        private void ValidatePublicPlace()
        {
            RuleFor(c => c.Publicplace)
                .NotEmpty().WithMessage("Public Place is not empty")
                .MaximumLength(50).WithMessage("Public Place can take maximum 50 characters");
        }

        private void ValidateCity()
        {
            RuleFor(c => c.City)
                .NotEmpty().WithMessage("City is not empty")
                .MaximumLength(100).WithMessage("City Place can take maximum 100 characters");
        }

        private void ValidateState()
        {
            RuleFor(c => c.State)
                .NotEmpty().WithMessage("State is ot empty")
                .MaximumLength(2).WithMessage("State is just abreviation");
        }

        private void ValidateZipCode()
        {
            RuleFor(c => c.ZipCode)
                .NotEmpty().WithMessage("Zipcode is not empty")
                .Length(8).WithMessage("Zipcode can has maximum 8 characters");
        }
    }
}
