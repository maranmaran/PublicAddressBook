using Backend.Common;
using Backend.Domain;
using Backend.Domain.Entities.User;
using Backend.Domain.Enum;
using Backend.Infrastructure.Exceptions;
using Backend.Library.AmazonS3.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Business.Users.UsersRequests.GetUser
{
    public class GetUserRequestHandler : IRequestHandler<GetUserRequest, ApplicationUser>
    {
        private readonly IApplicationDbContext _context;
        private readonly IS3Service _s3Service;

        public GetUserRequestHandler(IApplicationDbContext context, IS3Service s3Service)
        {
            _context = context;
            _s3Service = s3Service;
        }

        public async Task<ApplicationUser> Handle(GetUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                switch (request.AccountType)
                {
                    case AccountType.User:

                        return await GetUser(request);

                    default:
                        throw new NotImplementedException($"This account type does not exist: {request.AccountType}");
                }
            }
            catch (Exception e)
            {
                throw new NotFoundException(nameof(ApplicationUser), request.Id, e);
            }
        }

        private async Task<ApplicationUser> GetUser(GetUserRequest request)
        {
            var user = await _context.Users.FirstAsync(x => x.Id == request.Id);
            user.Avatar = await RefreshAvatar(user.Avatar);

            return user;
        }

        private async Task<string> RefreshAvatar(string avatar)
        {
            // must be s3 then if not generic
            if (GenericAvatarConstructor.IsGenericAvatar(avatar) == false)
                return await _s3Service.GetPresignedUrlAsync(avatar);

            return avatar;
        }
    }
}