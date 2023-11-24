using OperationPlayground.EntityData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground
{
    public class DeathPlane : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var entity = other.GetComponentInParent<GenericEntity>();
            if(!entity) return;

            entity.Health.Kill();
        }
    }
}
