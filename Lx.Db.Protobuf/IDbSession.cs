using System;
using System.Collections.Generic;

namespace Lx.Db.Protobuf
{
    public interface IDbSession : IDisposable
    {
        void Save<T>(string key, T data);
        T Get<T>(string key);
        void Remove(string key);
        void Save<T>(long key, T data);
        T Get<T>(long key);
        void Remove(long key);
        void Commit();
        void Rollback();
        IEnumerable<T> GetAll<T>();
    }
}