using Backend.Domain.Entities.User;
using Backend.Domain.Enum;
using MediatR;
using System.Collections.Generic;

namespace Backend.Business.Users.UsersRequests.GetAllUsers
{
    public class GetAllUsersRequest : IRequest<IEnumerable<ApplicationUser>>
    {
        public AccountType AccountType { get; set; }

        public GetAllUsersRequest(AccountType accountType)
        {
            AccountType = accountType;
        }
    }
}