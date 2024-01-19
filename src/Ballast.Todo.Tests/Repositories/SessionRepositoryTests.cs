using Ballast.Todo.Domain.Entities;
using Ballast.Todo.Infrastructure.Database;
using Ballast.Todo.Infrastructure.Database.Repositories;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;

namespace Ballast.Todo.Tests.Repositories
{
    public class SessionRepositoryTests
    {
        private readonly ILogger<SessionRepository> _logger;

        public SessionRepositoryTests()
        {
            _logger = new LoggerFactory().CreateLogger<SessionRepository>();
        }

        [Fact]
        public async Task SessionRepository_DeleteSessionsAsync_Valid_Success()
        {
            var data = new List<Session>
            {
                new() { UserId = Guid.NewGuid().ToString() }
            };

            var cursor = new Mock<IAsyncCursor<Session>>();
            cursor.Setup(_ => _.Current).Returns(data);
            cursor.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                  .Returns(true)
                  .Returns(false);
            cursor.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                  .Returns(Task.FromResult(true))
                  .Returns(Task.FromResult(false));

            var mockCollection = new Mock<IMongoCollection<Session>>();
            mockCollection.Setup(op => op.DeleteManyAsync(It.IsAny<FilterDefinition<Session>>(),
                                                         It.IsAny<CancellationToken>()));
            var mockContext = new Mock<IDataContext>();
            mockContext.Setup(m => m.Sessions).Returns(mockCollection.Object);

            var repository = new SessionRepository(mockContext.Object, _logger);

            await repository.DeleteSessionsAsync(data[0].UserId);

            mockCollection.Verify(m => m.DeleteManyAsync(It.IsAny<FilterDefinition<Session>>(),
                                                         It.IsAny<CancellationToken>()),
                                                        Times.Once());
        }

        [Fact]
        public async Task SessionRepository_CreateSessionAsync_Valid_Success()
        {
            var session = new Session
            {
                UserId = Guid.NewGuid().ToString()
            };
            var mockCollection = new Mock<IMongoCollection<Session>>();
            var mockContext = new Mock<IDataContext>();
            mockContext.Setup(m => m.Sessions).Returns(mockCollection.Object);

            var repository = new SessionRepository(mockContext.Object, _logger);

            await repository.CreateSessionAsync(session);

            mockCollection.Verify(m => m.InsertOneAsync(It.IsAny<Session>(),
                                                        It.IsAny<InsertOneOptions>(),
                                                        It.IsAny<CancellationToken>()),
                                                        Times.Once());
        }
    }
}
