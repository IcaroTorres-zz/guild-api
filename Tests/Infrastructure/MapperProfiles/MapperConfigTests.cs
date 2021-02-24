using Tests.Helpers;
using Xunit;

namespace Tests.Infrastructure.MapperProfiles
{
    [Trait("Infrastructure", "AutoMapper")]
    public class MapperConfigTests
    {
        [Fact]
        public void TestConfig()
        {
            var mapperConfig = MapperConfig.Configuration;
            mapperConfig.AssertConfigurationIsValid();
        }
    }
}
