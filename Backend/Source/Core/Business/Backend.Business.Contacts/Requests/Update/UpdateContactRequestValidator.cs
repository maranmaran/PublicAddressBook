using Backend.Domain;
using FluentValidation;
using System.Linq;

namespace Backend.Business.Contacts.Requests.Update
{
    public class UpdateContactRequestValidator : AbstractValidator<UpdateContactRequest>
    {
        private readonly IApplicationDbContext _context;

        public UpdateContactRequestValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Address)
                .Transform(x => x?.Trim())
                .NotNull().WithMessage("CONTACT.VALIDATION_ERRORS.INVALID_ADDRESS")
                .NotEmpty().WithMessage("CONTACT.VALIDATION_ERRORS.INVALID_ADDRESS")
                .NotEqual(" ").WithMessage("CONTACT.VALIDATION_ERRORS.INVALID_ADDRESS");

            RuleFor(x => x.Name)
                .Transform(x => x?.Trim())
                .NotNull().WithMessage("CONTACT.VALIDATION_ERRORS.INVALID_NAME")
                .NotEmpty().WithMessage("CONTACT.VALIDATION_ERRORS.INVALID_NAME")
                .NotEqual(" ").WithMessage("CONTACT.VALIDATION_ERRORS.INVALID_NAME");

            RuleForEach(x => x.PhoneNumbers)
                .NotNull().WithMessage("CONTACT.VALIDATION_ERRORS.INVALID_PHONE_NUMBER")
                .NotEmpty().WithMessage("CONTACT.VALIDATION_ERRORS.INVALID_PHONE_NUMBER")
                .NotEqual(" ").WithMessage("CONTACT.VALIDATION_ERRORS.INVALID_PHONE_NUMBER");

            RuleFor(x => x)
                .Must(BeUnique).OverridePropertyName("Unique").WithMessage("CONTACT.VALIDATION_ERRORS.NOT_UNIQUE");
        }

        public bool BeUnique(UpdateContactRequest request)
        {
            var contact = _context.Contacts.FirstOrDefault(x => x.Id != request.Id && x.Name == request.Name && x.Address == request.Address);
            return contact == null;
        }
    }
}
