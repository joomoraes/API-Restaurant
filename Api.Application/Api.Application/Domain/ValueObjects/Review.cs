namespace Api.Application.Domain.ValueObjects
{
    using FluentValidation;
    using FluentValidation.Results;

    public class Review : AbstractValidator<Review>
    {
        public Review(int starts, string comments)
        {
            Starts = starts;
            Comments = comments;
        }

        public int Starts { get; private set; }
        public string Comments { get; private set; }
        public ValidationResult ValidationResult { get; set; }

        public virtual bool _Validate()
        {
            ValidateStarts();
            ValidateComments();

            ValidationResult = Validate(this);

            return ValidationResult.IsValid;
        }

        private void ValidateStarts()
        {
            RuleFor(c => c.Starts)
                .GreaterThan(0).WithMessage("Number of starts begone then zero")
                .LessThanOrEqualTo(5).WithMessage("Number of start begone less or equal five");
        }

        public void ValidateComments()
        {
            RuleFor(c => c.Comments)
                .NotEmpty().WithMessage("Comments can't be empty")
                .MaximumLength(100).WithMessage("Comments can be max 100 character");
        }
    }
}
