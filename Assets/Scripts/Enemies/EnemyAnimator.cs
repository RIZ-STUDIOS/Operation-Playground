using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground
{
    public class EnemyAnimator : MonoBehaviour
    {
        private Animator animator;
        private CharacterController enemyController;

        private int velocityXHash;
        private int velocityYHash;

        private void Awake()
        {
            enemyController = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
            velocityXHash = Animator.StringToHash("VelocityX");
            velocityYHash = Animator.StringToHash("VelocityY");
        }

        private void Update()
        {
            Vector3 enemyVelocity = enemyController.velocity.normalized;

            animator.SetFloat(velocityXHash, enemyVelocity.x);
            animator.SetFloat(velocityYHash, enemyVelocity.z);
        }
    }
}
