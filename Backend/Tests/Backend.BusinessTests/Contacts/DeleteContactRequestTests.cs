using Backend.Business.Contacts.Requests.Delete;
using Backend.Domain;
using Backend.Domain.Entities.Contacts;
using Backend.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Backend.BusinessTests.Contacts
{
    public class DeleteContactRequestTests
    {
        private readonly IApplicationDbContext _context;

        public DeleteContactRequestTests()
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
        public async Task DeleteContactHandler_ValidRequestWithNumbers_Deletes()
        {
            var handler = new DeleteContactRequestHandler(_context);

            var request = new DeleteContactRequest(Guid.Parse("d5535764-d39d-42bf-9b84-429b72a6e902"));

            await handler.Handle(request, CancellationToken.None);

            var contact = await _context.Contacts.FirstOrDefaultAsync(x => x.Id == request.Id, CancellationToken.None);
            Assert.Null(contact);
        }


        [Fact]
        public async Task DeleteContactHandler_ContactNotFound_Fails()
        {
            var handler = new DeleteContactRequestHandler(_context);

            var request = new DeleteContactRequest(Guid.Empty);

            await Assert.ThrowsAsync<DeleteFailureException>(() => handler.Handle(request, CancellationToken.None));
        }
    }
}