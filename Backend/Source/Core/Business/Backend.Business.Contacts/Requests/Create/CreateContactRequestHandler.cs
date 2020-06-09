using AutoMapper;
using Backend.Common.Extensions;
using Backend.Domain;
using Backend.Domain.Entities.Contacts;
using Backend.Infrastructure.Exceptions;
using Backend.Library.Logging.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Business.Contacts.Requests.Create
{
    public class CreateContactRequestHandler : IRequestHandler<CreateContactRequest, Contact>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILoggingService _logger;

        public CreateContactRequestHandler(IApplicationDbContext context,
            IMapper mapper,
            ILoggingService logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<Contact> Handle(CreateContactRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var contact = _mapper.Map<CreateContactRequest, Contact>(request);

                if (!request.PhoneNumbers.IsNullOrEmpty())
                {
                    var phoneNumbers = request.PhoneNumbers.Select(x => new PhoneNumber() { Number = x }).ToList();
                    contact.PhoneNumbers = phoneNumbers;
                }

                await _context.Contacts.AddAsync(contact, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                return contact;
            }
            catch (Exception e)
            {
                await _logger.LogInfo(e, "Failed to create contact");
                throw new CreateFailureException(nameof(CreateContactRequest), e);
            }
        }
    }
}