using Backend.Common;
using Backend.Domain;
using Backend.Domain.Entities.User;
using Backend.Domain.Enum;
using Backend.Infrastructure.Exceptions;
using Backend.Library.AmazonS3.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Business.Users.UsersRequests.GetAllUsers
{
    public class GetAllUsersRequestHandler : IRequestHandler<GetAllUsersRequest, IEnumerable<ApplicationUser>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IS3Service _s3Service;

        public GetAllUsersRequestHandler(IApplicationDbContext context, IS3Service s3Service)
        {
            _context = context;
            _s3Service = s3Service;
        }

        public async Task<IEnumerable<ApplicationUser>> Handle(GetAllUsersRequest request, CancellationToken cancellationToken)
        {
            try
            {
                switch (request.AccountType)
                {
                    case AccountType.User:
                        return await GetAllUsers(cancellationToken);

                    default:
                        throw new NotImplementedException($"This account type does not exist: {request.AccountType}");
                }
            }
            catch (Exception e)
            {
                throw new NotFoundException(nameof(ApplicationUser), "Something went wrong fetching users", e);
            }
        }

        private async Task<IEnumerable<ApplicationUser>> GetAllUsers(CancellationToken cancellationToken = default)
        {
            var users = await _context.Users.ToListAsync(cancellationToken);

            return await RefreshAvatars(users);
        }

        // Todo this should be outsourced and reused..also tested
        private async Task<IEnumerable<ApplicationUser>> RefreshAvatars(IEnumerable<ApplicationUser> list)
        {
            var s3Avatars = list.Where(x => GenericAvatarConstructor.IsGenericAvatar(x.Avatar) == false); // must be s3 then if not generic
            foreach (var s3Avatar in s3Avatars)
            {
                s3Avatar.Avatar = await _s3Service.GetPresignedUrlAsync(s3Avatar.Avatar);
            }

            return list;
        }
    }
}