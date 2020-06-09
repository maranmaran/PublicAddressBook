using AutoMapper;
using Backend.Business.Users.UsersRequests.CreateUser;
using Backend.Business.Users.UsersRequests.UpdateUser;
using Backend.Domain.Entities.User;
using Backend.Infrastructure.Extensions;

namespace Backend.Business.Users
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<CreateUserRequest, ApplicationUser>().IgnoreAllVirtual()
                .ForMember(x => x.UserSetting, o => o.MapFrom(x => new UserSetting()));

            CreateMap<UpdateUserRequest, ApplicationUser>().IgnoreAllVirtual();
        }
    }
}
