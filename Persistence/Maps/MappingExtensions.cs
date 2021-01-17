using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Persistence.Maps
{
    public static class MappingExtensions
    {
        public static ModelBuilder SeedEntities(this ModelBuilder builder)
        {
            var jsonSettings = new JsonSerializerSettings { ContractResolver = new NonPublicPropertiesResolver() };
            builder.Entity<Guild>().Seed(jsonSettings, x => new { x.Id, x.Name, x.Disabled, x.CreatedDate, x.ModifiedDate });
            builder.Entity<Member>().Seed(jsonSettings, x => new { x.Id, x.Name, x.GuildId, x.Disabled, x.IsGuildLeader, x.CreatedDate, x.ModifiedDate });
            builder.Entity<Invite>().Seed(jsonSettings, x => new { x.Id, x.GuildId, x.MemberId, x.Disabled, x.Status, x.CreatedDate, x.ModifiedDate });
            builder.Entity<Membership>().Seed(jsonSettings, x => new { x.Id, x.GuildId, x.MemberId, x.Disabled, x.CreatedDate, x.ModifiedDate });
            return builder;
        }
        private class NonPublicPropertiesResolver : DefaultContractResolver
        {
            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                var prop = base.CreateProperty(member, memberSerialization);
                if (member is PropertyInfo propertyInfo)
                {
                    prop.Readable = (propertyInfo.GetMethod != null);
                    prop.Writable = (propertyInfo.SetMethod != null);
                }
                return prop;
            }
        }
        private static void Seed<T>(this EntityTypeBuilder<T> builder,
            JsonSerializerSettings jsonSettings,
            Func<T, object> anonymousConversion) where T : EntityModel<T>
        {
            using var stream = new StreamReader($"..\\Persistence\\Json\\{typeof(T).Name.ToLower()}s.json");
            var entitiesT = JsonConvert.DeserializeObject<List<T>>(stream.ReadToEnd(), jsonSettings);
            builder.HasData(entitiesT.Select(x => anonymousConversion(x)).ToArray());
        }
    }
}