using Backend.Domain.Entities.Contacts;
using MediatR;
using System;
using System.Collections.Generic;

namespace Backend.Business.Contacts.Requests.Update
{
    public class UpdateContactRequest : IRequest<Contact>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<string> PhoneNumbers { get; set; }
    }
}
