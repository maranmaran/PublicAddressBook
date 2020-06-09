using Backend.Domain;
using Backend.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Backend.BusinessTests.Users
{
    public class UserValidatorTests
    {
        private readonly IApplicationDbContext _context;
        public UserValidatorTests()
        {
            _context = TestHelper.GetContext();
            var users = new List<ApplicationUser>()
            {
                new ApplicationUser()
                {
                    Id = Guid.Parse("601fe5f2-a47b-48f2-bb18-089e855a2992"),
                    Username = "username",
                    PasswordHash = "password",
                    Email = "username@mail.com"
                },
                new ApplicationUser()
                {
                    Id = Guid.Parse("601fe5f2-a47b-48f2-bb18-089e855a2993"),
                    Username = "username2",
                    PasswordHash = "password2",
                    Email = "username2@mail.com"
                }
            };

            _context.Users.AddRange(users);
            _context.SaveChangesAsync(CancellationToken.None).Wait();
        }

    }
}
