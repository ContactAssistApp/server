using CovidSafe.Entities.Validation;

namespace CovidSafe.Entities.Protos
{
    /// <summary>
    /// <see cref="BluetoothMatch"/> partial from generated Protobuf class
    /// </summary>
    public partial class BluetoothMatch : IValidatable
    {
        /// <inheritdoc/>
        public ValidationResult Validate()
        {
            ValidationResult result = new ValidationResult();

            // Only validate if collection contains seeds
            if(this.Seeds.Count > 0)
            {
                foreach(BlueToothSeed seed in this.Seeds)
                {
                    // Use Validate() from BlueToothSeed
                    result.AddRange(seed.Validate());
                }
            }

            return result;
        }
    }
}
