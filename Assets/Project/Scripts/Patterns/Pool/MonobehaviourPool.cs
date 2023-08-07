using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.Scripts.Patterns.Pool
{
    public abstract class MonobehaviourPool<T> : MonoBehaviour, IPool<T> where T : MonoBehaviour
    {

        [SerializeField] 
        protected bool IsRoot;
        [SerializeField]
        protected Transform RootLocation;
        [SerializeField]
        protected IFactory<T> _factory;
        protected List<T> _pool;
        protected bool _prewarmed;
        private bool failedInit = false;
        [SerializeField] 
        protected bool preWarmOnStart;
        [SerializeField] 
        protected int startingCount;

        protected void Awake()
        {
            _factory = GetComponent<IFactory<T>>();
            if (_factory == null)
            {
                Debug.LogWarning("Factory is Empty");
                failedInit = true;
            }
            _pool = new List<T>();
            _prewarmed = false;
            if (IsRoot || RootLocation == null)
            {
                RootLocation = this.transform;
            }
            if (preWarmOnStart && !failedInit)
            {
                PreWarm(startingCount);
            }
        }

        protected void Start()
        {
            if (failedInit)
            {
                this.gameObject.SetActive(false);
            }
        }

        public event IPool<T>.ObjectPutHandler OnPutEvent;

        public virtual void PreWarm(int preWarmCount)
        {
            if(_prewarmed)
                return;
            _prewarmed = true;
            Create(preWarmCount);
        }

        public virtual T Get()
        {
            if (_prewarmed)
            {
                if (_pool.Count < 1)
                    Create();
                T item = _pool[0];
                _pool.RemoveAt(0);
                item.gameObject.SetActive(true);
                item.gameObject.transform.SetParent(null);
                return item;
            }

            return null;
        }

        public virtual IEnumerable<T> GetMultiple(int count = 1)
        {
            if (_prewarmed)
            {
                if (_pool.Count < count)
                {
                    int difference = count - _pool.Count;
                    for (int i = 0; i < difference; i++)
                        Create();
                }
                List<T> items = _pool.GetRange(0, count);
                // if (items.Any(x => x == null))
                // {
                //     Create(count);
                //     items = _pool.GetRange(0, count);
                // }
                _pool.RemoveRange(0,count);
                _pool.ForEach(x =>
                {
                    x.gameObject.SetActive(true);
                    x.gameObject.transform.SetParent(null);
                });
                return items;
            }

            return null;
        }

        public virtual void Put(T value, bool invokeEvent = false)
        {
            if (_prewarmed)
            {
                if (RootLocation != null)
                {
                    value.gameObject.transform.SetParent(RootLocation);
                    value.gameObject.transform.localPosition = Vector3.zero;
                }
                value.gameObject.SetActive(false);
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
                    if (RootLocation != null)
                    {
                        x1.gameObject.transform.SetParent(RootLocation);
                        x1.gameObject.transform.localPosition = Vector3.zero;
                    }
                    x1.gameObject.SetActive(false);
                    _pool.Add(x1);
                }
            }
        }
        
        private void Create(int createCount = 1)
        {
            for (int i = 0; i < createCount; i++)
            {
                Put(_factory.Create());
            }
        }
    }
}