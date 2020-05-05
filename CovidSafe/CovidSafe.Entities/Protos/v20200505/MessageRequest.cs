using CovidSafe.Entities.Validation;

namespace CovidSafe.Entities.Protos.v20200505
{
    /// <summary>
    /// <see cref="MessageRequest"/> partial from generated Protobuf class
    /// </summary>
    public partial class MessageRequest : IValidatable
    {
        /// <inheritdoc/>
        public RequestValidationResult Validate()
        {
            RequestValidationResult result = new RequestValidationResult();

            // Only validate if collection contains message information
            if(this.RequestedQueries.Count > 0)
            {
                foreach(MessageInfo info in this.RequestedQueries)
                {
                    // Use Validate() method in MessageInfo
                    result.Combine(info.Validate());
                }
            }

            return result;
        }
    }
}
