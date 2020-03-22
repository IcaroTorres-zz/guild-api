using System;
using System.Collections.Generic;
using DataAccess.Entities;

namespace Domain.Models.NullEntities
{
    public class NullEntityFactory
    {
        private Dictionary<Type, object> NullTypes = new Dictionary<Type, object>
        {
            { typeof(Guild), new NullGuild() },
            { typeof(Member), new NullMember() },
            { typeof(Invite), new NullInvite() },
        };
        public T GetNullObject<T>() => (T) NullTypes[typeof(T)];
    }
}