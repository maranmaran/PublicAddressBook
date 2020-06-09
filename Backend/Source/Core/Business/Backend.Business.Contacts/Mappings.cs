using AutoMapper;
using Backend.Business.Contacts.Requests.Create;
using Backend.Business.Contacts.Requests.Update;
using Backend.Domain.Entities.Contacts;

namespace Backend.Business.Contacts
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<CreateContactRequest, Contact>().ForMember(x => x.PhoneNumbers, opt => opt.Ignore());
            CreateMap<UpdateContactRequest, Contact>().ForMember(x => x.PhoneNumbers, opt => opt.Ignore());
        }
    }
}
