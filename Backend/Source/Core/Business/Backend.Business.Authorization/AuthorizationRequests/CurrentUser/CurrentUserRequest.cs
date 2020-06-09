using MediatR;
using System;

namespace Backend.Business.Authorization.AuthorizationRequests.CurrentUser
{
    public class CurrentUserRequest : IRequest<CurrentUserRequestResponse>
    {
        public Guid UserId { get; set; }

        public CurrentUserRequest(Guid userId)
        {
            UserId = userId;
        }
    }
}