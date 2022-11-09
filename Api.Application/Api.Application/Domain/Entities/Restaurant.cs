namespace Api.Application.Domain.Entities
{

    using Api.Application.Domain.Enums;
    using Api.Application.Domain.ValueObjects;
    using FluentValidation;
    using FluentValidation.Results;

    public class Restaurant : AbstractValidator<Restaurant>
    {

        public Restaurant(string _Name, EKitchen _Kitchen)
        {
            Name = _Name;
            Kitchen = _Kitchen;
        }

        public Restaurant(string _Id, string _Name, EKitchen _Kitchen)
        {
            Id = _Id;
            Name = _Name;
            Kitchen = _Kitchen;
        }

        public string Id { get; private set; }
        public string Name { get; private set; } 
        public EKitchen Kitchen { get; private set; }
        public  Address Address { get; private set; }

        public ValidationResult ValidationResult { get; set; }

        public void AtributeAddress(Address address)
        {
            Address = address;
        }

        public virtual bool _Validate()
        {
            ValidateName();
            ValidationResult = Validate(this);

            ValidateAddress();

            return ValidationResult.IsValid;
        }

        private void ValidateName()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Name can't be empty")
                .MaximumLength(30).WithMessage("Name can take maximum 30 characters");
        }

        private void ValidateAddress()
        {
            if (Address._Validate())
                return;

            foreach (var erro in Address.ValidationResult.Errors)
                ValidationResult.Errors.Add(erro);
        }

    }
}
