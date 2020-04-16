namespace CovidSafe.Entities.Validation
{
    /// <summary>
    /// Object is validatable contract
    /// </summary>
    public interface IValidatable
    {
        /// <summary>
        /// Determines if the current object is valid
        /// </summary>
        /// <returns><see cref="RequestValidationResult"/> summary</returns>
        RequestValidationResult Validate();
    }
}
