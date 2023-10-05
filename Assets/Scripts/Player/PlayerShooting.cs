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

        void OnFire(InputValue input)
        {
            Bullet newBullet = Instantiate(bulletPrefab).GetComponent<Bullet>();
            newBullet.gameObject.transform.position = firingPoint.transform.position;
            newBullet.gameObject.transform.rotation = firingPoint.transform.rotation;
            newBullet.Fire(firingPoint.transform.forward.normalized);
        }
    }
}
