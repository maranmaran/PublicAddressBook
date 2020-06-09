using AutoMapper;
using Backend.Business.Contacts.Requests.Create;
using Backend.Domain;
using Backend.Domain.Entities.Contacts;
using Backend.Infrastructure.Exceptions;
using Backend.Library.Logging.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Backend.BusinessTests.Contacts
{
    public class CreateContactRequestTests
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateContactRequestTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateContactRequest, Contact>().ForMember(x => x.PhoneNumbers, opt => opt.Ignore());
            });
            _mapper = config.CreateMapper();

            _context = TestHelper.GetContext();
        }

        [Fact]
        public async Task CreateContactHandler_ValidRequestWithNumbers_Creates()
        {
            var logMock = new Mock<ILoggingService>();
            var handler = new CreateContactRequestHandler(_context, _mapper, logMock.Object);

            var request = new CreateContactRequest()
            {
                PhoneNumbers = new List<string>() { "Number3", "Number4" },
                Address = "Address",
                Name = "Name"
            };

            var result = await handler.Handle(request, CancellationToken.None);
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(2, result.PhoneNumbers.Count);

            var contact = await _context.Contacts.FirstOrDefaultAsync(x => x.Id == result.Id, CancellationToken.None);
            Assert.NotNull(contact);
        }

        [Fact]
        public async Task CreateContactHandler_ValidRequestWithoutNumbers_Creates()
        {
            var logMock = new Mock<ILoggingService>();
            var handler = new CreateContactRequestHandler(_context, _mapper, logMock.Object);

            var request = new CreateContactRequest()
            {
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
        public async Task CreateContactHandler_InvalidData_Fails()
        {
            var logMock = new Mock<ILoggingService>();
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map(It.IsAny<CreateContactRequest>(), It.IsAny<Contact>())).Throws(new Exception());

            var handler = new CreateContactRequestHandler(_context, mapperMock.Object, logMock.Object);

            var request = new CreateContactRequest()
            {
                PhoneNumbers = new List<string>() { "Number3", "Number4" },
                Address = "Address",
                Name = "Name"
            };

            await Assert.ThrowsAsync<CreateFailureException>(() => handler.Handle(request, CancellationToken.None));
            logMock.Verify(x => x.LogInfo(It.IsAny<Exception>(), It.IsAny<string>()), Times.Once);
            logMock.VerifyNoOtherCalls();
        }
    }
}
