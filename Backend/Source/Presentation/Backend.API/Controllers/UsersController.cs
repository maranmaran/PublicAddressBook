using Backend.Business.Users.UsersRequests.GetUser;
using Backend.Business.Users.UsersRequests.SaveUserSettings;
using Backend.Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.API.Controllers
{
    /// <summary>
    /// Users API
    /// </summary>
    public class UsersController : BaseController
    {
        /// <summary>
        /// Gets single user information by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="accountType"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}/{accountType}")]
        public async Task<IActionResult> Get(Guid id, AccountType accountType, CancellationToken cancellationToken = default)
        {
            return Ok(await Mediator.Send(new GetUserRequest(id, accountType), cancellationToken));
        }

        /// <summary>
        /// Saves user settings
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveUserSetting([FromBody] SaveUserSettingsRequest request, CancellationToken cancellationToken = default)
        {
            return Accepted(await Mediator.Send(request, cancellationToken));
        }
    }
}
