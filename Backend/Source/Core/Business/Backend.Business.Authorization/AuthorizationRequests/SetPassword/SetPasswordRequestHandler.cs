using Backend.Business.Authorization.AuthorizationRequests.CurrentUser;
using Backend.Business.Authorization.AuthorizationRequests.SignIn;
using Backend.Business.Authorization.Utils;
using Backend.Domain;
using Backend.Domain.Entities.User;
using Backend.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Business.Authorization.AuthorizationRequests.SetPassword
{
    public class SetPasswordRequestHandler : IRequestHandler<SetPasswordRequest, (CurrentUserRequestResponse response, string token)>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;

        public SetPasswordRequestHandler(IApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<(CurrentUserRequestResponse response, string token)> Handle(SetPasswordRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // get user
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

                // validate
                if (user == null)
                    throw new NotFoundException(nameof(ApplicationUser), request.UserId);

                // hash
                user.PasswordHash = PasswordHasher.GetPasswordHash(request.Password);

                // save
                _context.Users.Update(user);
                await _context.SaveChangesAsync(cancellationToken);

                // log user in on client automatically
                var signInRequest = new SignInRequest() { Password = request.Password, Username = user.Username };
                var response = await _mediator.Send(signInRequest, cancellationToken);

                return response;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Could not set new password for user with id: {request.UserId}", e);
            }
        }
    }
}