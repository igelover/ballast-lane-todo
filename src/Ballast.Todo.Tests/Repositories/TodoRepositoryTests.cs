using Ballast.Todo.Domain.Entities;
using Ballast.Todo.Infrastructure.Database;
using Ballast.Todo.Infrastructure.Database.Repositories;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;

namespace Ballast.Todo.Tests.Repositories
{
    public class TodoRepositoryTests
    {
        private readonly ILogger<TodoRepository> _logger;

        public TodoRepositoryTests()
        {
            _logger = new LoggerFactory().CreateLogger<TodoRepository>();
        }

        [Fact]
        public async Task TodoRepository_CreateItemAsync_Valid_Success()
        {
            var item = new TodoItem
            {
                UserId = Guid.NewGuid().ToString()
            };
            var mockCollection = new Mock<IMongoCollection<TodoItem>>();
            var mockContext = new Mock<IDataContext>();
            mockContext.Setup(m => m.TodoItems).Returns(mockCollection.Object);

            var repository = new TodoRepository(mockContext.Object, _logger);

            await repository.CreateItemAsync(item);

            mockCollection.Verify(m => m.InsertOneAsync(It.IsAny<TodoItem>(),
                                                        It.IsAny<InsertOneOptions>(),
                                                        It.IsAny<CancellationToken>()),
                                                        Times.Once());
        }

        // For me this is the best way to test this, but unfortunately it's not working at this moment.
        // For some reason the collection loses track of the items already in it. Seems like a bug to me.
        //[Fact]
        //public async Task TodoRepository_GetItemAsync_Valid_Success()
        //{
        //    var itemId = Guid.NewGuid().ToString();
        //    var userId = Guid.NewGuid().ToString();
        //    var data = new List<TodoItem>
        //    {
        //        new() { Id = itemId, Description = "AAA", UserId = userId },
        //        new() { Description = "BBB", UserId = userId },
        //        new() { Description = "ZZZ", UserId = userId },
        //    }
        //    .AsQueryable();

        //    var mockCollection = new Mock<IMongoCollection<TodoItem>>();
        //    mockCollection.As<IQueryable<TodoItem>>().Setup(m => m.Provider).Returns(data.Provider);
        //    mockCollection.As<IQueryable<TodoItem>>().Setup(m => m.Expression).Returns(data.Expression);
        //    mockCollection.As<IQueryable<TodoItem>>().Setup(m => m.ElementType).Returns(data.ElementType);
        //    mockCollection.As<IQueryable<TodoItem>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        //    var mockContext = new Mock<DataContext>();
        //    mockContext.Setup(c => c.TodoItems).Returns(mockCollection.Object);

        //    var repository = new TodoRepository(mockContext.Object, _logger);
        //    var item = await repository.GetItemAsync(itemId, userId);

        //    Assert.NotNull(item);
        //}

        [Fact]
        public async Task TodoRepository_GetItemAsync_Valid_Success()
        {
            var data = new List<TodoItem>
            {
                new() { Description = "AAA", UserId = Guid.NewGuid().ToString() },
                new() { Description = "BBB", UserId = Guid.NewGuid().ToString() }
            };

            var cursor = new Mock<IAsyncCursor<TodoItem>>();
            cursor.Setup(_ => _.Current).Returns(data);
            cursor.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                  .Returns(true)
                  .Returns(false);
            cursor.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                  .Returns(Task.FromResult(true))
                  .Returns(Task.FromResult(false));

            var mockCollection = new Mock<IMongoCollection<TodoItem>>();
            mockCollection.Setup(op => op.FindAsync(It.IsAny<FilterDefinition<TodoItem>>(),
                                             It.IsAny<FindOptions<TodoItem, TodoItem>>(),
                                             It.IsAny<CancellationToken>())).ReturnsAsync(cursor.Object);


            var mockContext = new Mock<DataContext>();
            mockContext.Setup(c => c.TodoItems).Returns(mockCollection.Object);

            var repository = new TodoRepository(mockContext.Object, _logger);

            var result = await repository.GetItemAsync(data[0].Id, data[0].UserId);

            Assert.NotNull(result);
            Assert.Equal(data[0].Description, result.Description);
        }

