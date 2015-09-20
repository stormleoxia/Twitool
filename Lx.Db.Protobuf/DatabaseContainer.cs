using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using ProtoBuf;

namespace Lx.Db.Protobuf
{
    internal class DatabaseContainer<TKey>
    {
        private readonly Dictionary<TKey, TypeEnvelop<TKey>> _envelops = new Dictionary<TKey, TypeEnvelop<TKey>>();

        public DatabaseContainer()
        {
        }

        public DatabaseContainer(List<TypeEnvelop<TKey>> values)
        {
            foreach (var value in values)
            {
                _envelops[value.Key] = value;
            }
        }

        public List<TypeEnvelop<TKey>> Values
        {
            get { return _envelops.Values.ToList(); }
        }

        public void Add<TInstance>(TKey key, TInstance data)
        {
            MemoryStream ms = new MemoryStream();
            Serializer.Serialize(ms, data);
            var typeEnvelop = new TypeEnvelop<TKey>(key, typeof (TInstance), ms.ToArray()) {Instance = data};
            _envelops[key] = typeEnvelop;
        }

        public void Remove(TKey key)
        {
            _envelops.Remove(key);
        }

        public TInstance Get<TInstance>(TKey key)
        {
            TypeEnvelop<TKey> envelop;
            if (!_envelops.TryGetValue(key, out envelop))
            {
                return default(TInstance);
            }
            var typeSignature = envelop.TypeSignature;
            if (TypeEnvelop<TKey>.GetSignature(typeof (TInstance)) != typeSignature)
            {
                throw new SerializationException("Stored data for key " + key + " is with a different type signature " +
                                                 typeSignature);
            }
            return OpenEnvelop<TInstance>(envelop);
        }

        private static TInstance OpenEnvelop<TInstance>(TypeEnvelop<TKey> envelop)
        {
            if (envelop.Instance == null)
            {
                var ms = new MemoryStream(envelop.SerializedData);
                var deserialized = Serializer.Deserialize<TInstance>(ms);
                envelop.Instance = deserialized;
                return deserialized;
            }
            return (TInstance) envelop.Instance; // Note there could be unboxing but deserialization is worse.
        }

        public IEnumerable<T> GetAll<T>()
        {
            var signature = TypeEnvelop<TKey>.GetSignature(typeof (T));
            return _envelops.Values.Where(x => x.TypeSignature == signature).Select(OpenEnvelop<T>).ToArray();
        }
    }
}