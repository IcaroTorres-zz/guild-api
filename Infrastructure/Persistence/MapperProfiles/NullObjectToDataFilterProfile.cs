using AutoMapper;
using Domain.Common;
using Domain.Models;

namespace Infrastructure.Persistence.MapperProfiles
{
    public class NullObjectToDataFilterProfile : Profile
    {
        public NullObjectToDataFilterProfile()
        {
            CreateMap<Guild, Entities.Guild>();

            CreateMap<Member, Entities.Member>()
                .ForMember(dest => dest.Guild,
                    opt => opt.PreCondition(
                        src => !(src.Guild is INullObject)));

            CreateMap<Invite, Entities.Invite>()
                .ForMember(dest => dest.Member,
                    opt => opt.PreCondition(
                        src => !(src.Member is INullObject)))
                .ForMember(dest => dest.Guild,
                    opt => opt.PreCondition(
                        src => !(src.Guild is INullObject)));

            CreateMap<Membership, Entities.Membership>()
                .ForMember(dest => dest.Member,
                    opt => opt.PreCondition(
                        src => !(src.Member is INullObject)))
                .ForMember(dest => dest.Guild,
                    opt => opt.PreCondition(
                        src => !(src.Guild is INullObject)));
        }
    }
}
