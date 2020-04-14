using System.Diagnostics.CodeAnalysis;

namespace CovidSafe.Entities.Validation
{
    /// <summary>
    /// Validation property name enumeration
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ValidationProperty
    {
        /// <summary>
        /// String reference used when multiple properties cause a 
        /// <see cref="ValidationFailure"/>
        /// </summary>
        public const string Multiple = "multiple";
    }
}
