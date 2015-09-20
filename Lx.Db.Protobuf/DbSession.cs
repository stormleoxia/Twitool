using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProtoBuf;

namespace Lx.Db.Protobuf
{
    internal class DbSession : IDbSession
    {
        private readonly string _fileName;
        private DatabaseContainer<long> _intContainer = new DatabaseContainer<long>();
        private DatabaseContainer<string> _stringContainer = new DatabaseContainer<string>();

        public DbSession(string fileName)
        {
            _fileName = fileName;
            ReloadDb();
        }

        private bool ReloadDb()
        {
            if (File.Exists(_fileName))
            {
                using (var stream = File.Open(_fileName, FileMode.Open, FileAccess.Read))
                {
                    var container = Serializer.Deserialize<SerializableContainer>(stream);
                    _intContainer = new DatabaseContainer<long>(container.LongValues);
                    _stringContainer = new DatabaseContainer<string>(container.StringValues);
                }
                return true;
            }
            return false;
        }

        public void Save<TInstance>(long key, TInstance data)
        {
            _intContainer.Add(key, data);
        }

        public T Get<T>(long key)
        {
            return _intContainer.Get<T>(key);
        }

        public void Remove(long key)
        {
            _intContainer.Remove(key);
        }

        public void Commit()
        {
            using (var stream = File.Open(_fileName, FileMode.OpenOrCreate, FileAccess.Write))
            {
                var serializable = new SerializableContainer(_intContainer, _stringContainer);
                Serializer.Serialize(stream, serializable);
                stream.Flush();
                stream.Close();
            }
        }

        public void Rollback()
        {
            if (!ReloadDb())
            {
                _intContainer = new DatabaseContainer<long>();
                _stringContainer = new DatabaseContainer<string>();
            }
        }

        public IEnumerable<T> GetAll<T>()
        {
            List<T> list = new List<T>();
            list.AddRange(_intContainer.GetAll<T>());
            list.AddRange(_stringContainer.GetAll<T>());
            return list;
        }

        public void Save<TInstance>(string key, TInstance data)
        {
            _stringContainer.Add(key, data);
        }

        public T Get<T>(string key)
        {
            return _stringContainer.Get<T>(key);
        }

        public void Remove(string key)
        {
            _stringContainer.Remove(key);
        }

        public void Dispose()
        {
            Commit();
        }
    }
}