        [Fact]
        public async Task TodoRepository_GetAllItemsAsync_Empty_Success()
        {
            var userId = Guid.NewGuid().ToString();
            var data = new List<TodoItem>
            {
                new() { Description = "AAA", UserId = userId },
                new() { Description = "BBB", UserId = userId }
            };

            var cursor = new Mock<IAsyncCursor<TodoItem>>();
            cursor.Setup(_ => _.Current).Returns(data);
            cursor.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                  .Returns(true)
                  .Returns(false);
            cursor.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                  .Returns(Task.FromResult(true))
                  .Returns(Task.FromResult(false));

            var mockCollection = new Mock<IMongoCollection<TodoItem>>();
            mockCollection.Setup(op => op.FindAsync(It.IsAny<FilterDefinition<TodoItem>>(),
                                             It.IsAny<FindOptions<TodoItem, TodoItem>>(),
                                             It.IsAny<CancellationToken>())).ReturnsAsync(cursor.Object);

            var mockContext = new Mock<DataContext>();
            mockContext.Setup(c => c.TodoItems).Returns(mockCollection.Object);

            var logger = new LoggerFactory().CreateLogger<TodoRepository>();
            var repository = new TodoRepository(mockContext.Object, logger);

            var result = await repository.GetAllItemsAsync(userId);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task TodoRepository_UpdateItemAsync_Valid_Success()
        {
            var data = new List<TodoItem>
            {
                new() { Id = Guid.NewGuid().ToString(), UserId = Guid.NewGuid().ToString() }
            };

            var cursor = new Mock<IAsyncCursor<TodoItem>>();
            cursor.Setup(_ => _.Current).Returns(data);
            cursor.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                  .Returns(true)
                  .Returns(false);
            cursor.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                  .Returns(Task.FromResult(true))
                  .Returns(Task.FromResult(false));

            var mockCollection = new Mock<IMongoCollection<TodoItem>>();
            mockCollection.Setup(op => op.ReplaceOneAsync(It.IsAny<FilterDefinition<TodoItem>>(),
                                                          It.IsAny<TodoItem>(),
                                                          It.IsAny<ReplaceOptions>(),
                                                          It.IsAny<CancellationToken>()));
            var mockContext = new Mock<IDataContext>();
            mockContext.Setup(m => m.TodoItems).Returns(mockCollection.Object);

            var repository = new TodoRepository(mockContext.Object, _logger);

            await repository.UpdateItemAsync(data[0].UserId, data[0]);

            mockCollection.Verify(m => m.ReplaceOneAsync(It.IsAny<FilterDefinition<TodoItem>>(),
                                                         It.IsAny<TodoItem>(),
                                                         It.IsAny<ReplaceOptions>(),
                                                         It.IsAny<CancellationToken>()),
                                                         Times.Once());
        }

        [Fact]
        public async Task TodoRepository_DeleteItemAsync_Valid_Success()
        {
            var data = new List<TodoItem>
            {
                new() { Id = Guid.NewGuid().ToString(), UserId = Guid.NewGuid().ToString() }
            };

            var cursor = new Mock<IAsyncCursor<TodoItem>>();
            cursor.Setup(_ => _.Current).Returns(data);
            cursor.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                  .Returns(true)
                  .Returns(false);
            cursor.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                  .Returns(Task.FromResult(true))
                  .Returns(Task.FromResult(false));

            var mockCollection = new Mock<IMongoCollection<TodoItem>>();
            mockCollection.Setup(op => op.DeleteOneAsync(It.IsAny<FilterDefinition<TodoItem>>(),
                                                         It.IsAny<CancellationToken>()));
            var mockContext = new Mock<IDataContext>();
            mockContext.Setup(m => m.TodoItems).Returns(mockCollection.Object);

            var repository = new TodoRepository(mockContext.Object, _logger);

            await repository.DeleteItemAsync(data[0].Id, data[0].UserId);

            mockCollection.Verify(m => m.DeleteOneAsync(It.IsAny<FilterDefinition<TodoItem>>(),
                                                         It.IsAny<CancellationToken>()),
                                                        Times.Once());
        }

        [Fact]
        public async Task TodoRepository_MarkItemDoneAsync_Valid_Success()
        {
            var data = new List<TodoItem>
            {
                new() { Id = Guid.NewGuid().ToString(), UserId = Guid.NewGuid().ToString() }
            };

            var cursor = new Mock<IAsyncCursor<TodoItem>>();
            cursor.Setup(_ => _.Current).Returns(data);
            cursor.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                  .Returns(true)
                  .Returns(false);
            cursor.SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                  .Returns(Task.FromResult(true))
                  .Returns(Task.FromResult(false));

            var mockCollection = new Mock<IMongoCollection<TodoItem>>();
            mockCollection.Setup(op => op.UpdateOneAsync(It.IsAny<FilterDefinition<TodoItem>>(),
                                                         It.IsAny<UpdateDefinition<TodoItem>>(),
                                                         It.IsAny<UpdateOptions>(),
                                                         It.IsAny<CancellationToken>()));
            var mockContext = new Mock<IDataContext>();
            mockContext.Setup(m => m.TodoItems).Returns(mockCollection.Object);

            var repository = new TodoRepository(mockContext.Object, _logger);

            await repository.MarkItemDoneAsync(data[0].Id, data[0].UserId);

            mockCollection.Verify(m => m.UpdateOneAsync(It.IsAny<FilterDefinition<TodoItem>>(),
                                                        It.IsAny<UpdateDefinition<TodoItem>>(),
                                                        It.IsAny<UpdateOptions>(),
                                                        It.IsAny<CancellationToken>()),
                                                        Times.Once());
        }
    }
}
