using AutoMapper;
using Backend.Domain;
using Backend.Domain.Entities.User;
using Backend.Domain.Enum;
using Backend.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Business.Users.UsersRequests.UpdateUser
{
    public class UpdateUserRequestHandler : IRequestHandler<UpdateUserRequest, ApplicationUser>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateUserRequestHandler(
            IApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApplicationUser> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                switch (request.AccountType)
                {
                    case AccountType.User:
                        return await UpdateUser(request);

                    default:
                        throw new NotImplementedException($"This account type does not exist: {request.AccountType}");
                }
            }
            catch (Exception e)
            {
                throw new UpdateFailureException(nameof(ApplicationUser), request.Id, e);
            }
        }

        private async Task<ApplicationUser> UpdateUser(UpdateUserRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (user == null)
                throw new NotFoundException(nameof(ApplicationUser), request.Id);

            _mapper.Map(request, user);

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return user;
        }

    }
}