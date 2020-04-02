namespace Domain.Entities
{
    public partial class Guild : EntityModel<Guild>
    {
        public Guild(string name, Member member)
        {
            Name = name;
            Invite(member).BeAccepted();
            Promote(member);
        }
        public virtual void ChangeName(string newName)
        {
            Name = newName;
        }

        public virtual Invite Invite(Member member)
        {
            var invite = new Invite(Id, member.Id)
            {
                Guild = this,
                Member = member
            };
            Invites.Add(invite);
            return invite;
        }

        public virtual Member AcceptMember(Member member)
        {
            Members.Add(member);
            if (Members.Count == 1)
            {
                member.BePromoted();
            }
            return member;
        }

        public virtual Member Promote(Member member)
        {
            return member.BePromoted();
        }

        public virtual Member KickMember(Member member)
        {
            if (member.IsGuildMaster)
            {
                member.BeDemoted();
            }
            Members.Remove(member);
            return member;
        }
    }
}