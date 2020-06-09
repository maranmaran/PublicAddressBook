using System;

namespace Backend.Domain.DeserializatorPOCOs
{
    public class ContactDeserializeResult
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
