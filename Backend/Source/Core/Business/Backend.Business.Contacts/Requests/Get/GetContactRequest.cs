using System;
using Backend.Domain.Entities.Contacts;
using MediatR;

namespace Backend.Business.Contacts.Requests.Get
{

    public class GetContactRequest : IRequest<Contact>
    {
        public GetContactRequest(Guid contactId)
        {
            Id = contactId;
        }

        public Guid Id { get; set; }
    }
}
