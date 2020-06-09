using MediatR;
using System;

namespace Backend.Business.Contacts.Requests.Delete
{

    public class DeleteContactRequest : IRequest<Unit>
    {
        public DeleteContactRequest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
