using Backend.Domain;
using FluentValidation;
using System.Linq;

namespace Backend.Business.Users.UsersRequests.UpdateUser
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        private readonly IApplicationDbContext _context;

        public UpdateUserRequestValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Id)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty();

            RuleFor(x => x.Username)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .NotEmpty()
                .NotEqual(" ")
                .MaximumLength(30)
                .Must(UniqueUsername)
                .WithMessage("Username must be unique");

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .NotEmpty()
                .NotEqual(" ")
                .EmailAddress()
                .Must(UniqueEmail)
                .WithMessage("Email must be unique");

            RuleFor(x => x.FirstName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .NotEmpty()
                .NotEqual(" ")
                .MaximumLength(30);

            RuleFor(x => x.LastName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .MaximumLength(30);
        }

        private bool UniqueUsername(UpdateUserRequest request, string username)
        {
            var user = _context.Users.FirstOrDefault(x => x.Username == username && x.Id != request.Id);
            return user == null; // no user with that username
        }

        private bool UniqueEmail(UpdateUserRequest request, string email)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == email && x.Id != request.Id);
            return user == null; // no user with that email
        }
    }
}