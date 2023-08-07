using System.Collections.Generic;
using System.Linq;

namespace Project.Scripts.Patterns.Pool
{
    public abstract class Pool<T> : IPool<T> where T : class
    {
        protected IFactory<T> _factory;
        protected List<T> _pool;
        protected bool _prewarmed;

        protected Pool(IFactory<T> factory, bool triggerPreWarm = false, int prewarmCount = 1)
        {
            _factory = factory;
            _pool = new List<T>();
            _prewarmed = false;
            if(triggerPreWarm)
                PreWarm(prewarmCount);
        }

        public event IPool<T>.ObjectPutHandler OnPutEvent;

        public virtual void PreWarm(int prewarmCount)
        {
            if(_prewarmed)
                return;
            _prewarmed = true;
            Create(prewarmCount);
        }

        public virtual T Get()
        {
            if (_prewarmed)
            {
                T item = _pool[0];
                if (item == null)
                {
                    Create();
                    item = _pool[0];
                }
                _pool.RemoveAt(0);
                return item;
            }

            return null;
        }

        public virtual IEnumerable<T> GetMultiple(int count = 1)
        {
            if (_prewarmed)
            {
                List<T> items = _pool.GetRange(0, count);
                if (items.Any(x => x == null))
                {
                    Create(count);
                    items = _pool.GetRange(0, count);
                }
                _pool.RemoveRange(0,count);
                return items;
            }

            return null;
        }

        public virtual void Put(T value, bool invokeEvent = false)
        {
            if (_prewarmed)
            {
                _pool.Add(value);
                if(invokeEvent)
                    OnPutEvent?.Invoke(value);
            }
        }

        public virtual void Put(IEnumerable<T> value)
        {
            if (_prewarmed)
            {
                foreach (var x1 in value)
                {
                    _pool.Add(x1);
                }
            }
        }

        private void Create(int createCount = 1)
        {
            for (int i = 0; i < createCount; i++)
            {
                _pool.Add(_factory.Create());
            }
        }
    }
}