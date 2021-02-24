using Application.Common.Responses;
using Application.Identity;
using AutoMapper;

namespace Infrastructure.Identity.MapperProfiles
{
    public class IdentityProfile : Profile
    {
        public IdentityProfile()
        {
            CreateMap<User, UserResponse>();
            CreateMap<RegisterUserCommand, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore());
            CreateMap<UpdateUserCommand, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore());
        }
    }
}
