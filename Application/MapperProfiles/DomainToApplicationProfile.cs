using AutoMapper;
using Business.Dtos;
using Domain.Models;
using Domain.Models.Nulls;

namespace Application.MapperProfiles
{
    public class DomainToApplicationProfile : Profile
    {
        public DomainToApplicationProfile()
        {
            CreateMap<Guild, GuildDto>()
                .ForMember(dest => dest.Leader,
                    opt =>
                    {
                        opt.PreCondition(src => !(src.GetLeader() is INullObject));
                        opt.MapFrom(src => src.GetLeader());
                    });
            CreateMap<Guild, MemberGuildDto>();

            CreateMap<Member, MemberDto>()
                .ForMember(dest => dest.Guild,
                    opt => opt.PreCondition(
                        src => !(src.Guild is INullObject)));
            CreateMap<Member, GuildMemberDto>();

            CreateMap<Invite, InviteDto>()
                .ForMember(dest => dest.InviteDate,
                    opt => opt.MapFrom(
                        src => src.CreatedDate))
                .ForMember(dest => dest.ResponseDate,
                    opt => opt.MapFrom(
                        src => src.ModifiedDate))
                .ForMember(dest => dest.Member,
                    opt => opt.PreCondition(
                        src => !(src.Member is INullObject)))
                .ForMember(dest => dest.Guild,
                    opt => opt.PreCondition(
                        src => !(src.Guild is INullObject)));

            CreateMap<Membership, MembershipDto>()
                .ForMember(dest => dest.Since,
                    opt => opt.MapFrom(
                        src => src.CreatedDate))
                .ForMember(dest => dest.Until,
                    opt => opt.MapFrom(
                        src => src.ModifiedDate))
                .ForMember(dest => dest.Member,
                    opt => opt.PreCondition(
                        src => !(src.Member is INullObject)))
                .ForMember(dest => dest.Guild,
                    opt => opt.PreCondition(
                        src => !(src.Guild is INullObject)));

            CreateMap<Pagination<Member>, Pagination<MemberDto>>();
            CreateMap<Pagination<Guild>, Pagination<GuildDto>>();
            CreateMap<Pagination<Invite>, Pagination<InviteDto>>();
            CreateMap<Pagination<Membership>, Pagination<MembershipDto>>();
        }
    }
}
