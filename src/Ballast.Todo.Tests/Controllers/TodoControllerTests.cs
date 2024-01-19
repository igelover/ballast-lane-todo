using Ballast.Todo.API.Controllers;
using Ballast.Todo.Domain.DTO;
using Ballast.Todo.Domain.Entities;
using Ballast.Todo.Tests.Mocks.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Ballast.Todo.Tests.Controllers
{
    public class TodoControllerTests
    {
        private readonly ILogger<TodoController> _logger;

        public TodoControllerTests()
        {
            _logger = new LoggerFactory().CreateLogger<TodoController>();
        }

        [Fact]
        public async Task TodoController_CreateItemAsync_ValidRequest_Success()
        {
            var userId = Guid.NewGuid().ToString();
            var item = new TodoItem
            {
                Description = "Test",
                UserId = userId
            };
            var itemDTO = new TodoItemDTO
            {
                Description = "Test"
            };

            var mockTodoService = new MockTodoService().MockCreateItemAsync(item);
            var todoController = new TodoController(mockTodoService.Object, _logger);

            var actionResult = await todoController.CreateItemAsync(userId, itemDTO);

            var result = actionResult.Result as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);

            mockTodoService.VerifyCreateItemAsync(Times.Once());
        }

        [Fact]
        public async Task TodoController_GetAllItemsAsync_ValidRequest_Success()
        {
            var userId = Guid.NewGuid().ToString();
            var items = new List<TodoItem>
            {
                new()
            };

            var mockTodoService = new MockTodoService().MockGetAllItemsAsync(items);
            var todoController = new TodoController(mockTodoService.Object, _logger);

            var actionResult = await todoController.GetAllItemsAsync(userId);

            var result = actionResult.Result as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);

            mockTodoService.VerifyGetAllItemsAsync(Times.Once());
        }

        [Fact]
        public async Task TodoController_GetItemAsync_ValidRequest_Success()
        {
            var userId = Guid.NewGuid().ToString();
            var itemId = Guid.NewGuid().ToString();
            var item = new TodoItem
            {
                Id = itemId,
                Description = "Test",
                UserId = userId
            };

            var mockTodoService = new MockTodoService().MockGetItemAsync(item);
            var todoController = new TodoController(mockTodoService.Object, _logger);

            var actionResult = await todoController.GetItemAsync(userId, itemId);

            var result = actionResult.Result as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);

            mockTodoService.VerifyGetItemAsync(Times.Once());
        }

        [Fact]
        public async Task TodoController_UpdateItemAsync_ValidRequest_Success()
        {
            var userId = Guid.NewGuid().ToString();
            var itemId = Guid.NewGuid().ToString();
            var item = new TodoItem
            {
                Id = itemId,
                Description = "Test",
                UserId = userId
            };

            var mockTodoService = new MockTodoService().MockUpdateItemAsync(item);
            var todoController = new TodoController(mockTodoService.Object, _logger);

            var actionResult = await todoController.UpdateItemAsync(userId, itemId, item);

            var result = actionResult.Result as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);

            mockTodoService.VerifyUpdateItemAsync(Times.Once());
        }

        [Fact]
        public async Task TodoController_DeleteItemAsync_ValidRequest_Success()
        {
            var userId = Guid.NewGuid().ToString();
            var itemId = Guid.NewGuid().ToString();

            var mockTodoService = new MockTodoService().MockDeleteItemAsync();
            var todoController = new TodoController(mockTodoService.Object, _logger);

            var actionResult = await todoController.DeleteItemAsync(userId, itemId);

            var result = actionResult.Result as NoContentResult;
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);

            mockTodoService.VerifyDeleteItemAsync(Times.Once());
        }

        [Fact]
        public async Task TodoController_MarkItemDoneAsync_ValidRequest_Success()
        {
            var userId = Guid.NewGuid().ToString();
            var itemId = Guid.NewGuid().ToString();

            var mockTodoService = new MockTodoService().MockMarkItemDoneAsync();
            var todoController = new TodoController(mockTodoService.Object, _logger);

            var actionResult = await todoController.MarkItemDoneAsync(userId, itemId);

            var result = actionResult.Result as NoContentResult;
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);

            mockTodoService.VerifyMarkItemDoneAsync(Times.Once());
        }
    }
}
