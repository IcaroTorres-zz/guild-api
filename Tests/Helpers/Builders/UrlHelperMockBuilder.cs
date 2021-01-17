using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.Business.Usecases.Guilds.CreateGuild
{
    public sealed class UrlHelperMockBuilder
    {
        private readonly Mock<IUrlHelper> _mock;

        private UrlHelperMockBuilder()
        {
            _mock = new Mock<IUrlHelper>();
        }

        public static UrlHelperMockBuilder Create()
        {
            return new UrlHelperMockBuilder();
        }

        public UrlHelperMockBuilder SetupLink(string routename, string url = null)
        {
            var result = url ?? "https://localhost:5001/dummy";

            _mock.Setup(x => x.Link(routename, It.IsAny<object>())).Returns(result);

            return this;
        }

        public IUrlHelper Build() => _mock.Object;
    }
}
