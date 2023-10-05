using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace OperationPlayground.Player
{
    public class Bullet : MonoBehaviour
    {
        private float travelDuration = 1;
        private float bulletSpeed = 20;

        public void Fire(Vector3 fireDir)
        {
            StartCoroutine(BulletTravel(fireDir));
        }

        private IEnumerator BulletTravel(Vector3 firingDirection)
        {
            float timer = 0;
            
            while (timer < travelDuration)
            {
                transform.position += firingDirection * Time.deltaTime * bulletSpeed;
                timer += Time.deltaTime;
                yield return null;
            }

            Destroy(gameObject);
        }
    }
}
