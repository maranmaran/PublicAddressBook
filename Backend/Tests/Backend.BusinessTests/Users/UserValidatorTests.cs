using Backend.Business.Users.UsersRequests.CreateUser;
using Backend.Business.Users.UsersRequests.UpdateUser;
using Backend.Domain;
using Backend.Domain.Entities.User;
using Backend.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

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

        #region CreateUserValidator
        [Fact]
        public async Task CreateUserValidator_ValidRequest_Valid()
        {
            var validator = new CreateUserRequestValidator(_context);
            var result = validator.Validate(new CreateUserRequest()
            {
                Username = "username3",
                AccountType = AccountType.User,
                Email = "username3@mail.com",
                FirstName = "name",
                LastName = "name",
                Gender = Gender.Female
            });

            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task CreateUserValidator_UsernameNotUnique_Invalid()
        {
            var validator = new CreateUserRequestValidator(_context);
            var result = validator.Validate(new CreateUserRequest()
            {
                Username = "username",
                AccountType = AccountType.User,
                Email = "username3@mail.com",
                FirstName = "name",
                LastName = "name",
                Gender = Gender.Female
            });

            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task CreateUserValidator_UsernameMaxLength_Invalid()
        {
            var validator = new CreateUserRequestValidator(_context);
            var result = validator.Validate(new CreateUserRequest()
            {
                Username = "usernameusernameusernameusernameusernameusername",
                AccountType = AccountType.User,
                Email = "username3@mail.com",
                FirstName = "name",
                LastName = "name",
                Gender = Gender.Female
            });

            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task CreateUserValidator_UsernameNullEmptyWhitespaceMaxLength_Invalid()
        {
            var validator = new CreateUserRequestValidator(_context);
            var result = validator.Validate(new CreateUserRequest()
            {
                Username = null,
                AccountType = AccountType.User,
                Email = "username3@mail.com",
                FirstName = "name",
                LastName = "name",
                Gender = Gender.Female
            });
            var result2 = validator.Validate(new CreateUserRequest()
            {
                Username = string.Empty,
                AccountType = AccountType.User,
                Email = "username3@mail.com",
                FirstName = "name",
                LastName = "name",
                Gender = Gender.Female
            });
            var result3 = validator.Validate(new CreateUserRequest()
            {
                Username = " ",
                AccountType = AccountType.User,
                Email = "username3@mail.com",
                FirstName = "name",
                LastName = "name",
                Gender = Gender.Female
            });
            var result4 = validator.Validate(new CreateUserRequest()
            {
                Username = "username3username3username3username3username3username3username3username3",
                AccountType = AccountType.User,
                Email = "username3@mail.com",
                FirstName = "name",
                LastName = "name",
                Gender = Gender.Female
            });

            Assert.False(result.IsValid);
            Assert.False(result2.IsValid);
            Assert.False(result3.IsValid);
            Assert.False(result4.IsValid);
        }

        [Fact]
        public async Task CreateUserValidator_FirstNameNullEmptyWhitespaceMaxLength_Invalid()
        {
            var validator = new CreateUserRequestValidator(_context);
            var result = validator.Validate(new CreateUserRequest()
            {
                Username = "username",
                AccountType = AccountType.User,
                Email = "username3@mail.com",
                FirstName = string.Empty,
                LastName = "name",
                Gender = Gender.Female
            });
            var result2 = validator.Validate(new CreateUserRequest()
            {
                Username = "username",
                AccountType = AccountType.User,
                Email = "username3@mail.com",
                FirstName = null,
                LastName = "name",
                Gender = Gender.Female
            });
            var result3 = validator.Validate(new CreateUserRequest()
            {
                Username = "username",
                AccountType = AccountType.User,
                Email = "username3@mail.com",
                FirstName = " ",
                LastName = "name",
                Gender = Gender.Female
            });
            var result4 = validator.Validate(new CreateUserRequest()
            {
                Username = "username",
                AccountType = AccountType.User,
                Email = "username3@mail.com",
                FirstName = "firstnamefirstnamefirstnamefirstnamefirstnamefirstnamefirstname",
                LastName = "name",
                Gender = Gender.Female
            });

            Assert.False(result.IsValid);
            Assert.False(result2.IsValid);
            Assert.False(result3.IsValid);
            Assert.False(result4.IsValid);
        }

        [Fact]
        public async Task CreateUserValidator_LastNameNullEmptyWhitespace_Invalid()
        {
            var validator = new CreateUserRequestValidator(_context);
            var result = validator.Validate(new CreateUserRequest()
            {
                Username = "username",
                AccountType = AccountType.User,
                Email = "username3@mail.com",
                FirstName = "firstname",
                LastName = string.Empty,
                Gender = Gender.Female
            });
            var result2 = validator.Validate(new CreateUserRequest()
            {
                Username = "username",
                AccountType = AccountType.User,
                Email = "username3@mail.com",
                FirstName = "firstname",
                LastName = null,
                Gender = Gender.Female
            });
            var result3 = validator.Validate(new CreateUserRequest()
            {
                Username = "username",
                AccountType = AccountType.User,
                Email = "username3@mail.com",
                FirstName = "firstname",
                LastName = " ",
                Gender = Gender.Female
            });
            var result4 = validator.Validate(new CreateUserRequest()
            {
                Username = "username",
                AccountType = AccountType.User,
                Email = "username3@mail.com",
                FirstName = "firstname",
                LastName = "namenamenamenamenamenamenamenamenamenamenamenamenamenamenamename",
                Gender = Gender.Female
            });

            Assert.False(result.IsValid);
            Assert.False(result2.IsValid);
            Assert.False(result3.IsValid);
            Assert.False(result4.IsValid);
        }

        [Fact]
        public async Task CreateUserValidator_UsernameUnique_Invalid()
        {
            var validator = new CreateUserRequestValidator(_context);
            var result = validator.Validate(new CreateUserRequest()
            {
                Username = "username",
                AccountType = AccountType.User,
                Email = "username3@mail.com",
                FirstName = "name",
                LastName = "name",
                Gender = Gender.Female
            });

            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task CreateUserValidator_EmailUnique_Invalid()
        {
            var validator = new CreateUserRequestValidator(_context);
            var result = validator.Validate(new CreateUserRequest()
            {
                Username = "username3",
                AccountType = AccountType.User,
                Email = "username@mail.com",
                FirstName = "name",
                LastName = "name",
                Gender = Gender.Female
            });

            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task CreateUserValidator_EmailIsEmail_Invalid()
        {
            var validator = new CreateUserRequestValidator(_context);
            var result = validator.Validate(new CreateUserRequest()
            {
                Username = "username3",
                AccountType = AccountType.User,
                Email = "username.com",
                FirstName = "name",
                LastName = "name",
                Gender = Gender.Female
            });

            Assert.False(result.IsValid);
        }
        #endregion

        #region UpdateUserValidator
        [Fact]
        public async Task UpdateUserValidator_ValidRequest_Valid()
        {
            var validator = new UpdateUserRequestValidator(_context);
            var result = validator.Validate(new UpdateUserRequest()
            {
                Id = Guid.Parse("601fe5f2-a47b-48f2-bb18-089e855a2992"),
                Username = "username3",
                AccountType = AccountType.User,
                Email = "username3@mail.com",
                FirstName = "name",
                LastName = "name",
                Gender = Gender.Female
            });

            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task UpdateUserValidator_UsernameNotUnique_Invalid()
        {
            var validator = new UpdateUserRequestValidator(_context);
            var result = validator.Validate(new UpdateUserRequest()
            {
                Id = Guid.Parse("601fe5f2-a47b-48f2-bb18-089e855a2992"),
                Username = "username",
                AccountType = AccountType.User,
                Email = "username3@mail.com",
                FirstName = "name",
                LastName = "name",
                Gender = Gender.Female
            });

            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task UpdateUserValidator_UsernameMaxLength_Invalid()
        {
            var validator = new UpdateUserRequestValidator(_context);
            var result = validator.Validate(new UpdateUserRequest()
            {
                Id = Guid.Parse("601fe5f2-a47b-48f2-bb18-089e855a2992"),
                Username = "usernameusernameusernameusernameusernameusername",
                AccountType = AccountType.User,
                Email = "username3@mail.com",
                FirstName = "name",
                LastName = "name",
                Gender = Gender.Female
            });

            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task UpdateUserValidator_UsernameNullEmptyWhitespaceMaxLength_Invalid()
        {
            var validator = new UpdateUserRequestValidator(_context);
            var result = validator.Validate(new UpdateUserRequest()
            {
                Id = Guid.Parse("601fe5f2-a47b-48f2-bb18-089e855a2992"),
                Username = null,
                AccountType = AccountType.User,
                Email = "username3@mail.com",
                FirstName = "name",
                LastName = "name",
                Gender = Gender.Female
            });
            var result2 = validator.Validate(new UpdateUserRequest()
            {
                Id = Guid.Parse("601fe5f2-a47b-48f2-bb18-089e855a2992"),
                Username = string.Empty,
                AccountType = AccountType.User,
                Email = "username3@mail.com",
                FirstName = "name",
                LastName = "name",
                Gender = Gender.Female
            });
            var result3 = validator.Validate(new UpdateUserRequest()
            {
                Id = Guid.Parse("601fe5f2-a47b-48f2-bb18-089e855a2992"),
                Username = " ",
                AccountType = AccountType.User,
                Email = "username3@mail.com",
                FirstName = "name",
                LastName = "name",
                Gender = Gender.Female
            });
            var result4 = validator.Validate(new UpdateUserRequest()
            {
                Id = Guid.Parse("601fe5f2-a47b-48f2-bb18-089e855a2992"),
                Username = "username3username3username3username3username3username3username3username3",
                AccountType = AccountType.User,
                Email = "username3@mail.com",
                FirstName = "name",
                LastName = "name",
                Gender = Gender.Female
            });

            Assert.False(result.IsValid);
            Assert.False(result2.IsValid);
            Assert.False(result3.IsValid);
            Assert.False(result4.IsValid);
        }

        [Fact]
        public async Task UpdateUserValidator_FirstNameNullEmptyWhitespaceMaxLength_Invalid()
        {
            var validator = new UpdateUserRequestValidator(_context);
            var result = validator.Validate(new UpdateUserRequest()
            {
                Id = Guid.Parse("601fe5f2-a47b-48f2-bb18-089e855a2992"),
                Username = "username",
                AccountType = AccountType.User,
                Email = "username3@mail.com",
                FirstName = string.Empty,
                LastName = "name",
                Gender = Gender.Female
            });
            var result2 = validator.Validate(new UpdateUserRequest()
            {
                Id = Guid.Parse("601fe5f2-a47b-48f2-bb18-089e855a2992"),
                Username = "username",
                AccountType = AccountType.User,
                Email = "username3@mail.com",
                FirstName = null,
                LastName = "name",
                Gender = Gender.Female
            });
            var result3 = validator.Validate(new UpdateUserRequest()
            {
                Id = Guid.Parse("601fe5f2-a47b-48f2-bb18-089e855a2992"),
                Username = "username",
                AccountType = AccountType.User,
                Email = "username3@mail.com",
                FirstName = " ",
                LastName = "name",
                Gender = Gender.Female
            });
            var result4 = validator.Validate(new UpdateUserRequest()
            {
                Id = Guid.Parse("601fe5f2-a47b-48f2-bb18-089e855a2992"),
                Username = "username",
                AccountType = AccountType.User,
                Email = "username3@mail.com",
                FirstName = "firstnamefirstnamefirstnamefirstnamefirstnamefirstnamefirstname",
                LastName = "name",
                Gender = Gender.Female
            });

            Assert.False(result.IsValid);
            Assert.False(result2.IsValid);
            Assert.False(result3.IsValid);
            Assert.False(result4.IsValid);
        }

        [Fact]
        public async Task UpdateUserValidator_LastNameNullEmptyWhitespace_Invalid()
        {
            var validator = new UpdateUserRequestValidator(_context);
            var result = validator.Validate(new UpdateUserRequest()
            {
                Id = Guid.Parse("601fe5f2-a47b-48f2-bb18-089e855a2992"),
                Username = "username",
                AccountType = AccountType.User,
                Email = "username3@mail.com",
                FirstName = "firstname",
                LastName = string.Empty,
                Gender = Gender.Female
            });
            var result2 = validator.Validate(new UpdateUserRequest()
            {
                Id = Guid.Parse("601fe5f2-a47b-48f2-bb18-089e855a2992"),
                Username = "username",
                AccountType = AccountType.User,
                Email = "username3@mail.com",
                FirstName = "firstname",
                LastName = null,
                Gender = Gender.Female
            });
            var result3 = validator.Validate(new UpdateUserRequest()
            {
                Id = Guid.Parse("601fe5f2-a47b-48f2-bb18-089e855a2992"),
                Username = "username",
                AccountType = AccountType.User,
                Email = "username3@mail.com",
                FirstName = "firstname",
                LastName = " ",
                Gender = Gender.Female
            });
            var result4 = validator.Validate(new UpdateUserRequest()
            {
                Id = Guid.Parse("601fe5f2-a47b-48f2-bb18-089e855a2992"),
                Username = "username",
                AccountType = AccountType.User,
                Email = "username3@mail.com",
                FirstName = "firstname",
                LastName = "namenamenamenamenamenamenamenamenamenamenamenamenamenamenamename",
                Gender = Gender.Female
            });

            Assert.False(result.IsValid);
            Assert.False(result2.IsValid);
            Assert.False(result3.IsValid);
            Assert.False(result4.IsValid);
        }

        [Fact]
        public async Task UpdateUserValidator_UsernameUnique_Invalid()
        {
            var validator = new UpdateUserRequestValidator(_context);
            var result = validator.Validate(new UpdateUserRequest()
            {
                Id = Guid.Parse("601fe5f2-a47b-48f2-bb18-089e855a2992"),
                Username = "username",
                AccountType = AccountType.User,
                Email = "username3@mail.com",
                FirstName = "name",
                LastName = "name",
                Gender = Gender.Female
            });

            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task UpdateUserValidator_EmailUnique_Invalid()
        {
            var validator = new UpdateUserRequestValidator(_context);
            var result = validator.Validate(new UpdateUserRequest()
            {
                Id = Guid.Parse("601fe5f2-a47b-48f2-bb18-089e855a2992"),
                Username = "username3",
                AccountType = AccountType.User,
                Email = "username@mail.com",
                FirstName = "name",
                LastName = "name",
                Gender = Gender.Female
            });

            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task UpdateUserValidator_IdEmptyOrNotFound_Invalid()
        {
            var validator = new UpdateUserRequestValidator(_context);
            var result2 = validator.Validate(new UpdateUserRequest()
            {
                Id = Guid.Empty,
                Username = "username3",
                AccountType = AccountType.User,
                Email = "username@mail.com",
                FirstName = "name",
                LastName = "name",
                Gender = Gender.Female
            });

            Assert.False(result2.IsValid);
        }

        [Fact]
        public async Task UpdateUserValidator_EmailIsEmail_Invalid()
        {
            var validator = new UpdateUserRequestValidator(_context);
            var result = validator.Validate(new UpdateUserRequest()
            {
                Id = Guid.Parse("601fe5f2-a47b-48f2-bb18-089e855a2992"),
                Username = "username3",
                AccountType = AccountType.User,
                Email = "username.com",
                FirstName = "name",
                LastName = "name",
                Gender = Gender.Female
            });

            Assert.False(result.IsValid);
        }
        #endregion

    }
}
