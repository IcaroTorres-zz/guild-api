using AutoMapper;
using Domain.Models;
using Domain.Models.Nulls;

namespace Persistence.MapperProfiles
{
    public class NullObjectToDataFilterProfile : Profile
    {
        public NullObjectToDataFilterProfile()
        {
            CreateMap<Guild, Entities.Guild>()
                .ForMember(dest => dest.GetLeader(),
                    opt => opt.PreCondition(
                        src => !(src.GetLeader() is INullObject)));

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
