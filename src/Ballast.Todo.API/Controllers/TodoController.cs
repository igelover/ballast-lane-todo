using Ballast.Todo.Application.Contracts.Services;
using Ballast.Todo.Domain.DTO;
using Ballast.Todo.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ballast.Todo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("ApiUser")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;
        private readonly ILogger<TodoController> _logger;

        /// <summary>
        /// Default constructor receiving an <see cref="ITodoService"/>
        /// </summary>
        /// <param name="todoService">The <see cref="ITodoService"/> to use</param>
        /// <param name="logger">The logger</param>
        public TodoController(ITodoService todoService, ILogger<TodoController> logger)
        {
            _todoService = todoService;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint to create a new ToDo item
        /// </summary>
        /// <param name="userId">The user creating the item</param>
        /// <param name="item">The item to be created</param>
        /// <returns>The newly created <see cref="TodoItem"/></returns>
        [HttpPost("{userId}")]
        [ProducesResponseType(typeof(TodoItem), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<User>> CreateItemAsync([FromRoute] string userId, [FromBody] TodoItemDTO item)
        {
            _logger.LogDebug("Received request to create new item for user {user}", userId);
            var result = await _todoService.CreateItemAsync(userId, item);
            return Ok(result);
        }

        /// <summary>
        /// Returns a list of items associated to the given user
        /// </summary>
        /// <param name="userId">The user requesting the items</param>
        /// <returns>A list of ToDo items</returns>
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(IEnumerable<TodoItem>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetAllItemsAsync([FromRoute] string userId)
        {
            _logger.LogDebug("Received request to retrieve all items for user {user}", userId);
            var items = await _todoService.GetAllItemsAsync(userId);
            return Ok(items);
        }

        /// <summary>
        /// Returns the item associated to the given user with the given ID
        /// </summary>
        /// <param name="userId">The user requesting the item</param>
        /// <param name="id">The item ID</param>
        /// <returns>A ToDo item with the given ID</returns>
        [HttpGet("{userId}/{id}")]
        [ProducesResponseType(typeof(TodoItem), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetItemAsync([FromRoute] string userId, [FromRoute] string id)
        {
            _logger.LogDebug("Received request to retrieve item {item} for user {user}", id, userId);
            var item = await _todoService.GetItemAsync(id, userId);
            return Ok(item);
        }

        /// <summary>
        /// Endpoint to update an existing ToDo item
        /// </summary>
        /// <param name="userId">The user owner of the item</param>
        /// <param name="id">The item ID</param>
        /// <param name="item">The item to be updated</param>
        /// <returns>The update <see cref="TodoItem"/></returns>
        [HttpPut("{userId}/{id}")]
        [ProducesResponseType(typeof(TodoItem), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<User>> UpdateItemAsync([FromRoute] string id, [FromRoute] string userId, [FromBody] TodoItem item)
        {
            _logger.LogDebug("Received request to update item {item} for user {user}", id, userId);
            var result = await _todoService.UpdateItemAsync(id, userId, item);
            return Ok(result);
        }

        /// <summary>
        /// Endpoint to delete an existing ToDo item
        /// </summary>
        /// <param name="userId">The user owner of the item</param>
        /// <param name="id">The item ID</param>
        [HttpDelete("{userId}/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<User>> DeleteItemAsync([FromRoute] string id, [FromRoute] string userId)
        {
            _logger.LogDebug("Received request to delete item {item} for user {user}", id, userId);
            await _todoService.DeleteItemAsync(id, userId);
            return NoContent();
        }

        /// <summary>
        /// Endpoint to mark an existing ToDo item as Done
        /// </summary>
        /// <param name="userId">The user owner of the item</param>
        /// <param name="id">The item ID</param>
        [HttpPost("{userId}/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<User>> MarkItemDoneAsync([FromRoute] string id, [FromRoute] string userId)
        {
            _logger.LogDebug("Received request to mark item {item} done for user {user}", id, userId);
            await _todoService.MarkItemDoneAsync(id, userId);
            return NoContent();
        }
    }
}
