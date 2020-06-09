using AutoMapper;
using Backend.Common.Extensions;
using Backend.Domain;
using Backend.Domain.Entities.Contacts;
using Backend.Infrastructure.Exceptions;
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

        public UpdateContactRequestHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<Contact> Handle(UpdateContactRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var contact = await _context.Contacts.Include(x => x.PhoneNumbers).FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
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
                throw new UpdateFailureException("Could not update contact", request.Id, e);
            }
        }
    }
}
