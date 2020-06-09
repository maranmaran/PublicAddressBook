using System;
using System.Threading;
using System.Threading.Tasks;
using Backend.Domain;
using Backend.Domain.Entities.Contacts;
using Backend.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Business.Contacts.Requests.Get
{
    public class GetContactRequestHandler : IRequestHandler<GetContactRequest, Contact>
    {
        private readonly IApplicationDbContext _context;

        public GetContactRequestHandler(IApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Contact> Handle(GetContactRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var contact = await _context.Contacts
                    .Include(x => x.PhoneNumbers)
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (contact == null)
                    throw new NotFoundException(nameof(Contact), request.Id);

                return contact;
            }
            catch (Exception e)
            {
                throw new Exception(nameof(GetContactRequest), e);
            }
        }
    }
}