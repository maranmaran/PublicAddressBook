using Backend.Business.Authorization.AuthorizationRequests.ChangePassword;
using Backend.Business.Authorization.AuthorizationRequests.CurrentUser;
using Backend.Business.Authorization.AuthorizationRequests.SignIn;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.API.Controllers
{
    /// <summary>
    /// Authorization API
    /// </summary>
    public class AuthorizationController : BaseController
    {
        /// <summary>
        /// Signs user in 
        /// </summary>
        /// <param name="request">Request containing sign in information like username and password</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn([FromBody] SignInRequest request, CancellationToken cancellationToken = default)
        {
            var (response, token) = await Mediator.Send(request, cancellationToken);
            Response.Cookies.Append("jwt", token);

            return Ok(response);
        }

        /// <summary>
        /// Gets information that's needed for authenticated user by ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> CurrentUserInformation(Guid id, CancellationToken cancellationToken = default)
        {
            var response = await Mediator.Send(new CurrentUserRequest(id), cancellationToken);

            return Ok(response);
        }

        /// <summary>
        /// Changes password for user
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken cancellationToken = default)
        {
            await Mediator.Send(request, cancellationToken);

            return Accepted();
        }
    }
}
