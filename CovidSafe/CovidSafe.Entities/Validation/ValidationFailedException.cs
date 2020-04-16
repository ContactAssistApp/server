using System;

namespace CovidSafe.Entities.Validation
{
    /// <summary>
    /// <see cref="Exception"/> used for failed <see cref="ValidationResult"/> objects
    /// </summary>
    public class ValidationFailedException : Exception
    {
        /// <summary>
        /// <see cref="ValidationResult"/> from process
        /// </summary>
        public ValidationResult ValidationResult { get; private set; }

        /// <summary>
        /// Creates a new <see cref="ValidationFailedException"/> instance
        /// </summary>
        /// <param name="validationResult"><see cref="ValidationResult"/></param>
        public ValidationFailedException(ValidationResult validationResult) : base()
        {
            this.ValidationResult = validationResult;
        }
    }
}