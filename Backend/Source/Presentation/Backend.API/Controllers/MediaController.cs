using Backend.Business.Media.MediaRequests.UploadUserAvatar;
using Backend.Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using Backend.Business.Media.MediaRequests.GetUserMedia;

namespace Backend.API.Controllers
{
    public class MediaController : BaseController
    {
        [HttpGet("{id}/{type?}")]
        public async Task<IActionResult> GetUserMediaByType(Guid id, MediaType? type = null, CancellationToken cancellationToken = default)
        {
            return Ok(await Mediator.Send(new GetUserMediaRequest(id, type), cancellationToken));
        }


        [HttpPost]
        public async Task<IActionResult> UploadAvatar(
            [FromForm] Guid userId,
            [FromForm] string base64Image,
            CancellationToken cancellationToken = default)
        {
            return Ok(await Mediator.Send(new UploadUserAvatarRequest()
            {
                UserId = userId,
                Base64 = base64Image,
            }, cancellationToken));
        }
    }
}
