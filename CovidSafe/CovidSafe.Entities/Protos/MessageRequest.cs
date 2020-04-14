using CovidSafe.Entities.Validation;

namespace CovidSafe.Entities.Protos
{
    /// <summary>
    /// <see cref="MessageRequest"/> partial from generated Protobuf class
    /// </summary>
    public partial class MessageRequest : IValidatable
    {
        /// <inheritdoc/>
        public ValidationResult Validate()
        {
            ValidationResult result = new ValidationResult();

            // Only validate if collection contains message information
            if(this.RequestedQueries.Count > 0)
            {
                foreach(MessageInfo info in this.RequestedQueries)
                {
                    // Use Validate() method in MessageInfo
                    result.AddRange(info.Validate());
                }
            }

            return result;
        }
    }
}
