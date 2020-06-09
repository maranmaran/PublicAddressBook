using Backend.Business.Media.MediaRequests.GetUserMedia;
using Backend.Business.Media.MediaRequests.UploadUserAvatar;
using Backend.Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.API.Controllers
{
    /// <summary>
    /// Media API
    /// </summary>
    public class MediaController : BaseController
    {
        /// <summary>
        /// Gets all user media by type
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}/{type?}")]
        public async Task<IActionResult> GetUserMediaByType(Guid id, MediaType? type = null, CancellationToken cancellationToken = default)
        {

            throw new NotImplementedException("Needs AWS key and purpose");
            return Ok(await Mediator.Send(new GetUserMediaRequest(id, type), cancellationToken));
        }

        /// <summary>
        /// Uploads user avatar
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="base64Image"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<IActionResult> UploadAvatar(
            [FromForm] Guid userId,
            [FromForm] string base64Image,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException("Needs AWS key and purpose");
            return Ok(await Mediator.Send(new UploadUserAvatarRequest()
            {
                UserId = userId,
                Base64 = base64Image,
            }, cancellationToken));
        }
    }
}
