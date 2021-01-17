using Tests.Helpers;
using Xunit;

namespace Tests.Application.MapperProfiles
{
    [Trait("Application", "AutoMapper")]
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
