using FluentValidation;

namespace Domain.Validators
{
    public class GoldenRaspberryCSVValidator : AbstractValidator<GoldenRaspberryCSV>
    {
        public GoldenRaspberryCSVValidator()
        {
            RuleFor(x => x.Year).NotEmpty().NotNull();
            RuleFor(x => x.Title).NotEmpty().NotNull();
            RuleFor(x => x.Studio).NotEmpty().NotNull();
            RuleFor(x => x.Producer).NotEmpty().NotNull();
        }
    }
}