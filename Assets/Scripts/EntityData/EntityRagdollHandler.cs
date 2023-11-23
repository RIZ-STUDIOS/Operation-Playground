using OperationPlayground.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityRagdollHandler : MonoBehaviour
{
    private Rigidbody[] rigidbodies;
    private EnemyHealth enemyHealth;
    
    private void Awake()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        DisableRagdoll();

        enemyHealth = GetComponentInParent<EnemyHealth>();
        enemyHealth.onDeath += EnableRagdoll;
    }

    public void DisableRagdoll()
    {
        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = true;
        }
    }

    [ContextMenu("TriggerRagdoll")]
    public void EnableRagdoll()
    {
        transform.parent = null;

        GetComponent<Animator>().enabled = false;

        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = false;
        }

        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(3);

        Destroy(gameObject);
    }
}
