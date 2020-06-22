using Beef.Entities;
using System;
using System.Collections.Generic;

namespace Demo.Service.Business.Data
{
    public interface IDataStorage
    {
        IEnumerable<T> Read<T>()
            where T : class, IGuidIdentifier, new();

        T Read<T>(Guid objectId)
            where T : class, IGuidIdentifier, new();

        void Write<T>(T obj)
            where T : class, IGuidIdentifier, new();

        void Delete<T>(T obj)
            where T : class, IGuidIdentifier, new();
    }
}
