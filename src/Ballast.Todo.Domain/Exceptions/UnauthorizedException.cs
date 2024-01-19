namespace Ballast.Todo.Domain.Exceptions
{
    /// <summary>
    /// Exception used when the request has not been applied 
    /// because it lacks valid authentication credentials for the target resource
    /// </summary>
    /// <remarks>For example wrong username or password</remarks>
    public class UnauthorizedException : ApplicationException
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public UnauthorizedException()
        {
        }
    }
}
