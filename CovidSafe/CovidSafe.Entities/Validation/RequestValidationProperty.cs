using System.Diagnostics.CodeAnalysis;

namespace CovidSafe.Entities.Validation
{
    /// <summary>
    /// Request validation property name constants
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class RequestValidationProperty
    {
        /// <summary>
        /// String reference used when multiple properties cause a 
        /// <see cref="RequestValidationFailure"/>
        /// </summary>
        public const string Multiple = "multiple";
    }
}
