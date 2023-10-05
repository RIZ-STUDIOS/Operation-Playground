using OperationPlayground.Projectiles;
using OperationPlayground.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerShooting : MonoBehaviour
    {
        [SerializeField]
        private GameObject bulletPrefab;

        [SerializeField]
        private GameObject firingPoint;

        [SerializeField]
        private ProjectileScriptableObject projectileSo;

        void OnFire(InputValue input)
        {
            var gameObject = Instantiate(projectileSo.prefab);
            var projectile = gameObject.AddComponent<Projectile>();
            projectile.projectileSo = projectileSo;

            gameObject.transform.position = firingPoint.transform.position;
            gameObject.transform.forward = firingPoint.transform.forward;

            /*Bullet newBullet = Instantiate(bulletPrefab).GetComponent<Bullet>();
            newBullet.gameObject.transform.position = firingPoint.transform.position;
            newBullet.gameObject.transform.rotation = firingPoint.transform.rotation;
            newBullet.Fire(firingPoint.transform.forward.normalized);*/
        }
    }
}
