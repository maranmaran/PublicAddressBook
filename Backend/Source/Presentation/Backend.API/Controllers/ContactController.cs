using Backend.Business.Contacts.Requests.Create;
using Backend.Business.Contacts.Requests.Delete;
using Backend.Business.Contacts.Requests.Get;
using Backend.Business.Contacts.Requests.GetPaged;
using Backend.Business.Contacts.Requests.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.API.Controllers
{
    public class ContactsController : BaseController
    {

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetPagedContactsRequest request, CancellationToken cancellationToken = default)
        {
            return Ok(await Mediator.Send(request, cancellationToken));
        }

        [AllowAnonymous]
        [HttpGet("{contactId}")]
        public async Task<IActionResult> Get(Guid contactId, CancellationToken cancellationToken = default)
        {
            return Ok(await Mediator.Send(new GetContactRequest(contactId), cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateContactRequest request, CancellationToken cancellationToken = default)
        {
            return Ok(await Mediator.Send(request, cancellationToken));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateContactRequest request, CancellationToken cancellationToken = default)
        {
            return Ok(await Mediator.Send(request, cancellationToken));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            return Ok(await Mediator.Send(new DeleteContactRequest(id), cancellationToken));
        }
    }
}
