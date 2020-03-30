using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace Infrastructure.Maps
{
    public static class SeedExtensions
    {
        public static ModelBuilder SeedEntities(this ModelBuilder builder)
        {
            builder.Entity<Guild>().Seed();
            builder.Entity<Member>().Seed();
            builder.Entity<Invite>().Seed();
            builder.Entity<Membership>().Seed();
            return builder;
        }

        public static ModelBuilder EnableGuidToStringConversion(this ModelBuilder builder)
        {
            builder.Model.GetEntityTypes().ToList().ForEach(entityType =>
                entityType.GetProperties().ToList().ForEach(property =>
                {
                    if (property.ClrType == typeof(Guid) || property.ClrType == typeof(Guid?))
                    {
                        property.SetValueConverter(
                            new ValueConverter<Guid, string>(guid => guid.ToString(), str => Guid.Parse(str)));
                    }
                }));
            return builder;
        }
        private static void Seed<T>(this EntityTypeBuilder<T> builder) where T : EntityModel<T>
        {
            using var stream = new StreamReader($"Infrastructure\\Json\\{typeof(T).Name.ToLower()}s.json");
            var entitiesT = JsonConvert.DeserializeObject<T[]>(stream.ReadToEnd());
            builder.HasData(entitiesT);
        }
    }
}