using AutoMapper;
using Dashboard.DAL.Models.Identity;
using Dashboard.DAL.ViewModels;

namespace Dashboard.BLL.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile() 
        {
            // SignUpVM -> User
            CreateMap<SignUpVM, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));

            // User -> UserVM
            CreateMap<User, UserVM>();

            // CreateUserVM -> User
            CreateMap<CreateUserVM, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
        }
    }
}
