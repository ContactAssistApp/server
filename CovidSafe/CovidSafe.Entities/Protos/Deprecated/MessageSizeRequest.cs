using CovidSafe.Entities.Validation;
using CovidSafe.Entities.Validation.Resources;

#pragma warning disable CS1591, CS0612, CS3021, IDE1006
namespace CovidSafe.Entities.Protos.Deprecated
{
    [global::ProtoBuf.ProtoContract()]
    public partial class MessageSizeRequest : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"region")]
        public Region Region { get; set; }

        [global::ProtoBuf.ProtoMember(2, Name = @"last_query_time")]
        public long LastQueryTime { get; set; }

    }

    /// <summary>
    /// <see cref="MessageSizeRequest"/> partial from generated Protobuf class
    /// </summary>
    public partial class MessageSizeRequest : IValidatable
    {
        /// <inheritdoc/>
        public RequestValidationResult Validate()
        {
            RequestValidationResult result = new RequestValidationResult();

            // Validate timestamp
            result.Combine(Validator.ValidateTimestamp(this.LastQueryTime, parameterName: nameof(this.LastQueryTime)));

            // Validate region
            if(this.Region == null)
            {
                result.Fail(
                    RequestValidationIssue.InputNull,
                    nameof(this.Region),
                    ValidationMessages.NullRegion
                );
            }
            else
            {
                // Use Region.Validate()
                result.Combine(this.Region.Validate());
            }

            return result;
        }
    }
}
#pragma warning restore CS1591, CS0612, CS3021, IDE1006