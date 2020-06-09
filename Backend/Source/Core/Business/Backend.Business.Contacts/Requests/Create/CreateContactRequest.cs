using Backend.Domain.Entities.Contacts;
using MediatR;
using System.Collections.Generic;

namespace Backend.Business.Contacts.Requests.Create
{

    public class CreateContactRequest : IRequest<Contact>
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public List<string> PhoneNumbers { get; set; }
    }
}
