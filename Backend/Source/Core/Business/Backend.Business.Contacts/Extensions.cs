using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Backend.Domain.Entities.Contacts;

namespace Backend.Business.Contacts
{
    public class Factory
    {
        public static IEnumerable<PhoneNumber> ScaffoldPhoneNumbers(IEnumerable<string> phoneNumbers)
        {
            return phoneNumbers.Select(x => new PhoneNumber() { Number = x }).ToList();
        } 
    }
}
