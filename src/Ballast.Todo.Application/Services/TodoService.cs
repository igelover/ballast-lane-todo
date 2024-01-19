using Ballast.Todo.Application.Contracts.Persistence;
using Ballast.Todo.Application.Contracts.Services;
using Ballast.Todo.Domain.DTO;
using Ballast.Todo.Domain.Entities;
using Ballast.Todo.Domain.Exceptions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using ValidationException = Ballast.Todo.Domain.Exceptions.ValidationException;

namespace Ballast.Todo.Application.Services
{
    public class TodoService : ITodoService
    {
        private readonly IValidator<TodoItem> _validator;
        private readonly ITodoRepository _repository;
        private readonly ILogger<TodoService> _logger;

        public TodoService(IValidator<TodoItem> validator, ITodoRepository repository, ILogger<TodoService> logger)
        {
            _validator = validator;
            _repository = repository;
            _logger = logger;
        }

        public async Task<TodoItem> CreateItemAsync(string userId, TodoItemDTO item)
        {
            if (item is null)
            {
                throw new BadRequestException(nameof(item), "Item cannot be null");
            }

            var todoItem = new TodoItem
            {
                Description = item.Description,
                UserId = userId,
            };

            var result = await _validator.ValidateAsync(todoItem);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
            await _repository.CreateItemAsync(todoItem);
            _logger.LogDebug("Created item {item} for user {user}", todoItem.Id, userId);
            return todoItem;
        }

        public async Task<TodoItem> GetItemAsync(string id, string userId)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BadRequestException(nameof(id), "Cannot be null or whitespace");
            }
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new BadRequestException(nameof(userId), "Cannot be null or whitespace");
            }

            var item = await _repository.GetItemAsync(id, userId);
            _logger.LogDebug("Returning item {item} for user {user}", id, userId);
            return item is null ? throw new NotFoundException(nameof(item), id) : item;
        }

        public async Task<IEnumerable<TodoItem>> GetAllItemsAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new BadRequestException(nameof(userId), "Cannot be null or whitespace");
            }

            _logger.LogDebug("Returning all items for user {user}", userId);
            return await _repository.GetAllItemsAsync(userId);
        }

        public async Task<TodoItem> UpdateItemAsync(string id, string userId, TodoItem item)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BadRequestException(nameof(id), "Cannot be null or whitespace");
            }
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new BadRequestException(nameof(userId), "Cannot be null or whitespace");
            }
            if (item is null)
            {
                throw new BadRequestException(nameof(item), "Item cannot be null");
            }
            if (id != item.Id)
            {
                throw new BadRequestException(nameof(id), "Invaild item ID.");
            }

            var result = await _validator.ValidateAsync(item);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
            if(!await _repository.UpdateItemAsync(userId, item))
            {
                throw new NotFoundException("item", id);
            };
            _logger.LogDebug("Updated item {item} for user {user}", id, userId);
            return item;
        }

        public async Task DeleteItemAsync(string id, string userId)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BadRequestException(nameof(id), "Cannot be null or whitespace");
            }
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new BadRequestException(nameof(userId), "Cannot be null or whitespace");
            }
            if (!await _repository.DeleteItemAsync(id, userId))
            {
                throw new NotFoundException("item", id);
            };
            _logger.LogDebug("Deleted item {item} for user {user}", id, userId);
        }

        public async Task MarkItemDoneAsync(string id, string userId)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BadRequestException(nameof(id), "Cannot be null or whitespace");
            }
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new BadRequestException(nameof(userId), "Cannot be null or whitespace");
            }
            if (!await _repository.MarkItemDoneAsync(id, userId))
            {
                throw new NotFoundException("item", id);
            };
            _logger.LogDebug("Item {item} marked Done for user {user}", id, userId);
        }
    }
}
