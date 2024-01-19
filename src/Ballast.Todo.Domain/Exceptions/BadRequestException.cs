namespace Ballast.Todo.Domain.Exceptions
{
    /// <summary>
    /// Exception used to return validation error messages
    /// </summary>
    public class BadRequestException : ApplicationException
    {
        /// <summary>
        /// List of validation errors
        /// </summary>
        public List<ValidationErrorDetail> Errors { get; } = [];

        /// <summary>
        /// Default constructor (initalizes a custom base message)
        /// </summary>
        public BadRequestException() : base("One or more validations failed.")
        {
            Errors = [];
        }

        /// <summary>
        /// Constructor receiving a field name and an error message
        /// </summary>
        /// <param name="key">Field name</param>
        /// <param name="message">Error message</param>
        public BadRequestException(string key, string message) : this()
        {
            Errors.Add(new ValidationErrorDetail() { Key = key, Messages = new List<string>() { message } });
        }
    }

    /// <summary>
    /// A validation error detail, contains a field name (key) and a list of error messages.
    /// </summary>
    public class ValidationErrorDetail
    {
        /// <summary>
        /// Field name validated
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// List of error messages asociated with the field name
        /// </summary>
        public IList<string> Messages { get; set; } = [];
    }
}
