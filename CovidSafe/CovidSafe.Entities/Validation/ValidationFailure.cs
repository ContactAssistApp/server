namespace CovidSafe.Entities.Validation
{
    /// <summary>
    /// Details of validation failures
    /// </summary>
    public class ValidationFailure
    {
        /// <summary>
        /// <see cref="ValidationIssue"/> classification
        /// </summary>
        public ValidationIssue Issue { get; set; }
        /// <summary>
        /// Validation failure detail message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Name of object property which failed validation
        /// </summary>
        public string Property { get; set; }
    }
}
