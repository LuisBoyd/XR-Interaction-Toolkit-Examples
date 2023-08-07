using System;
using Project.Scripts.Patterns.Pool;
using UnityEngine;
using UnityEngine.VFX;

namespace Project.Scripts.VFXScripts
{
    using UnityEngine.VFX.Utility;
    [RequireComponent(typeof(VisualEffect), typeof(MonobehaviourPool<>))]
    public class VFXOutputEventPrefabSpawnPool<T> : VFXOutputEventAbstractHandler where T : MonoBehaviour
    {
        public delegate void TSpawn(T spawnedObject);
        public event TSpawn On_VFXprefabSpawned;
        
        public override bool canExecuteInEditor => false;
        public MonobehaviourPool<T> Pool => _pool;
        public uint instanceCount => m_InstanceCount;
        public bool parentInstance => m_parentInstances;

        public bool KeepInstancesAlive => keepInstanceAlive;
        
        [SerializeField, Tooltip("The Pool the Object/Objects spawn from")] 
        private MonobehaviourPool<T> _pool;
        [SerializeField] [Tooltip("The maximum amount of prefabs that can be active at a time")]
        private uint m_InstanceCount = 1;
        [SerializeField] [Tooltip("Whether to attach prefab instances to current game object. Use this setting to treat position and angle attributes as local space.")]
        private bool m_parentInstances;
        [SerializeField] [Tooltip("Whether to Keep the spawned objects alive Ignoring Lifetime (you will need to manually get rid of the objects)")]
        private bool keepInstanceAlive = false;
        
        [Tooltip("Whether to use the position attribute to set prefab position on spawn")]
        public bool usePosition = true;
        [Tooltip("Whether to use the angle attribute to set prefab rotation on spawn")]
        public bool useAngle = true;
        [Tooltip("Whether to use the scale attribute to set prefab localScale on spawn")]
        public bool useScale = true;
        [Tooltip("Whether to use the lifetime attribute to determine how long the prefab will be enabled")]
        public bool useLifetime = true;

        static readonly T[] k_EmptyGameObjects = new T[0];
        static readonly float[] k_EmptyTimeToLive = new float[0];
        T[] m_Instances = k_EmptyGameObjects;
        float[] m_TimesToLive = k_EmptyTimeToLive;

        private bool isAppQuitting = false;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (Application.isPlaying)
            {
                _pool.OnPutEvent += On_Object_Re_Enter_Pool;
                Application.quitting += () => isAppQuitting = true;
            }
        }

        private void On_Object_Re_Enter_Pool(T putobject)
        {
            int indexOfObject = Array.IndexOf(m_Instances, putobject);
            m_Instances[indexOfObject] = null;
            m_TimesToLive[indexOfObject] = 0.0f;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (Application.isPlaying && !isAppQuitting)
            {
                _pool.OnPutEvent -= On_Object_Re_Enter_Pool;
                foreach (var Instance in m_Instances)
                {
                    Instance.gameObject.SetActive(false);
                }
            }
        }

        private void OnDestroy()
        {
            //DisposeInstances();
        }

        private void DisposeInstances()
        {
            foreach (var instance in m_Instances)
            {
                if (instance)
                {
                    if(Application.isPlaying)
                        _pool.Put(instance);
                }
            }

            m_Instances = k_EmptyGameObjects;
            m_TimesToLive = k_EmptyTimeToLive;
        }
        static readonly int k_PositionID = Shader.PropertyToID("position");
        static readonly int k_AngleID = Shader.PropertyToID("angle");
        static readonly int k_ScaleID = Shader.PropertyToID("scale");
        static readonly int k_LifetimeID = Shader.PropertyToID("lifetime");
        
        void UpdateHideFlag(T instance)
        {
            instance.hideFlags = HideFlags.HideAndDontSave;
            //We are using HideInHierarchy to prevent unexpected deletion in edit mode.
            //instance.hideFlags = HideFlags.DontSave | HideFlags.NotEditable;
        }

        void CheckAndRebuildInstances()
        {
            bool rebuild = m_Instances.Length != m_InstanceCount;
            if (rebuild)
            {
                DisposeInstances();
                if (_pool != null && m_InstanceCount != 0)
                {
                    m_Instances = new T[m_InstanceCount];
                    m_TimesToLive = new float[m_InstanceCount];

                    for (int i = 0; i < m_Instances.Length; i++)
                    {
                        T newInstance = null;
                        newInstance = _pool.Get();
                        newInstance.name = $"{name} - #{i} - {typeof(T).Name}";
                        newInstance.gameObject.SetActive(false);
                        newInstance.transform.parent = m_parentInstances ? transform : null;
                        UpdateHideFlag(newInstance);

                        m_Instances[i] = newInstance;
                        m_TimesToLive[i] = float.NegativeInfinity;
                    }
                }
            }
        }

        public override void OnVFXOutputEvent(VFXEventAttribute eventAttribute)
        {
            CheckAndRebuildInstances();
            int freeIdx = -1;
            for (int i = 0; i < m_Instances.Length; i++)
            {
                if (!m_Instances[i].gameObject.activeSelf)
                {
                    freeIdx = i;
                    break;
                }
            }

            if (freeIdx != -1)
            {
                var availableInstance = m_Instances[freeIdx];
                availableInstance.gameObject.SetActive(true);
                if (usePosition && eventAttribute.HasVector3(k_PositionID))
                {
                    if (m_parentInstances)
                        availableInstance.transform.localPosition = eventAttribute.GetVector3(k_PositionID);
                    else
                        availableInstance.transform.position = eventAttribute.GetVector3(k_PositionID);
                }
                if (useAngle && eventAttribute.HasVector3(k_AngleID))
                {
                    if (parentInstance)
                        availableInstance.transform.localEulerAngles = eventAttribute.GetVector3(k_AngleID);
                    else
                        availableInstance.transform.eulerAngles = eventAttribute.GetVector3(k_AngleID);
                }

                if (useScale && eventAttribute.HasVector3(k_ScaleID))
                    availableInstance.transform.localScale = eventAttribute.GetVector3(k_ScaleID);

                if (useLifetime && eventAttribute.HasFloat(k_LifetimeID))
                    m_TimesToLive[freeIdx] = eventAttribute.GetFloat(k_LifetimeID);
                else
                    m_TimesToLive[freeIdx] = float.NegativeInfinity;

                var handlers = availableInstance.GetComponentsInChildren<VFXOutputEventPrefabAttributeAbstractHandler>();
                foreach (var handler in handlers)
                {
                    handler.OnVFXEventAttribute(eventAttribute, m_VisualEffect);
                }
                On_VFXprefabSpawned?.Invoke(availableInstance);
            }
        }

        private void Update()
        {
            if (Application.isPlaying)
            {
                CheckAndRebuildInstances();
                if (!keepInstanceAlive)
                {
                    var dt = Time.deltaTime;
                    for (int i = 0; i < m_Instances.Length; i++)
                    {
                        if (m_TimesToLive[i] <= 0.0f && m_Instances[i].gameObject.activeSelf)
                            _pool.Put(m_Instances[i]);
                        else
                            m_TimesToLive[i] -= dt;
                    }
                }
            }
            else
            {
                DisposeInstances();
            }
        }
    }
}