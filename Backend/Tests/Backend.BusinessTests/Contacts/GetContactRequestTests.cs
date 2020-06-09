using Backend.Business.Contacts.Requests.Get;
using Backend.Domain;
using Backend.Domain.Entities.Contacts;
using Backend.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Backend.BusinessTests.Contacts
{
    public class GetContactRequestTests
    {
        private readonly IApplicationDbContext _context;

        public GetContactRequestTests()
        {
            _context = TestHelper.GetContext();

            var contacts = new List<Contact>()
            {
                new Contact()
                {
                    Id = Guid.Parse("d5535764-d39d-42bf-9b84-429b72a6e902"),
                    Name = "Contact1",
                    Address = "Address1",
                    PhoneNumbers = new List<PhoneNumber>()
                    {
                        new PhoneNumber()
                        {
                            Number = "Number1",
                            Id = Guid.Parse("92d55817-9618-4fc3-ab93-7759307df904")
                        }
                    }
                },
            };

            _context.Contacts.AddRange(contacts);
            _context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        [Fact]
        public async Task GetContactHandler_ValidRequest_Gets()
        {
            var handler = new GetContactRequestHandler(_context);

            var request = new GetContactRequest(Guid.Parse("d5535764-d39d-42bf-9b84-429b72a6e902"));

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(Guid.Parse("d5535764-d39d-42bf-9b84-429b72a6e902"), result.Id);
            Assert.Equal("Contact1", result.Name);
            Assert.Equal("Address1", result.Address);
            Assert.Equal(1, result.PhoneNumbers?.Count);
            Assert.Equal("Number1", result.PhoneNumbers.ElementAt(0).Number);
        }


        [Fact]
        public async Task GetContactHandler_ContactNotFound_Fails()
        {
            var handler = new GetContactRequestHandler(_context);

            var request = new GetContactRequest(Guid.Empty);

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(request, CancellationToken.None));
        }
    }
}