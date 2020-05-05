using CovidSafe.Entities.Validation;

namespace CovidSafe.Entities.v20200415.Protos
{
    /// <summary>
    /// <see cref="BluetoothMatch"/> partial from generated Protobuf class
    /// </summary>
    public partial class BluetoothMatch : IValidatable
    {
        /// <inheritdoc/>
        public RequestValidationResult Validate()
        {
            RequestValidationResult result = new RequestValidationResult();

            // Only validate if collection contains seeds
            if (this.Seeds.Count > 0)
            {
                foreach (BlueToothSeed seed in this.Seeds)
                {
                    // Use Validate() from BlueToothSeed
                    result.Combine(seed.Validate());
                }
            }

            return result;
        }
    }
}