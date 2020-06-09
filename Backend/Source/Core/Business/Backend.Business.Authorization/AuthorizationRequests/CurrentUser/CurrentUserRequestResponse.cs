using Backend.Domain.Entities.User;
using System;
using AccountType = Backend.Domain.Enum.AccountType;

namespace Backend.Business.Authorization.AuthorizationRequests.CurrentUser
{
    public class CurrentUserRequestResponse
    {
        // basic account info
        public Guid Id { get; set; }

        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; }
        public string Avatar { get; set; }

        public AccountType AccountType { get; set; }
        public bool Active { get; set; }
        public UserSetting UserSetting { get; set; }

    }
}