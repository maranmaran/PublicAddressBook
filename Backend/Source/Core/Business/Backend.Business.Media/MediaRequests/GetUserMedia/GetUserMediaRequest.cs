using System;
using System.Collections.Generic;
using Backend.Domain.Entities.Media;
using MediatR;
using MediaType = Backend.Domain.Enum.MediaType;

namespace Backend.Business.Media.MediaRequests.GetUserMedia
{
    public class GetUserMediaRequest : IRequest<IEnumerable<MediaFile>>
    {
        public GetUserMediaRequest(Guid id, MediaType? type = null)
        {
            UserId = id;
            MediaType = type;
        }

        public Guid UserId { get; set; }
        public MediaType? MediaType { get; set; }
    }
}