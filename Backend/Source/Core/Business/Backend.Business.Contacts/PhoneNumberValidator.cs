using Backend.Domain.Entities.Contacts;
using FluentValidation;

namespace Backend.Business.Contacts
{
    public class PhoneNumberValidator : AbstractValidator<PhoneNumber>
    {
        public PhoneNumberValidator()
        {
            RuleFor(x => x.Number)
                .NotNull()
                .NotEmpty()
                .NotEqual(" ");
        }
    }
}