using AutoMapper;
using Dashboard.DAL.Models.Identity;
using Dashboard.DAL.ViewModels;

namespace Dashboard.BLL.MappingProfiles
{
    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile() 
        {
            // Role -> RoleVM
            CreateMap<Role, RoleVM>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));

            CreateMap<RoleVM, Role>();
        }
    }
}
