using Application.Common.Responses;
using AutoMapper;
using Domain.Common;
using Domain.Models;

namespace Application.Common.MapperProfiles
{
    public class DomainToApplicationProfile : Profile
    {
        public DomainToApplicationProfile()
        {
            CreateMap<Guild, GuildResponse>()
                .ForMember(dest => dest.Leader, opt =>
                {
                    opt.PreCondition(src => !(src.GetLeader() is INullObject));
                    opt.MapFrom(src => src.GetLeader());
                });
            CreateMap<Guild, MemberGuildResponse>();

            CreateMap<Member, MemberResponse>()
                .ForMember(dest => dest.Guild, opt =>
                {
                    opt.PreCondition(src => !(src.GetGuild() is INullObject));
                    opt.MapFrom(src => src.GetGuild());
                });
            CreateMap<Member, GuildMemberResponse>();

            CreateMap<Invite, InviteResponse>()
                .ForMember(dest => dest.InviteDate,
                    opt => opt.MapFrom(
                        src => src.CreatedDate))
                .ForMember(dest => dest.ResponseDate,
                    opt => opt.MapFrom(
                        src => src.ModifiedDate))
                .ForMember(dest => dest.Member, opt =>
                {
                    opt.PreCondition(src => !(src.GetMember() is INullObject));
                    opt.MapFrom(src => src.GetMember());
                })
                .ForMember(dest => dest.Guild, opt =>
                {
                    opt.PreCondition(src => !(src.GetGuild() is INullObject));
                    opt.MapFrom(src => src.GetGuild());
                });

            CreateMap<Membership, MembershipResponse>()
                .ForMember(dest => dest.Since,
                    opt => opt.MapFrom(
                        src => src.CreatedDate))
                .ForMember(dest => dest.Until,
                    opt => opt.MapFrom(
                        src => src.ModifiedDate));

            CreateMap<PagedResponse<Member>, PagedResponse<MemberResponse>>();
            CreateMap<PagedResponse<Guild>, PagedResponse<GuildResponse>>();
            CreateMap<PagedResponse<Invite>, PagedResponse<InviteResponse>>();
            CreateMap<PagedResponse<Membership>, PagedResponse<MembershipResponse>>();
        }
    }
}
