using Ballast.Todo.Application.Contracts.Persistence;
using Ballast.Todo.Domain.Entities;
using Moq;

namespace Ballast.Todo.Tests.Mocks.Repositories
{
    public class MockSessionRepository : Mock<ISessionRepository>
    {
        public MockSessionRepository MockDeleteSessionsAsync(bool result)
        {
            Setup(x => x.DeleteSessionsAsync(It.IsAny<string>())).ReturnsAsync(result);
            return this;
        }
        public MockSessionRepository VerifyDeleteSessionsAsync(Times times)
        {
            Verify(x => x.DeleteSessionsAsync(It.IsAny<string>()), times);
            return this;
        }

        public MockSessionRepository MockCreateSessionAsync(bool result)
        {
            Setup(x => x.CreateSessionAsync(It.IsAny<Session>()));
            return this;
        }
        public MockSessionRepository VerifyCreateSessionAsync(Times times)
        {
            Verify(x => x.CreateSessionAsync(It.IsAny<Session>()), times);
            return this;
        }
    }
}
