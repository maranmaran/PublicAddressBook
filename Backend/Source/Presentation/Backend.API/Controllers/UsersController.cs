using Backend.Business.Users.UsersRequests.GetUser;
using Backend.Business.Users.UsersRequests.SaveUserSettings;
using Backend.Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.API.Controllers
{
    public class UsersController : BaseController
    {

        [HttpGet("{id}/{accountType}")]
        public async Task<IActionResult> Get(Guid id, AccountType accountType, CancellationToken cancellationToken = default)
        {
            return Ok(await Mediator.Send(new GetUserRequest(id, accountType), cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> SaveUserSetting([FromBody] SaveUserSettingsRequest request, CancellationToken cancellationToken = default)
        {
            return Accepted(await Mediator.Send(request, cancellationToken));
        }
    }
}
