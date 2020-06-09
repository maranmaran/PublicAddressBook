using AutoMapper;
using Backend.Business.Contacts.Requests.Update;
using Backend.Domain;
using Backend.Domain.Entities.Contacts;
using Backend.Infrastructure.Exceptions;
using Backend.Library.Logging.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Backend.BusinessTests.Contacts
{

    public class UpdateContactRequestTests
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateContactRequestTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UpdateContactRequest, Contact>().ForMember(x => x.PhoneNumbers, opt => opt.Ignore());
                cfg.CreateMap<UpdateContactRequest, Contact>().ForMember(x => x.PhoneNumbers, opt => opt.Ignore());
            });
            _mapper = config.CreateMapper();

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
        public async Task UpdateContactHandler_ValidRequestWithNumbers_Updates()
        {
            var logMock = new Mock<ILoggingService>();
            var handler = new UpdateContactRequestHandler(_context, _mapper, logMock.Object);

            var request = new UpdateContactRequest()
            {
                Id = Guid.Parse("d5535764-d39d-42bf-9b84-429b72a6e902"),
                PhoneNumbers = new List<string>() { "Number3", "Number4" },
                Address = "AddressUpdated",
                Name = "NameUpdated"
            };

            var result = await handler.Handle(request, CancellationToken.None);
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(2, result.PhoneNumbers.Count);
            Assert.Equal("Number3", result.PhoneNumbers.ElementAt(0).Number);

            var contact = await _context.Contacts.FirstOrDefaultAsync(x => x.Id == result.Id, CancellationToken.None);
            Assert.NotNull(contact);
            Assert.Equal(contact.Address, result.Address);
            Assert.Equal(contact.Name, result.Name);
        }

        [Fact]
        public async Task UpdateContactHandler_ValidRequestWithoutNumbers_Updates()
        {
            var logMock = new Mock<ILoggingService>();
            var handler = new UpdateContactRequestHandler(_context, _mapper, logMock.Object);

            var request = new UpdateContactRequest()
            {
                Id = Guid.Parse("d5535764-d39d-42bf-9b84-429b72a6e902"),
                PhoneNumbers = new List<string>() { },
                Address = "Address",
                Name = "Name"
            };

            var result = await handler.Handle(request, CancellationToken.None);
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(0, result.PhoneNumbers?.Count);

            var contact = await _context.Contacts.FirstOrDefaultAsync(x => x.Id == result.Id, CancellationToken.None);
            Assert.NotNull(contact);
        }

        [Fact]
        public async Task UpdateContactHandler_MappingFails_Fails()
        {
            var logMock = new Mock<ILoggingService>();
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map(It.IsAny<UpdateContactRequest>(), It.IsAny<Contact>())).Throws(new Exception());

            var handler = new UpdateContactRequestHandler(_context, mapperMock.Object, logMock.Object);

            var request = new UpdateContactRequest()
            {
                Id = Guid.Parse("d5535764-d39d-42bf-9b84-429b72a6e902"),
                PhoneNumbers = new List<string>() { "Number3", "Number4" },
                Address = "Address",
                Name = "Name"
            };

            await Assert.ThrowsAsync<UpdateFailureException>(() => handler.Handle(request, CancellationToken.None));
            logMock.Verify(x => x.LogInfo(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
            logMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateContactHandler_ContactNotFound_Fails()
        {
            var logMock = new Mock<ILoggingService>();

            var handler = new UpdateContactRequestHandler(_context, _mapper, logMock.Object);

            var request = new UpdateContactRequest()
            {
                PhoneNumbers = new List<string>() { "Number3", "Number4" },
                Address = "Address",
                Name = "Name"
            };

            await Assert.ThrowsAsync<UpdateFailureException>(() => handler.Handle(request, CancellationToken.None));
            logMock.Verify(x => x.LogInfo(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
            logMock.VerifyNoOtherCalls();
        }
    }
}