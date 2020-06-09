using System;
using System.Collections.Generic;

namespace Backend.Domain.Entities.Contacts
{
    public class Contact
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public ICollection<PhoneNumber> PhoneNumbers { get; set; } = new HashSet<PhoneNumber>();
    }
}
