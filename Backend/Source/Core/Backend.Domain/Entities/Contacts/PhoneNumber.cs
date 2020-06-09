using System;

namespace Backend.Domain.Entities.Contacts
{
    public class PhoneNumber
    {
        public Guid Id { get; set; }
        public string Number { get; set; }

        public Guid ContactId { get; set; }
        public virtual Contact Contact { get; set; }
    }
}