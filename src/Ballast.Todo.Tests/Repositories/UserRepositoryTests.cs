using Ballast.Todo.Domain.Entities;
using Ballast.Todo.Domain.Exceptions;
using Ballast.Todo.Infrastructure.Database;
using Ballast.Todo.Infrastructure.Database.Repositories;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;

namespace Ballast.Todo.Tests.Repositories
{
    public class UserRepositoryTests
    {
        private readonly ILogger<UserRepository> _logger;

        public UserRepositoryTests()
        {
            _logger = new LoggerFactory().CreateLogger<UserRepository>();
        }

        [Fact]
        public async Task UserRepository_GetByEmailAsync_Valid_Success()
        {
            var data = new List<User>
            {
                new() { Email = "example@test.com" },
                new() { Email = "test@example.com" }
            };

            var cursor = new Mock<IAsyncCursor<User>>();
            cursor.Setup(_ => _.Current).Returns(data);
            cursor.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                  .Returns(true)
                  .Returns(false);
            cursor.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                  .Returns(Task.FromResult(true))
                  .Returns(Task.FromResult(false));

            var mockCollection = new Mock<IMongoCollection<User>>();
            mockCollection.Setup(op => op.FindAsync(It.IsAny<FilterDefinition<User>>(),
                                             It.IsAny<FindOptions<User, User>>(),
                                             It.IsAny<CancellationToken>())).ReturnsAsync(cursor.Object);


            var mockContext = new Mock<DataContext>();
            mockContext.Setup(c => c.Users).Returns(mockCollection.Object);

            var repository = new UserRepository(mockContext.Object, _logger);

            var result = await repository.GetByEmailAsync(data[0].Email);

            Assert.NotNull(result);
            Assert.Equal(data[0].Email, result.Email);
        }

        [Fact]
        public async Task UserRepository_RegisterUserAsync_Valid_Success()
        {
            var user = new User();
            var mockCollection = new Mock<IMongoCollection<User>>();
            var mockContext = new Mock<IDataContext>();
            mockContext.Setup(m => m.Users).Returns(mockCollection.Object);

            var repository = new UserRepository(mockContext.Object, _logger);

            await repository.RegisterUserAsync(user);

            mockCollection.Verify(m => m.InsertOneAsync(It.IsAny<User>(),
                                                        It.IsAny<InsertOneOptions>(),
                                                        It.IsAny<CancellationToken>()),
                                                        Times.Once());
        }

        [Fact]
        public async Task UserRepository_ValidateCredentialsAsync_Valid_Success()
        {
            var data = new List<User>
            {
                new() { Email = "example@test.com", Password = "123" },
                new() { Email = "test@example.com", Password = "321" }
            };

            var cursor = new Mock<IAsyncCursor<User>>();
            cursor.Setup(_ => _.Current).Returns(data);
            cursor.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                  .Returns(true)
                  .Returns(false);
            cursor.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                  .Returns(Task.FromResult(true))
                  .Returns(Task.FromResult(false));

            var mockCollection = new Mock<IMongoCollection<User>>();
            mockCollection.Setup(op => op.FindAsync(It.IsAny<FilterDefinition<User>>(),
                                             It.IsAny<FindOptions<User, User>>(),
                                             It.IsAny<CancellationToken>())).ReturnsAsync(cursor.Object);


            var mockContext = new Mock<DataContext>();
            mockContext.Setup(c => c.Users).Returns(mockCollection.Object);

            var repository = new UserRepository(mockContext.Object, _logger);

            var result = await repository.ValidateCredentialsAsync("example@test.com", "123");

            Assert.NotNull(result);
            Assert.Equal(data[0].Id, result);
        }

        [Fact]
        public async Task UserRepository_ValidateCredentialsAsync_Invalid_Failure()
        {
            var data = new List<User>();

            var cursor = new Mock<IAsyncCursor<User>>();
            cursor.Setup(_ => _.Current).Returns(data);
            cursor.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                  .Returns(true)
                  .Returns(false);
            cursor.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                  .Returns(Task.FromResult(true))
                  .Returns(Task.FromResult(false));

            var mockCollection = new Mock<IMongoCollection<User>>();
            mockCollection.Setup(op => op.FindAsync(It.IsAny<FilterDefinition<User>>(),
                                             It.IsAny<FindOptions<User, User>>(),
                                             It.IsAny<CancellationToken>())).ReturnsAsync(cursor.Object);


            var mockContext = new Mock<DataContext>();
            mockContext.Setup(c => c.Users).Returns(mockCollection.Object);

            var repository = new UserRepository(mockContext.Object, _logger);

            await Assert.ThrowsAsync<UnauthorizedException>(() => repository.ValidateCredentialsAsync("example@test.com", "123"));
        }
    }
}
