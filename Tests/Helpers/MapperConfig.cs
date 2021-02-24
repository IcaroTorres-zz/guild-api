using Application.Common.MapperProfiles;
using AutoMapper;

namespace Tests.Helpers
{
    public static class MapperConfig
    {
        public static MapperConfiguration Configuration = new MapperConfiguration(a => a.AddMaps(typeof(DomainToApplicationProfile).Assembly));
    }
}
