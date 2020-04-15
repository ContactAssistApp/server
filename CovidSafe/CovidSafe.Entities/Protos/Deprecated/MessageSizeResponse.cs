#pragma warning disable CS1591, CS0612, CS3021, IDE1006
namespace CovidSafe.Entities.Protos.Deprecated
{
    [global::ProtoBuf.ProtoContract()]
    public partial class MessageSizeResponse : global::ProtoBuf.IExtensible
    {
        private global::ProtoBuf.IExtension __pbn__extensionData;
        global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
            => global::ProtoBuf.Extensible.GetExtensionObject(ref __pbn__extensionData, createIfMissing);

        [global::ProtoBuf.ProtoMember(1, Name = @"size_of_query_response")]
        public long SizeOfQueryResponse { get; set; }

    }
}
#pragma warning restore CS1591, CS0612, CS3021, IDE1006