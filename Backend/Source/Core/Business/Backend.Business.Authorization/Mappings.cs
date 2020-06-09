using AutoMapper;
using Backend.Business.Authorization.AuthorizationRequests.CurrentUser;
using Backend.Domain.Entities.User;

namespace Backend.Business.Authorization
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<ApplicationUser, CurrentUserRequestResponse>();
        }
    }
}
