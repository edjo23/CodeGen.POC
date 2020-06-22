using Beef.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Demo.Service.Business.Data
{
    public class JsonDataStorage : IDataStorage
    {
        private static ReaderWriterLockSlim Lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

        public JsonDataStorage()
        {
        }

        public string BasePath { get; set; } = ".";

        public IEnumerable<T> Read<T>()
            where T : class, IGuidIdentifier, new()
        {
            Lock.EnterWriteLock();

            try
            {
                return JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(GetFilePath<T>())) ?? new List<T>();
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        public T Read<T>(Guid key)
            where T : class, IGuidIdentifier, new()
        {
            return Read<T>().ToList().FirstOrDefault(o => o.Id == key);
        }

        public void Write<T>(T obj)
            where T : class, IGuidIdentifier, new()
        {
            WriteObject<T>(obj);
        }

        public void Delete<T>(T obj)
            where T : class, IGuidIdentifier, new()
        {
            DeleteObject<T>(obj);
        }

        private void WriteObject<T>(T obj)
            where T : class, IGuidIdentifier, new()
        {
            if (obj == null)
                return;

            Lock.EnterWriteLock();

            try
            {
                var objects = Read<T>().ToList();
                var before = objects.FirstOrDefault(o => o.Id == obj.Id);

                if (before != null)
                {
                    objects.Insert(objects.IndexOf(before), obj);
                    objects.Remove(before);
                }
                else
                {
                    objects.Add(obj);
                }

                File.WriteAllText(GetFilePath<T>(), JsonConvert.SerializeObject(objects, Formatting.Indented));
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        private void DeleteObject<T>(T obj)
            where T : class, IGuidIdentifier, new()
        {
            Lock.EnterWriteLock();

            try
            {
                var objects = Read<T>().ToList();
                var before = objects.FirstOrDefault(o => o.Id == obj.Id);

                if (before != null)
                {
                    objects.Remove(before);

                    File.WriteAllText(GetFilePath<T>(), JsonConvert.SerializeObject(objects, Formatting.Indented));
                }
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        private string GetFilePath<T>()
        {
            var path = Path.Combine(BasePath, String.Format("{0}.json", typeof(T).Name));

            if (!Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }

            if (!File.Exists(path))
                File.WriteAllText(path, "");

            return path;
        }
    }
}
