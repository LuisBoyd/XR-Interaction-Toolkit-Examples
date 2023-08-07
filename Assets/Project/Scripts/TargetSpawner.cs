using System;
using Project.Scripts.Patterns.Factory;
using Project.Scripts.Patterns.Pool;
using Project.Scripts.Patterns.State.Concrete.Game.Target;
using Unity.Mathematics;
using UnityEngine;

namespace Project.Scripts
{
    [RequireComponent(typeof(TargetPool), typeof(TargetFactory))]
    public class TargetSpawner : MonoBehaviour
    {
        private TargetPool _pool;
        [SerializeField] private int NumberOfTargets = 10;
        [SerializeField] private float radius = 5f;
        [SerializeField] private float yOffset = 0f;

        private void Awake()
        {
            _pool = GetComponent<TargetPool>();
        }

        private void Start()
        {
            Spawn();
        }

        public void Spawn()
        {
            float angleStep = 360f / NumberOfTargets;
            for (int i = 0; i < NumberOfTargets; i++)
            {
                float angle = i * angleStep;

                float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
                float z = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
                Vector3 spawnPosition = new Vector3(x, yOffset, z) + transform.position;

                Target item = _pool.Get();
                item.HelperTarget = this.transform;
                item.gameObject.transform.position = spawnPosition;
                item.gameObject.transform.rotation = quaternion.identity;
            }
        }
    }
}