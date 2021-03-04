using Domain.Models;

namespace Domain.Common
{
    public interface IModelFactory
    {
        Guild CreateGuild(string name, Member member);
        Member CreateMember(string name);
        Invite CreateInvite(Guild guild, Member member);
        Membership CreateMembership(Guild guild, Member member);
    }
}
