﻿using Backend.Domain;
using Backend.Domain.Entities.Media;
using Backend.Domain.Entities.User;
using Backend.Domain.Enum;
using Backend.Infrastructure.Exceptions;
using Backend.Library.AmazonS3.Interfaces;
using Backend.Library.MediaCompression.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Business.Media.MediaRequests.UploadUserAvatar
{
    public class UploadUserAvatarRequestHandler : IRequestHandler<UploadUserAvatarRequest, string>
    {

        private readonly IApplicationDbContext _context;
        private readonly IS3Service _s3;
        private readonly IMediaCompressionService _compressionService;

        public UploadUserAvatarRequestHandler(IS3Service s3, IApplicationDbContext context, IMediaCompressionService compressionService)
        {
            _s3 = s3;
            _context = context;
            _compressionService = compressionService;
        }

        public async Task<string> Handle(UploadUserAvatarRequest request, CancellationToken cancellationToken)
        {
            try
            {
                // save everything to s3
                var key = _s3.GetS3Key(nameof(ApplicationUser.Avatar), request.UserId);

                var byteArr = Convert.FromBase64String(request.Base64.Replace("data:image/jpeg;base64,", string.Empty));

                var compressedImage = await _compressionService.Compress(MediaType.Image, new MemoryStream(byteArr));

                await _s3.WriteToS3(key, compressedImage);
                var presignedUrl = await _s3.GetPresignedUrlAsync(key);

                // add newly created media
                var media = new MediaFile()
                {
                    ApplicationUserId = request.UserId,
                    DownloadUrl = presignedUrl,
                    Type = MediaType.Image,
                    DateModified = DateTime.UtcNow,
                    DateUploaded = DateTime.UtcNow,
                    Filename = $"Avatar - {DateTime.UtcNow:MM/dd/yyyy}",
                    S3FilePath = key,
                };
                await _context.MediaFiles.AddAsync(media, cancellationToken);

                // fetch and update user Avatar KEY for s3.. which will be presigned on every fetch of user
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
                user.Avatar = key;
                _context.Users.Update(user);
                await _context.SaveChangesAsync(cancellationToken);

                return presignedUrl;
            }
            catch (Exception e)
            {
                throw new CreateFailureException(nameof(UploadUserAvatarRequest), e);
            }
        }
    }
}