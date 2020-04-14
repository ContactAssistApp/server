using System;
using System.Collections.Generic;

namespace CovidSafe.Entities.Validation
{
    /// <summary>
    /// Object validation summary
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Tells if the result passed (true if yes, false if no)
        /// </summary>
        public bool IsValid
        {
            get
            {
                return this.Failures.Count == 0;
            }
        }

        /// <summary>
        /// Collection of <see cref="ValidationFailure"/> objects
        /// </summary>
        public List<ValidationFailure> Failures { get; set; } = new List<ValidationFailure>();

        /// <summary>
        /// Add <see cref="ValidationResult"/> failures to this object
        /// </summary>
        public void AddRange(ValidationResult other)
        {
            this.Failures.AddRange(other.Failures);
        }

        /// <summary>
        /// Report a new <see cref="ValidationFailure"/>
        /// </summary>
        /// <param name="issue"><see cref="ValidationIssue"/> classification</param>
        /// <param name="property">Failing property name</param>
        /// <param name="message">Failure text</param>
        public void Fail(ValidationIssue issue, string property, string message)
        {
            this.Failures.Add(new ValidationFailure
            {
                Issue = issue,
                Message = message
            });
        }

        /// <summary>
        /// Report a new <see cref="ValidationFailure"/>
        /// </summary>
        /// <param name="issue"><see cref="ValidationIssue"/> classification</param>
        /// <param name="property">Failing property name</param>
        /// <param name="message">Failure text</param>
        /// <param name="args">Failure text arguments (akin to <see cref="String.Format(string, object[])"/></param>
        public void Fail(ValidationIssue issue, string property, string message, params string[] args)
        {
            // Use overload
            this.Fail(issue, property, String.Format(message, args));
        }
    }
}