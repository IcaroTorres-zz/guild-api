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
                .ForMember(dest => dest.Leader,
                    opt => opt.PreCondition(
                        src => !(src.Leader is INullObject)));

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
