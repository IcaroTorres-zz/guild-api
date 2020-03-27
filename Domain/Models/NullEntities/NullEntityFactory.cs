using DataAccess.Entities;
using System;
using System.Collections.Generic;

namespace Domain.Models.NullEntities
{
    public class NullEntityFactory
    {
        private readonly Dictionary<Type, object> NullTypes = new Dictionary<Type, object>
        {
            { typeof(Guild), new NullGuild() },
            { typeof(Member), new NullMember() },
            { typeof(Invite), new NullInvite() },
            { typeof(GuildModel), new NullGuild() },
            { typeof(MemberModel), new NullMember() },
            { typeof(InviteModel), new NullInvite() },
        };
        public T GetNullObject<T>() => (T)NullTypes[typeof(T)];
    }
}