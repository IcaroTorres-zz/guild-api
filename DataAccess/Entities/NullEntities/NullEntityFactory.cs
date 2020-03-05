using System;
using System.Collections.Generic;
using DataAccess.Entities.NullEntities;

namespace DataAccess.Entities
{
    public class NullEntityFactory
    {
        private Dictionary<Type, object> NullTypes = new Dictionary<Type, object>
        {
            { typeof(Guild), new NullGuild() }
        };
        public T GetNullObject<T>() where T : class, new()
        {
            if (NullTypes.TryGetValue(typeof(T), out object nullType))
            {
                return (T)nullType;
            }
            return new T();
        }
    }
}