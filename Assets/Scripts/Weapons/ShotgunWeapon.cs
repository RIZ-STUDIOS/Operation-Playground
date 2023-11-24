using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Weapons
{
    public class ShotgunWeapon : Weapon
    {
        [SerializeField]
        private int numberOfShots = 3;

        protected override void ShootGun()
        {
            for (int i = 0; i < numberOfShots; i++)
            {
                base.ShootGun();
            }
        }
    }
}
