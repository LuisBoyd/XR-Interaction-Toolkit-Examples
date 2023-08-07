using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

namespace Project.Scripts.Weapon
{
    public abstract class GunWeapon: RangedWeapon
    {
        [FormerlySerializedAs("_MuzzleFlash")] [SerializeField] protected VisualEffect muzzleFlash;
        [SerializeField] protected AudioSource fireSfx;
        protected virtual void Awake()
        {
            fireSfx = GetComponent<AudioSource>();
        }
    }
}