using OperationPlayground.Projectiles;
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
                shootCooldown = weaponSo.cooldown;

                var projectileObject = Projectile.CreateProjectile(weaponSo.projectileScriptableObject, shooter);

                projectileObject.transform.position = FirePointTransform.position;
                projectileObject.transform.LookAt(GetFirePointForwardVector());
                projectileObject.transform.forward = GetFirePointForwardVector();

                if (!infiniteAmmo)
                {
                    currentAmmo--;
                }
                onAmmoChange?.Invoke();

                if (i == 0) gunshotSound.Play();
            }

            // Compensate for ammo loss.
            AddAmmo(numberOfShots - 1);
        }
    }
}
