using AutoMapper;

namespace BlogWebApi.Models.ModelMapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Post, PostDTO>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name)) //Adding custome resolver to map The category name
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username)); //Adding  custome resolver to map the username
        }
    }
}
