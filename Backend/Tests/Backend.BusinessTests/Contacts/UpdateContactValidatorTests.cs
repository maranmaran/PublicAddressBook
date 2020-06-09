using Backend.Business.Contacts.Requests.Update;
using Backend.Domain.Entities.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Backend.BusinessTests.Contacts
{
    public class UpdateContactValidatorTests
    {
        [Fact]
        public async Task UpdateContactValidator_ValidRequest_Valid()
        {
            var context = TestHelper.GetContext();

            var validator = new UpdateContactRequestValidator(context);
            var result = validator.Validate(new UpdateContactRequest()
            {
                Id = Guid.NewGuid(),
                Address = "Address",
                Name = "Name",
                PhoneNumbers = new List<string>() { "Number", "Number2" }
            });

            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task UpdateContactValidator_ContactNotUnique_Invalid()
        {
            var context = TestHelper.GetContext();
            context.Contacts.Add(new Contact() { Address = "Address", Name = "Name", Id = Guid.NewGuid() });
            context.SaveChangesAsync(CancellationToken.None).Wait();

            var validator = new UpdateContactRequestValidator(context);
            var result = validator.Validate(new UpdateContactRequest
            {
                Id = Guid.NewGuid(),
                Address = "Address",
                Name = "Name",
                PhoneNumbers = new List<string>() { "Number", "Number2" }
            });

            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task UpdateContactValidator_NameMaxLengthNotNullNotEmptyNotWhitespace_Invalid()
        {
            var context = TestHelper.GetContext();
            var validator = new UpdateContactRequestValidator(context);
            var result = validator.Validate(new UpdateContactRequest()
            {
                Id = Guid.NewGuid(),
                Address = "Address",
                Name = "",
                PhoneNumbers = new List<string>() { "Number", "Number2" }
            });
            var result1 = validator.Validate(new UpdateContactRequest()
            {
                Id = Guid.NewGuid(),
                Address = "Address",
                Name = " ",
                PhoneNumbers = new List<string>() { "Number", "Number2" }
            });
            var result2 = validator.Validate(new UpdateContactRequest()
            {
                Id = Guid.NewGuid(),
                Address = "Address",
                Name = null,
                PhoneNumbers = new List<string>() { "Number", "Number2" }
            });
            var result3 = validator.Validate(new UpdateContactRequest()
            {
                Id = Guid.NewGuid(),
                Address = "Address",
                Name = new string('n', 201),
                PhoneNumbers = new List<string>() { "Number", "Number2" }
            });


            Assert.False(result.IsValid);
            Assert.False(result1.IsValid);
            Assert.False(result2.IsValid);
            Assert.False(result3.IsValid);
            Assert.Equal("CONTACT.VALIDATION_ERRORS.INVALID_NAME", result.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task UpdateContactValidator_AddressMaxLengthNotNullNotEmptyNotWhitespace_Invalid()
        {
            var context = TestHelper.GetContext();
            var validator = new UpdateContactRequestValidator(context);
            var result = validator.Validate(new UpdateContactRequest()
            {
                Id = Guid.NewGuid(),
                Address = "",
                Name = "Name",
                PhoneNumbers = new List<string>() { "Number", "Number2" }
            });
            var result1 = validator.Validate(new UpdateContactRequest()
            {
                Id = Guid.NewGuid(),
                Address = null,
                Name = "Name",
                PhoneNumbers = new List<string>() { "Number", "Number2" }
            });
            var result2 = validator.Validate(new UpdateContactRequest()
            {

                Id = Guid.NewGuid(),
                Address = " ",
                Name = "Name",
                PhoneNumbers = new List<string>() { "Number", "Number2" }
            });
            var result3 = validator.Validate(new UpdateContactRequest()
            {
                Id = Guid.NewGuid(),
                Address = new string('a', 101),
                Name = "Name",
                PhoneNumbers = new List<string>() { "Number", "Number2" }
            });


            Assert.False(result.IsValid);
            Assert.False(result1.IsValid);
            Assert.False(result2.IsValid);
            Assert.False(result3.IsValid);
            Assert.Equal("CONTACT.VALIDATION_ERRORS.INVALID_ADDRESS", result.Errors.First().ErrorMessage);
        }

        [Fact]
        public async Task UpdateContactValidator_PhoneNumberMaxLengthNotNullNotEmptyNotWhitespace_Invalid()
        {
            var context = TestHelper.GetContext();
            var validator = new UpdateContactRequestValidator(context);
            var result = validator.Validate(new UpdateContactRequest()
            {
                Id = Guid.NewGuid(),
                Address = "Address",
                Name = "Name",
                PhoneNumbers = new List<string>() { "", " ", null, new string('p', 51) }
            });


            Assert.False(result.IsValid);
            Assert.Equal("CONTACT.VALIDATION_ERRORS.INVALID_PHONE_NUMBER", result.Errors.First().ErrorMessage);
        }


        [Fact]
        public async Task UpdateContactValidator_NotUniqueContact_Invalid()
        {
            var context = TestHelper.GetContext();
            context.Contacts.Add(new Contact()
            {
                Id = Guid.NewGuid(),
                Address = "Address",
                Name = "Name",
            });
            context.SaveChangesAsync(CancellationToken.None).Wait();

            var validator = new UpdateContactRequestValidator(context);
            var result = validator.Validate(new UpdateContactRequest()
            {
                Id = Guid.NewGuid(),
                Address = "Address",
                Name = "Name",
            });

            Assert.False(result.IsValid);
            Assert.Equal("CONTACT.VALIDATION_ERRORS.NOT_UNIQUE", result.Errors.First().ErrorMessage);
            Assert.Equal("Unique", result.Errors.First().PropertyName);
        }


        [Fact]
        public async Task UpdateContactValidator_EmptyId_Invalid()
        {
            var context = TestHelper.GetContext();
            context.SaveChangesAsync(CancellationToken.None).Wait();

            var validator = new UpdateContactRequestValidator(context);
            var result = validator.Validate(new UpdateContactRequest()
            {
                Id = Guid.Empty,
                Address = "Address",
                Name = "Name",
            });

            Assert.False(result.IsValid);
            Assert.Equal("Id", result.Errors.First().PropertyName);
        }
    }
}