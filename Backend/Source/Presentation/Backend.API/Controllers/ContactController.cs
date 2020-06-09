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
    /// <summary>
    /// Contacts CRUD operations handler
    /// </summary>
    public class ContactsController : BaseController
    {

        /// <summary>
        /// Gets paged contacts
        /// </summary>
        /// <param name="request">Request containing paging information</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] GetPagedContactsRequest request, CancellationToken cancellationToken = default)
        {
            return Ok(await Mediator.Send(request, cancellationToken));
        }

        /// <summary>
        /// Gets single contact by id
        /// </summary>
        /// <param name="contactId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{contactId}")]
        public async Task<IActionResult> Get(Guid contactId, CancellationToken cancellationToken = default)
        {
            return Ok(await Mediator.Send(new GetContactRequest(contactId), cancellationToken));
        }

        /// <summary>
        /// Creates contact 
        /// </summary>
        /// <param name="request">Contains contact information and list of phone numbers</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateContactRequest request, CancellationToken cancellationToken = default)
        {
            return Ok(await Mediator.Send(request, cancellationToken));
        }

        /// <summary>
        /// Updates contact
        /// </summary>
        /// <param name="request">Contains contact information for existing contact that's about to be updated</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateContactRequest request, CancellationToken cancellationToken = default)
        {
            return Ok(await Mediator.Send(request, cancellationToken));
        }

        /// <summary>
        /// Deletes contact
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            return Ok(await Mediator.Send(new DeleteContactRequest(id), cancellationToken));
        }
    }
}
