using System.Collections.Generic;
using ProtoBuf;

namespace Lx.Db.Protobuf
{
    [ProtoContract]
    internal class SerializableContainer
    {
        public SerializableContainer()
        {
            StringValues = new List<TypeEnvelop<string>>();
            LongValues = new List<TypeEnvelop<long>>();
        }

        public SerializableContainer(DatabaseContainer<long> longContainer, DatabaseContainer<string> stringContainer)
            :this()
        {
            StringValues = stringContainer.Values;
            LongValues = longContainer.Values;
        }
 
        [ProtoMember(1)]
        public List<TypeEnvelop<string>> StringValues { get; set; }
        
        [ProtoMember(2)]
        public List<TypeEnvelop<long>> LongValues { get; set; }

    }
}