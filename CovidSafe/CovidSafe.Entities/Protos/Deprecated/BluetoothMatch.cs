using CovidSafe.Entities.Validation;

namespace CovidSafe.Entities.Protos.Deprecated
{
    [global::ProtoBuf.ProtoContract()]
    public partial class BluetoothMatch : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"user_message")]
        [global::System.ComponentModel.DefaultValue("")]
        public string UserMessage { get; set; } = "";

        [global::ProtoBuf.ProtoMember(2, Name = @"seeds")]
        public global::System.Collections.Generic.List<BlueToothSeed> Seeds { get; } = new global::System.Collections.Generic.List<BlueToothSeed>();

    }

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
            if(this.Seeds.Count > 0)
            {
                foreach(BlueToothSeed seed in this.Seeds)
                {
                    // Use Validate() from BlueToothSeed
                    result.Combine(seed.Validate());
                }
            }

            return result;
        }
    }
}
