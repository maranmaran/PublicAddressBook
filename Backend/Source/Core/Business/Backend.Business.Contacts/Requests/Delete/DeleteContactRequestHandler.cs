using System;
using System.Threading;
using System.Threading.Tasks;
using Backend.Domain;
using Backend.Domain.Entities.Contacts;
using Backend.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend.Business.Contacts.Requests.Delete
{
    public class DeleteContactRequestHandler : IRequestHandler<DeleteContactRequest, Unit>
    {
        private readonly IApplicationDbContext _context;

        public DeleteContactRequestHandler(IApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Unit> Handle(DeleteContactRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var contact = await _context.Contacts.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (contact == null)
                    throw new NotFoundException("Could not find contact", request.Id);

                _context.Contacts.Remove(contact);

                await _context.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
            catch (Exception e)
            {
                throw new DeleteFailureException(nameof(Contact), request.Id, e);
            }
        }
    }
}