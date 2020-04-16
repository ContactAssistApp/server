using System;

namespace CovidSafe.Entities.Validation
{
    /// <summary>
    /// <see cref="Exception"/> used for failed <see cref="RequestValidationResult"/> objects
    /// </summary>
    public class RequestValidationFailedException : Exception
    {
        /// <summary>
        /// <see cref="RequestValidationResult"/> from process
        /// </summary>
        public RequestValidationResult ValidationResult { get; private set; }

        /// <summary>
        /// Creates a new <see cref="RequestValidationFailedException"/> instance
        /// </summary>
        /// <param name="validationResult"><see cref="RequestValidationResult"/></param>
        public RequestValidationFailedException(RequestValidationResult validationResult) : base()
        {
            this.ValidationResult = validationResult;
        }
    }
}