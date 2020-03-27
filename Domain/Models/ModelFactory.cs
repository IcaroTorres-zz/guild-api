using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Domain.Models.NullEntities
{
    public static class ModelFactory
    {
        private static readonly Dictionary<Type, object> NullTypes = new Dictionary<Type, object>
        {
            { typeof(Guild), new NullGuild() },
            { typeof(Member), new NullMember() },
            { typeof(Invite), new NullInvite() },
            { typeof(Membership), new NullMembership() }
        };
        public static TResult ConstructWith<TResult, T>(T entity) where T : EntityModel<T> where TResult : DomainModel<T>
        {
            return (TResult)(entity is T ? Activator.CreateInstance(typeof(TResult), entity) : NullTypes[typeof(T)]);
        }
    }
}