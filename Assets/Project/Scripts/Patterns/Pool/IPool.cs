using System.Collections.Generic;

namespace Project.Scripts.Patterns.Pool
{
    public interface IPool<T> where T : class
    {
        
        delegate void ObjectPutHandler(T putObject);

        event ObjectPutHandler OnPutEvent;
        
        void PreWarm(int preWarmCount);
        T Get();
        IEnumerable<T> GetMultiple(int count = 1);

        void Put(T value, bool invokeEvent = false);
        void Put(IEnumerable<T> value);
    }
}