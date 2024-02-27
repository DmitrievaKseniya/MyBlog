using API.Models.Articles;
using API.Models.Comments;
using API.Models.Roles;
using API.Models.Tags;
using API.Models.Users;
using AutoMapper;
using MyBlog.BLL.Models;

namespace API
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<User, GetUsersResponse>();
            CreateMap<Role, RoleViews>();
            CreateMap<AddUserRequest, User>()
                .ForMember(x => x.UserName, opt => opt.MapFrom(c => c.Email));

            CreateMap<Role, GetRolesResponse>();

            CreateMap<Article, GetArticlesResponse>();
            CreateMap<Tag, TagViews>();

            CreateMap<Tag, GetTagsResponse>();

            CreateMap<Comment, GetCommentsResponse>();
        }
    }
}
