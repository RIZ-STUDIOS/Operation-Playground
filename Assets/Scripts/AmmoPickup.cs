using OperationPlayground.Player;
using OperationPlayground.Resources;
using UnityEngine;

namespace OperationPlayground
{
    public class AmmoPickup : MonoBehaviour
    {
        [SerializeField]
        private float rotateSpeed;

        [SerializeField]
        private float boppingSpeed = 1;

        [SerializeField]
        private float boppingModifier = 1;

        private Vector3 spawnPosition;

        private void Start()
        {
            spawnPosition = transform.position;
        }

        private void Update()
        {
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
            transform.position = spawnPosition + new Vector3(0, Mathf.Sin(Time.time * boppingSpeed) * boppingModifier, 0);
        }

        private void OnTriggerEnter(Collider other)
        {
            var playerManager = other.GetComponentInParent<PlayerManager>();
            if (!playerManager) return;

            var weapon = playerManager.Shooter.CurrentWeapon;
            if (weapon.InfiniteAmmo)
            {
                int checks = 0;
                do
                {
                    weapon = playerManager.PlayerShooter.FindWeaponWithAmmo();
                    if (!weapon) break;

                    checks++;
                } while (weapon.CurrentAmmo == weapon.weaponSo.maxAmmo || checks >= playerManager.PlayerShooter.HeldWeapons.Count);
            }

            if (weapon) weapon.AddAmmo((int)Mathf.Ceil(weapon.weaponSo.maxAmmo / 10f));

            ResourceManager.Instance.Supplies += 5;

            Destroy(gameObject);
        }
    }
}
