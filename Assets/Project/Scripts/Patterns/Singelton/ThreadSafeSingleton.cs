using System;

namespace Project.Scripts.Patterns.Singelton
{
    /// <summary>
    /// Lazy Instantiated Singleton, Thread safe
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ThreadSafeSingleton<T> where T : class
    {
        private static readonly Lazy<T> lazyInstance = new Lazy<T>(CreateInstanceOfT);
        
        //private constructor to prevent direct instantiation
        protected ThreadSafeSingleton() {}
        
        public static T Instance
        {
            get { return lazyInstance.Value; }
        }

        private static T CreateInstanceOfT() => Activator.CreateInstance(typeof(T), true) as T;
    }
}