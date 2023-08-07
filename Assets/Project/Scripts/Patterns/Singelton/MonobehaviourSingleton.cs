using System;
using UnityEngine;

namespace Project.Scripts.Patterns.Singelton
{
    public class MonobehaviourSingleton<T> : MonoBehaviour where T :  MonobehaviourSingleton<T>
    {
        private static T _instance;
        public static T Instance
        {
            get => _instance;
        }

        protected virtual void Awake()
        {
            if (_instance != null && _instance == this)
            {
                Destroy(this);
                throw new SystemException("An instance of this singleton already exists");
            }
            else
            {
                _instance = (T) this;
            }
        }
    }
}