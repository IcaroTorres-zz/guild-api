using Domain.Entities;
using Domain.Models.NullEntities;
using System;
using System.Collections.Generic;

namespace Domain.Models
{
    public class ModelFactory
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
        public GuildModel Create(Guild guild) => guild is Guild ? new GuildModel(guild) : new NullGuild();
        public MemberModel Create(Member member) => member is Member ? new MemberModel(member) : new NullMember();
        public InviteModel Create(Invite invite) => invite is Invite ? new InviteModel(invite) : new NullInvite();
        public MembershipModel Create(Membership membership) => membership is Membership ? new MembershipModel(membership) : new NullMembership();
    }
}