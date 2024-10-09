using AutoMapper;
using Dashboard.DAL.Models;
using Dashboard.DAL.ViewModels;

namespace Dashboard.BLL.MappingProfiles
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile() 
        {
            // Product -> ProductVM
            CreateMap<Product, ProductVM>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
        }
    }
}
