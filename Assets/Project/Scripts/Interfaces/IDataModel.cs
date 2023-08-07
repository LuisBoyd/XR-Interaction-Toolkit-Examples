using System;

namespace Project.Scripts.Interfaces
{
    public interface IDataModel<T>
    {
        public String ToJson(T model);
        public T FromJson(String json);
    }
}