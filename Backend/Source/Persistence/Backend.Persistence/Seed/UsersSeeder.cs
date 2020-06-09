using Backend.Business.Authorization.Utils;
using Backend.Domain.Entities.User;
using Backend.Domain.Enum;
using System;

namespace Backend.Persistence.Seed
{
    public static class UsersSeeder
    {
        public static ApplicationUser GetUsers()
        {
            var user = new ApplicationUser()
            {
                Id = Guid.Parse("8d399c00-5654-4a54-9c2c-b44a485c3583"),
                FirstName = "Firstname",
                LastName = "Lastname",
                Username = "user",
                Email = "user@application.com",
                Gender = Gender.Male,
                PasswordHash = PasswordHasher.GetPasswordHash("user"),
                CreatedOn = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
                UserSettingId = Guid.Parse("8d399c00-5684-4a54-9c2c-b44a485c3583")
            };
            return user;
        }
    }
}