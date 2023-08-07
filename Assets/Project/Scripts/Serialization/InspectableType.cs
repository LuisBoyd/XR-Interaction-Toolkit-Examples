namespace Project.Scripts.Serialization
{
    using UnityEngine;
    
    [System.Serializable]
    public class InspectableType<T> : ISerializationCallbackReceiver
    {
        [SerializeField] string qualifiedName;
        public System.Type storedType { get; private set; }

#if UNITY_EDITOR
        //Hack: I wasn't able to find the base type from the SerializedProperty,
        // so I'm smuggling it in via an extra string stored only in-editor.
        [SerializeField] private string baseTypeName;
#endif

        public InspectableType(System.Type typeToStore)
        {
            storedType = typeToStore;
        }

        public override string ToString()
        {
            if (storedType == null) return string.Empty;
            return storedType.Name;
        }


        public void OnBeforeSerialize()
        {
            if (storedType != null)
            {
                qualifiedName = storedType.AssemblyQualifiedName;
#if UNITY_EDITOR
                baseTypeName = typeof(T).AssemblyQualifiedName;
#endif
            }
        }

        public void OnAfterDeserialize()
        {
            if (string.IsNullOrEmpty(qualifiedName) || qualifiedName == "null")
            {
                storedType = null;
                return;
            }
            storedType = System.Type.GetType(qualifiedName);
        }

        public static implicit operator System.Type(InspectableType<T> t) => t.storedType;
        
        // TODO: Validate that t is a subtype of T?
        public static implicit operator InspectableType<T>(System.Type t) => new InspectableType<T>(t);
    }
}