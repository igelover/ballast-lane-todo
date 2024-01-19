namespace Ballast.Todo.Domain.Exceptions
{
    /// <summary>
    /// Exception used when the request could not be completed
    /// due to a conflict with the current state of the target resource
    /// </summary>
    /// <remarks>For example a concurrency exception</remarks>
    public class ConflictException : ApplicationException
    {
        /// <summary>
        /// Constructor that receives an exception message
        /// </summary>
        /// <param name="message">The exception message</param>
        public ConflictException(string message) : base(message)
        {
        }
    }
}
