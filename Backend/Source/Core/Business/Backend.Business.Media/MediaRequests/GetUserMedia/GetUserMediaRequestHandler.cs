using Backend.Domain;
using Backend.Domain.Entities.Media;
using Backend.Infrastructure.Exceptions;
using Backend.Library.AmazonS3.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Backend.BusinessTests")]
namespace Backend.Business.Media.MediaRequests.GetUserMedia
{
    public class GetUserMediaRequestHandler : IRequestHandler<GetUserMediaRequest, IEnumerable<MediaFile>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IS3Service _s3AccessService;

        public GetUserMediaRequestHandler(IApplicationDbContext context, IS3Service s3AccessService)
        {
            _context = context;
            _s3AccessService = s3AccessService;
        }

        public async Task<IEnumerable<MediaFile>> Handle(GetUserMediaRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var media = (await GetMedia(request, cancellationToken)).ToList();

                // refresh download URLs
                foreach (var mediaFile in media)
                {
                    if (_s3AccessService.CheckIfPresignedUrlIsExpired(mediaFile.DownloadUrl))
                        mediaFile.DownloadUrl = await _s3AccessService.GetPresignedUrlAsync(mediaFile.S3FilePath);
                }

                return media;
            }
            catch (Exception e)
            {
                throw new FetchFailureException($"Could not fetch list of media of type {request.MediaType} for user {request.UserId}", e);
            }
        }

        internal async Task<IEnumerable<MediaFile>> GetMedia(GetUserMediaRequest request,
            CancellationToken cancellationToken = default)
        {
            var mediaQuery = _context.MediaFiles.Where(x => x.ApplicationUserId == request.UserId);

            if (request.MediaType.HasValue)
            {
                mediaQuery = mediaQuery.Where(x => x.Type == request.MediaType);
            }

            return await mediaQuery.ToListAsync(cancellationToken);
        }
    }
}