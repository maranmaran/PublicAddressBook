using Backend.Business.Authorization.Utils;
using Backend.Domain;
using FluentValidation;
using System.Linq;

namespace Backend.Business.Authorization.AuthorizationRequests.SignIn
{
    public class SignInRequestValidator : AbstractValidator<SignInRequest>
    {
        private readonly IApplicationDbContext _context;

        public SignInRequestValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Username)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty();

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .MinimumLength(4);

            RuleFor(x => x)
                .Must(BeValidUser).WithMessage($"User validation failed");
        }

        private bool BeValidUser(SignInRequest request)
        {
            var user = _context.Users.FirstOrDefault(x => x.Username == request.Username);

            // user must exist
            if (user == null)
                return false;

            // user must be active
            if (user.Active == false)
                return false;

            // user password and request password must match
            var requestPasswordHash = PasswordHasher.GetPasswordHash(request.Password);
            if (user.PasswordHash != requestPasswordHash)
                return false;

            // everything is valid
            return true;
        }
    }
}