using System;
using System.Collections.Generic;
using DataAccess.Entities.NullEntities;

namespace DataAccess.Entities
{
    public class NullEntityFactory
    {
        private Dictionary<Type, object> NullTypes = new Dictionary<Type, object>
        {
            { typeof(Guild), new NullGuild() },
            { typeof(Member), new NullMember() },
        };
        public T GetNullObject<T>() where T : class => (T)NullTypes[typeof(T)];
    }
}