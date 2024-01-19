using Ballast.Todo.Application.Services;
using Ballast.Todo.Application.Validators;
using Ballast.Todo.Domain.DTO;
using Ballast.Todo.Domain.Entities;
using Ballast.Todo.Domain.Exceptions;
using Ballast.Todo.Tests.Mocks.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace Ballast.Todo.Tests.Services
{
    public class TodoServiceTests
    {
        private const string EMPTY = "";
        private readonly ILogger<TodoService> _logger;

        public TodoServiceTests()
        {
            _logger = new LoggerFactory().CreateLogger<TodoService>();
        }

        [Fact]
        public async Task TodoService_CreateItemAsync_Valid_Success()
        {
            var userId = Guid.NewGuid().ToString();
            var item = new TodoItemDTO
            {
                Description = "Test"
            };

            var mockTodoRepo = new MockTodoRepository().MockCreateItemAsync();
            var todoService = new TodoService(new TodoItemValidator(), mockTodoRepo.Object, _logger);

            var newItem = await todoService.CreateItemAsync(userId, item);

            Assert.NotNull(newItem);
            mockTodoRepo.VerifyCreateItemAsync(Times.Once());
        }

        [Fact]
        public async Task TodoService_CreateItemAsync_Null_Failure()
        {
            var userId = Guid.NewGuid().ToString();

            var mockTodoRepo = new MockTodoRepository().MockCreateItemAsync();
            var todoService = new TodoService(new TodoItemValidator(), mockTodoRepo.Object, _logger);

            await Assert.ThrowsAsync<BadRequestException>(() => todoService.CreateItemAsync(userId, null));

            mockTodoRepo.VerifyCreateItemAsync(Times.Never());
        }

        [Fact]
        public async Task TodoService_CreateItemAsync_Invalid_Failure()
        {
            var userId = Guid.NewGuid().ToString();
            var item = new TodoItemDTO();

            var mockTodoRepo = new MockTodoRepository().MockCreateItemAsync();
            var todoService = new TodoService(new TodoItemValidator(), mockTodoRepo.Object, _logger);

            var ex = await Assert.ThrowsAsync<ValidationException>(() => todoService.CreateItemAsync(userId, item));

            Assert.NotEmpty(ex.Errors);
            mockTodoRepo.VerifyCreateItemAsync(Times.Never());
        }

        [Fact]
        public async Task TodoService_GetItemAsync_Valid_Success()
        {
            var itemId = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();
            var item = new TodoItem();

            var mockTodoRepo = new MockTodoRepository().MockGetItemAsync(item);
            var todoService = new TodoService(new TodoItemValidator(), mockTodoRepo.Object, _logger);

            var existingItem = await todoService.GetItemAsync(itemId, userId);

            Assert.NotNull(existingItem);
            mockTodoRepo.VerifyGetItemAsync(Times.Once());
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(EMPTY, EMPTY)]
        [InlineData("123", EMPTY)]
        [InlineData(EMPTY, "123")]
        [InlineData("123", null)]
        [InlineData(null, "123")]
        public async Task TodoService_GetItemAsync_Invalid_Faliure(string itemId, string userId)
        {
            var mockTodoRepo = new MockTodoRepository();
            var todoService = new TodoService(new TodoItemValidator(), mockTodoRepo.Object, _logger);

            await Assert.ThrowsAsync<BadRequestException>(() => todoService.GetItemAsync(itemId, userId));

            mockTodoRepo.VerifyGetItemAsync(Times.Never());
        }

        [Fact]
        public async Task TodoService_GetItemAsync_NotFound_Failure()
        {
            var itemId = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();

            var mockTodoRepo = new MockTodoRepository().MockGetItemAsync(null);
            var todoService = new TodoService(new TodoItemValidator(), mockTodoRepo.Object, _logger);

            await Assert.ThrowsAsync<NotFoundException>(() => todoService.GetItemAsync(itemId, userId));

            mockTodoRepo.VerifyGetItemAsync(Times.Once());
        }

        [Fact]
        public async Task TodoService_GetAllItemsAsync_Valid_Success()
        {
            var userId = Guid.NewGuid().ToString();
            var items = new List<TodoItem>
            {
                new()
            };

            var mockTodoRepo = new MockTodoRepository().MockGetAllItemsAsync(items);
            var todoService = new TodoService(new TodoItemValidator(), mockTodoRepo.Object, _logger);

            var existingItems = await todoService.GetAllItemsAsync(userId);

            Assert.NotNull(existingItems);
            mockTodoRepo.VerifyGetAllItemsAsync(Times.Once());
        }

        [Theory]
        [InlineData(null)]
        [InlineData(EMPTY)]
        public async Task TodoService_GetAllItemsAsync_Invalid_Faliure(string userId)
        {
            var mockTodoRepo = new MockTodoRepository();
            var todoService = new TodoService(new TodoItemValidator(), mockTodoRepo.Object, _logger);

            await Assert.ThrowsAsync<BadRequestException>(() => todoService.GetAllItemsAsync(userId));

            mockTodoRepo.VerifyGetAllItemsAsync(Times.Never());
        }

        [Fact]
        public async Task TodoService_UpdateItemAsync_Valid_Success()
        {
            var itemId = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();
            var item = new TodoItem
            {
                Id = itemId,
                Description = "Test",
                UserId = userId
            };

            var mockTodoRepo = new MockTodoRepository().MockUpdateItemAsync(true);
            var todoService = new TodoService(new TodoItemValidator(), mockTodoRepo.Object, _logger);

            var updatedItem = await todoService.UpdateItemAsync(itemId, userId, item);

            Assert.NotNull(updatedItem);
            mockTodoRepo.VerifyUpdateItemAsync(Times.Once());
        }

        public static IEnumerable<object[]> GetParameters()
        {
            var item = new TodoItem();

            return new List<object[]>
            {
                new object[] { null, null, null },
                new object[] { EMPTY, EMPTY, null },
                new object[] { "123", EMPTY, null },
                new object[] { EMPTY, "123", null },
                new object[] { "123", null, null },
                new object[] { null, "123", null },
                new object[] { null, null, item },
                new object[] { EMPTY, EMPTY, item },
                new object[] { "123", EMPTY, item },
                new object[] { EMPTY, "123", item },
                new object[] { "123", null, item },
                new object[] { null, "123", item },
                new object[] { "123", "123", null },
                new object[] { "123", "123", new TodoItem { Id = "321" } }
            };
        }

        [Theory]
        [MemberData(nameof(GetParameters))]
        public async Task TodoService_UpdateItemAsync_InvalidParams_Failure(string itemId, string userId, TodoItem item)
        {
            var mockTodoRepo = new MockTodoRepository().MockUpdateItemAsync(true);
            var todoService = new TodoService(new TodoItemValidator(), mockTodoRepo.Object, _logger);

            await Assert.ThrowsAsync<BadRequestException>(() => todoService.UpdateItemAsync(itemId, userId, item));

            mockTodoRepo.VerifyUpdateItemAsync(Times.Never());
        }

        [Fact]
        public async Task TodoService_UpdateItemAsync_InvalidItem_Failure()
        {
            var itemId = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();
            var item = new TodoItem { Id = itemId };

            var mockTodoRepo = new MockTodoRepository().MockUpdateItemAsync(true);
            var todoService = new TodoService(new TodoItemValidator(), mockTodoRepo.Object, _logger);

            var ex = await Assert.ThrowsAsync<ValidationException>(() => todoService.UpdateItemAsync(itemId, userId, item));

            Assert.NotEmpty(ex.Errors);
            mockTodoRepo.VerifyUpdateItemAsync(Times.Never());
        }

        [Fact]
        public async Task TodoService_UpdateItemAsync_NotFound_Failure()
        {
            var itemId = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();
            var item = new TodoItem
            {
                Id = itemId,
                Description = "Test",
                UserId = userId
            };

            var mockTodoRepo = new MockTodoRepository().MockUpdateItemAsync(false);
            var todoService = new TodoService(new TodoItemValidator(), mockTodoRepo.Object, _logger);

            await Assert.ThrowsAsync<NotFoundException>(() => todoService.UpdateItemAsync(itemId, userId, item));

            mockTodoRepo.VerifyUpdateItemAsync(Times.Once());
        }

        [Fact]
        public async Task TodoService_DeleteItemAsync_Valid_Success()
        {
            var itemId = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();

            var mockTodoRepo = new MockTodoRepository().MockDeleteItemAsync(true);
            var todoService = new TodoService(new TodoItemValidator(), mockTodoRepo.Object, _logger);

            await todoService.DeleteItemAsync(itemId, userId);

            mockTodoRepo.VerifyDeleteItemAsync(Times.Once());
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(EMPTY, EMPTY)]
        [InlineData("123", EMPTY)]
        [InlineData(EMPTY, "123")]
        [InlineData("123", null)]
        [InlineData(null, "123")]
        public async Task TodoService_DeleteItemAsync_InvalidParams_Failure(string itemId, string userId)
        {
            var mockTodoRepo = new MockTodoRepository().MockDeleteItemAsync(true);
            var todoService = new TodoService(new TodoItemValidator(), mockTodoRepo.Object, _logger);

            await Assert.ThrowsAsync<BadRequestException>(() => todoService.DeleteItemAsync(itemId, userId));

            mockTodoRepo.VerifyDeleteItemAsync(Times.Never());
        }

        [Fact]
        public async Task TodoService_DeleteItemAsync_NotFound_Failure()
        {
            var itemId = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();

            var mockTodoRepo = new MockTodoRepository().MockDeleteItemAsync(false);
            var todoService = new TodoService(new TodoItemValidator(), mockTodoRepo.Object, _logger);

            await Assert.ThrowsAsync<NotFoundException>(() => todoService.DeleteItemAsync(itemId, userId));

            mockTodoRepo.VerifyDeleteItemAsync(Times.Once());
        }

        [Fact]
        public async Task TodoService_MarkItemDoneAsync_Valid_Success()
        {
            var itemId = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();

            var mockTodoRepo = new MockTodoRepository().MockMarkItemDoneAsync(true);
            var todoService = new TodoService(new TodoItemValidator(), mockTodoRepo.Object, _logger);

            await todoService.MarkItemDoneAsync(itemId, userId);

            mockTodoRepo.VerifyMarkItemDoneAsync(Times.Once());
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(EMPTY, EMPTY)]
        [InlineData("123", EMPTY)]
        [InlineData(EMPTY, "123")]
        [InlineData("123", null)]
        [InlineData(null, "123")]
        public async Task TodoService_MarkItemDoneAsync_InvalidParams_Failure(string itemId, string userId)
        {
            var mockTodoRepo = new MockTodoRepository().MockMarkItemDoneAsync(true);
            var todoService = new TodoService(new TodoItemValidator(), mockTodoRepo.Object, _logger);

            await Assert.ThrowsAsync<BadRequestException>(() => todoService.MarkItemDoneAsync(itemId, userId));

            mockTodoRepo.VerifyMarkItemDoneAsync(Times.Never());
        }

        [Fact]
        public async Task TodoService_MarkItemDoneAsync_NotFound_Failure()
        {
            var itemId = Guid.NewGuid().ToString();
            var userId = Guid.NewGuid().ToString();

            var mockTodoRepo = new MockTodoRepository().MockMarkItemDoneAsync(false);
            var todoService = new TodoService(new TodoItemValidator(), mockTodoRepo.Object, _logger);

            await Assert.ThrowsAsync<NotFoundException>(() => todoService.MarkItemDoneAsync(itemId, userId));

            mockTodoRepo.VerifyMarkItemDoneAsync(Times.Once());
        }
    }
}
