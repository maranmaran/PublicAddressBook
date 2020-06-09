using Backend.Business.Authorization.Utils;
using Backend.Domain.Entities.User;
using Backend.Domain.Enum;
using System;

namespace Backend.Persistence.Seed
{
    public static class UsersSeeder
    {
        public static (Admin, ApplicationUser) GetUsers()
        {
            var admin = new Admin()
            {
                Id = Guid.Parse("0faee6ac-1772-4bbe-9990-a7d9a22dd529"),
                FirstName = "Admin",
                LastName = "",
                Username = "admin",
                Email = "admin@trainingcompanion.com",
                Gender = Gender.Male,
                PasswordHash = PasswordHasher.GetPasswordHash("admin"),
                CreatedOn = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
                CustomerId = "cus_FLi7gZv8w0j0GB",
                UserSettingId = Guid.Parse("0d528a91-fbbe-4a02-924a-792344bbbd65")
            };
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
                CustomerId = "cus_FHk5RepADdfm5H",
                UserSettingId = Guid.Parse("8d399c00-5684-4a54-9c2c-b44a485c3583")
            };
            return (admin, user);
        }
    }
}