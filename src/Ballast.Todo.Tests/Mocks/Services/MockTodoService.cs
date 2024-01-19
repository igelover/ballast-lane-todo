using Ballast.Todo.Application.Contracts.Services;
using Ballast.Todo.Domain.DTO;
using Ballast.Todo.Domain.Entities;
using Moq;

namespace Ballast.Todo.Tests.Mocks.Services
{
    public class MockTodoService : Mock<ITodoService>
    {
        public MockTodoService MockCreateItemAsync(TodoItem item)
        {
            Setup(x => x.CreateItemAsync(It.IsAny<string>(), It.IsAny<TodoItemDTO>())).ReturnsAsync(item);
            return this;
        }
        public MockTodoService VerifyCreateItemAsync(Times times)
        {
            Verify(x => x.CreateItemAsync(It.IsAny<string>(), It.IsAny<TodoItemDTO>()), times);
            return this;
        }

        public MockTodoService MockGetItemAsync(TodoItem item)
        {
            Setup(x => x.GetItemAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(item);
            return this;
        }
        public MockTodoService VerifyGetItemAsync(Times times)
        {
            Verify(x => x.GetItemAsync(It.IsAny<string>(), It.IsAny<string>()), times);
            return this;
        }

        public MockTodoService MockGetAllItemsAsync(IEnumerable<TodoItem> items)
        {
            Setup(x => x.GetAllItemsAsync(It.IsAny<string>())).ReturnsAsync(items);
            return this;
        }
        public MockTodoService VerifyGetAllItemsAsync(Times times)
        {
            Verify(x => x.GetAllItemsAsync(It.IsAny<string>()), times);
            return this;
        }

        public MockTodoService MockUpdateItemAsync(TodoItem item)
        {
            Setup(x => x.UpdateItemAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TodoItem>())).ReturnsAsync(item);
            return this;
        }
        public MockTodoService VerifyUpdateItemAsync(Times times)
        {
            Verify(x => x.UpdateItemAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TodoItem>()), times);
            return this;
        }

        public MockTodoService MockDeleteItemAsync()
        {
            Setup(x => x.DeleteItemAsync(It.IsAny<string>(), It.IsAny<string>()));
            return this;
        }
        public MockTodoService VerifyDeleteItemAsync(Times times)
        {
            Verify(x => x.DeleteItemAsync(It.IsAny<string>(), It.IsAny<string>()), times);
            return this;
        }

        public MockTodoService MockMarkItemDoneAsync()
        {
            Setup(x => x.MarkItemDoneAsync(It.IsAny<string>(), It.IsAny<string>()));
            return this;
        }
        public MockTodoService VerifyMarkItemDoneAsync(Times times)
        {
            Verify(x => x.MarkItemDoneAsync(It.IsAny<string>(), It.IsAny<string>()), times);
            return this;
        }
    }
}
