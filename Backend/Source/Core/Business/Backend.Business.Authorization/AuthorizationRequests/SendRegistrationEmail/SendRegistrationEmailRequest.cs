using Backend.Domain.Entities.User;
using MediatR;
using AccountType = Backend.Domain.Enum.AccountType;

namespace Backend.Business.Authorization.AuthorizationRequests.SendRegistrationEmail
{
    public class SendRegistrationEmailRequest : IRequest<Unit>
    {
        public AccountType AccountType { get; set; }

        public ApplicationUser User { get; set; }
        public SendRegistrationEmailRequest(ApplicationUser user)
        {
            User = user;
            AccountType = user.AccountType;
        }

    }
}