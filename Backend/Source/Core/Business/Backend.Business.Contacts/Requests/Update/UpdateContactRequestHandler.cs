using AutoMapper;
using Backend.Common.Extensions;
using Backend.Domain;
using Backend.Domain.Entities.Contacts;
using Backend.Infrastructure.Exceptions;
using Backend.Library.Logging.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Business.Contacts.Requests.Update
{
    public class UpdateContactRequestHandler : IRequestHandler<UpdateContactRequest, Contact>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILoggingService _loggingService;

        public UpdateContactRequestHandler(IApplicationDbContext context, IMapper mapper, ILoggingService loggingService)
        {
            _context = context;
            _mapper = mapper;
            _loggingService = loggingService;
        }


        public async Task<Contact> Handle(UpdateContactRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var contact = await _context.Contacts.Include(x => x.PhoneNumbers).FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (contact == null)
                    throw new NotFoundException(nameof(Contact), request.Id);

                _mapper.Map(request, contact);

                _context.PhoneNumbers.RemoveRange(contact.PhoneNumbers);
                if (!request.PhoneNumbers.IsNullOrEmpty())
                {
                    var phoneNumbers = request.PhoneNumbers.Select(x => new PhoneNumber() { Number = x }).ToList();
                    contact.PhoneNumbers = phoneNumbers;
                }

                _context.Contacts.Update(contact);
                await _context.SaveChangesAsync(cancellationToken);

                return contact;
            }
            catch (Exception e)
            {
                await _loggingService.LogInfo(e, "Failed to update contact");
                throw new UpdateFailureException("Could not update contact", request.Id, e);
            }
        }
    }
}
