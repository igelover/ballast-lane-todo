namespace Ballast.Todo.Domain.Exceptions
{
    /// <summary>
    /// Exception used when the requested resource is not found
    /// </summary>
    public class NotFoundException : ApplicationException
    {
        /// <summary>
        /// Custom constructor receiving the entity name and ID.
        /// </summary>
        /// <param name="name">Entity name</param>
        /// <param name="key">ID</param>
        public NotFoundException(string name, string key) : base($"Entity '{name}' with ID '{key}' was not found.")
        {
        }
    }
}
