using Backend.Domain;
using Backend.Domain.Entities.User;
using Backend.Domain.Enum;
using Backend.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Business.Users.UsersRequests.DeleteUser
{
    public class DeleteUserRequestHandler : IRequestHandler<DeleteUserRequest, Unit>
    {
        private readonly IApplicationDbContext _context;

        public DeleteUserRequestHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Id == Guid.Empty)
                    throw new ArgumentException("Invalid ID");

                switch (request.AccountType)
                {
                    case AccountType.User:

                        await DeleteUser(request, cancellationToken);
                        break;

                    default:
                        throw new NotImplementedException($"This account type doesn't have delete implemented: {request.AccountType}");
                }

                return Unit.Value;
            }
            catch (Exception e)
            {
                throw new DeleteFailureException(nameof(ApplicationUser), request.Id, e);
            }
        }

        private async Task DeleteUser(DeleteUserRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (user == null)
                throw new NotFoundException(nameof(ApplicationUser), request.Id);

            _context.Users.Remove(user);

            await _context.SaveChangesAsync(cancellationToken);
        }

    }
}