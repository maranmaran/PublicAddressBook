using Backend.Domain.Entities.User;
using MediatR;

namespace Backend.Business.Email.EmailRequests.ResetPassword
{
    public class ResetPasswordEmailRequest : IRequest<Unit>
    {
        public ApplicationUser User { get; set; }
        public ResetPasswordEmailRequest(ApplicationUser user)
        {
            User = user;
        }
    }
}
