using System;

namespace Lx.Db.Protobuf
{
    [ProtoBuf.ProtoContract]
    internal class TypeEnvelop<TKey>
    {
        public TypeEnvelop()
        {
        }

        public static string GetSignature(Type type)
        {
            return type.FullName;
        }


        public TypeEnvelop(TKey key, Type type, byte[] serializedData)
        {
            Key = key;
            TypeSignature = GetSignature(type);
            SerializedData = serializedData;
        }

        [ProtoBuf.ProtoMember(1)]
        public TKey Key { get; set; }

        [ProtoBuf.ProtoMember(2)]
        public string TypeSignature { get; set; }

        [ProtoBuf.ProtoMember(3)]
        public byte[] SerializedData { get; set; }

        /// <summary>
        /// Gets or sets the object deserialized instance.
        /// Only not empty after a first deserialization or during the session first add.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public object Instance { get; set; }
    }
}