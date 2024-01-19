using Ballast.Todo.Application.Contracts.Identity;
using Moq;

namespace Ballast.Todo.Tests.Mocks.Services
{
    public class MockJwtService : Mock<IJwtService>
    {
        public MockJwtService MockGenerateJwtFor((string, string) result)
        {
            Setup(x => x.GenerateJwtFor(It.IsAny<string>(), It.IsAny<string>())).Returns(result);
            return this;
        }
        public MockJwtService VerifyGenerateJwtFor(Times times)
        {
            Verify(x => x.GenerateJwtFor(It.IsAny<string>(), It.IsAny<string>()), times);
            return this;
        }
    }
}
