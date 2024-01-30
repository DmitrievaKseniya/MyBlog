using AutoMapper;
using MyBlog.BLL.Models;
using MyBlog.BLL.ViewModels;

namespace MyBlog.WebService
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<RegisterViewModel, User>()
                .ForMember(x => x.Email, opt => opt.MapFrom(c => c.EmailReg))
                .ForMember(x => x.UserName, opt => opt.MapFrom(c => c.Login));

            CreateMap<UserNewViewModel, User>()
                .ForMember(x => x.Email, opt => opt.MapFrom(c => c.EmailReg))
                .ForMember(x => x.UserName, opt => opt.MapFrom(c => c.Login));

            CreateMap<LoginViewModel, User>();

            CreateMap<UserEditViewModel, User>();
            CreateMap<User, UserEditViewModel>().ForMember(x => x.UserId, opt => opt.MapFrom(c => c.Id));
        }
    }
}
