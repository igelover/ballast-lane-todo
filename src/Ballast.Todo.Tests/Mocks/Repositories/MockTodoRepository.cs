using Ballast.Todo.Application.Contracts.Persistence;
using Ballast.Todo.Domain.Entities;
using Moq;

namespace Ballast.Todo.Tests.Mocks.Repositories
{
    public class MockTodoRepository : Mock<ITodoRepository>
    {
        public MockTodoRepository MockCreateItemAsync()
        {
            Setup(x => x.CreateItemAsync(It.IsAny<TodoItem>()));
            return this;
        }
        public MockTodoRepository VerifyCreateItemAsync(Times times)
        {
            Verify(x => x.CreateItemAsync(It.IsAny<TodoItem>()), times);
            return this;
        }

        public MockTodoRepository MockGetItemAsync(TodoItem item)
        {
            Setup(x => x.GetItemAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(item);
            return this;
        }
        public MockTodoRepository VerifyGetItemAsync(Times times)
        {
            Verify(x => x.GetItemAsync(It.IsAny<string>(), It.IsAny<string>()), times);
            return this;
        }

        public MockTodoRepository MockGetAllItemsAsync(IEnumerable<TodoItem> items)
        {
            Setup(x => x.GetAllItemsAsync(It.IsAny<string>())).ReturnsAsync(items);
            return this;
        }
        public MockTodoRepository VerifyGetAllItemsAsync(Times times)
        {
            Verify(x => x.GetAllItemsAsync(It.IsAny<string>()), times);
            return this;
        }

        public MockTodoRepository MockUpdateItemAsync(bool result)
        {
            Setup(x => x.UpdateItemAsync(It.IsAny<string>(), It.IsAny<TodoItem>())).ReturnsAsync(result);
            return this;
        }
        public MockTodoRepository VerifyUpdateItemAsync(Times times)
        {
            Verify(x => x.UpdateItemAsync(It.IsAny<string>(), It.IsAny<TodoItem>()), times);
            return this;
        }

        public MockTodoRepository MockDeleteItemAsync(bool result)
        {
            Setup(x => x.DeleteItemAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(result);
            return this;
        }
        public MockTodoRepository VerifyDeleteItemAsync(Times times)
        {
            Verify(x => x.DeleteItemAsync(It.IsAny<string>(), It.IsAny<string>()), times);
            return this;
        }

        public MockTodoRepository MockMarkItemDoneAsync(bool result)
        {
            Setup(x => x.MarkItemDoneAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(result);
            return this;
        }
        public MockTodoRepository VerifyMarkItemDoneAsync(Times times)
        {
            Verify(x => x.MarkItemDoneAsync(It.IsAny<string>(), It.IsAny<string>()), times);
            return this;
        }
    }
}
