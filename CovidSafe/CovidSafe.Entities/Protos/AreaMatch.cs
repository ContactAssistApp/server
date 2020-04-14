using CovidSafe.Entities.Validation;

namespace CovidSafe.Entities.Protos
{
    /// <summary>
    /// <see cref="AreaMatch"/> partial from generated Protobuf class
    /// </summary>
    public partial class AreaMatch : IValidatable
    {
        /// <inheritdoc/>
        public ValidationResult Validate()
        {
            ValidationResult result = new ValidationResult();

            // Only validate if collection contains areas
            if(this.Areas.Count > 0)
            {
                // Validate individual areas
                foreach(Area area in this.Areas)
                {
                    // Use Area.Validate()
                    result.AddRange(area.Validate());
                }
            }

            return result;
        }
    }
}
