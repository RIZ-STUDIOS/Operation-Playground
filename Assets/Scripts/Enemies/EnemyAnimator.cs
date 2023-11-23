using OperationPlayground.Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground
{
    public class EnemyAnimator : MonoBehaviour
    {
        private Animator animator;
        private CharacterController enemyController;
        private FollowWaypoints followWaypoints;

        private int velocityXHash;
        private int velocityYHash;

        private void Awake()
        {
            enemyController = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
            followWaypoints = GetComponent<FollowWaypoints>();
            velocityXHash = Animator.StringToHash("VelocityX");
            velocityYHash = Animator.StringToHash("VelocityY");
        }

        private void Update()
        {
            Vector3 enemyVelocity = transform.rotation * followWaypoints.GetMovementDirection();

            animator.SetFloat(velocityXHash, enemyVelocity.x);
            animator.SetFloat(velocityYHash, enemyVelocity.z);
        }
    }
}
