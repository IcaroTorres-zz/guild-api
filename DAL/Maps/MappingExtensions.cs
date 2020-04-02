using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System.IO;

namespace DAL.Maps
{
    public static class MappingExtensions
    {
        public static ModelBuilder SeedEntities(this ModelBuilder builder)
        {
            builder.Entity<Guild>().Seed();
            builder.Entity<Member>().Seed();
            builder.Entity<Invite>().Seed();
            builder.Entity<Membership>().Seed();
            return builder;
        }
        private static void Seed<T>(this EntityTypeBuilder<T> builder) where T : EntityModel<T>
        {
            using var stream = new StreamReader($"..\\DAL\\Json\\{typeof(T).Name.ToLower()}s.json");
            var entitiesT = JsonConvert.DeserializeObject<T[]>(stream.ReadToEnd());
            builder.HasData(entitiesT);
        }
    }
}