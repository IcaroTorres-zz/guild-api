using System.Linq;

namespace Domain.Entities
{
    public partial class Member : EntityModel<Member>
    {
        public Member(string name)
        {
            Name = name;
        }
        public virtual void ChangeName(string newName)
        {
            Name = newName;
        }
        public virtual Member JoinGuild(Guild guild)
        {
            LeaveGuild();
            Guild = guild;
            GuildId = guild.Id;
            Memberships.Add(new Membership(guild, this));
            return this;
        }
        public virtual Member BePromoted()
        {
            Guild?.Members
                 .Where(x => x.IsGuildMaster && x.Id != Id)
                 .ToList()
                 .ForEach(x => x.IsGuildMaster = false);

            IsGuildMaster = true;
            return this;
        }
        public virtual Member BeDemoted()
        {
            IsGuildMaster = false;

            Guild?.Members
                 .OrderByDescending(x => x.Memberships.SingleOrDefault(x => x.Until == null)?.GetDuration())
                 .FirstOrDefault(x => x.Id != Id && !x.IsGuildMaster)?.BePromoted();

            return this;
        }
        public virtual Member LeaveGuild()
        {
            Memberships.OrderBy(x => x.Since).LastOrDefault()?.RegisterExit();
            Guild?.KickMember(this);
            Guild = null;

            return this;
        }
    }
}
