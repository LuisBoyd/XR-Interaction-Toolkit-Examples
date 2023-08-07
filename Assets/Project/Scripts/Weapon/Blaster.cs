using System.Collections.Generic;
using Project.Scripts.Interfaces;
using UnityEngine;

namespace Project.Scripts.Weapon
{
    public class Blaster : GunWeapon
    {
        protected override void Awake()
        {
            base.Awake();
        }

        public override void FireProjectile()
        {
            muzzleFlash.Play();
            fireSfx.Play();
            
            //Build Ray
            Ray ray = new Ray(firePoint.position, firePoint.forward);
            if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit))
            {
                GameObject collidedObj = hit.collider.gameObject;
                if (collidedObj.TryGetComponent<IRaycastReceiver>(out IRaycastReceiver receiver))
                {
                    receiver.ReceiveRay(gameObject, ray, hit);
                }
            }
        }
    }
}