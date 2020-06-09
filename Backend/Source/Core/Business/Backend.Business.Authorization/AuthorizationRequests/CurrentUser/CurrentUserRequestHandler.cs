using AutoMapper;
using Backend.Common;
using Backend.Domain;
using Backend.Domain.Entities.User;
using Backend.Infrastructure.Exceptions;
using Backend.Library.AmazonS3.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Business.Authorization.AuthorizationRequests.CurrentUser
{
    public class CurrentUserRequestHandler : IRequestHandler<CurrentUserRequest, CurrentUserRequestResponse>
    {
        private readonly IMapper _mapper;
        private readonly IS3Service _s3Service;
        private readonly IApplicationDbContext _context;

        public CurrentUserRequestHandler(IMapper mapper, IApplicationDbContext context, IS3Service s3Service)
        {
            _mapper = mapper;
            _context = context;
            _s3Service = s3Service;
        }

        public async Task<CurrentUserRequestResponse> Handle(CurrentUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _context.Users
                                            .Include(x => x.UserSetting)
                                            .ThenInclude(x => x.NotificationSettings)
                                            .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken: cancellationToken);

                if (user == null) throw new NotFoundException(nameof(ApplicationUser), request.UserId);

                var response = _mapper.Map<CurrentUserRequestResponse>(user);

                // refresh avatar url if needed
                if (GenericAvatarConstructor.IsGenericAvatar(user.Avatar) == false)
                    response.Avatar = await _s3Service.GetPresignedUrlAsync(user.Avatar);

                return response;
            }
            catch (Exception e)
            {
                throw new NotFoundException(nameof(CurrentUser), request.UserId, e);
            }
        }
    }
}