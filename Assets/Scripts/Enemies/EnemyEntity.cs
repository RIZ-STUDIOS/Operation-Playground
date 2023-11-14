using OperationPlayground.EntityData;
using OperationPlayground.ScriptableObjects;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Enemies
{
    [RequireComponent(typeof(EnemyHealth))]
    [RequireComponent(typeof(EnemyShooter))]
    public class EnemyEntity : GenericEntity
    {
        public EnemyScriptableObject enemyScriptableObject;

        [SerializeField]
        private float nextWaypointDistance = 3;

        public EnemyHealth EnemyHealth => this.GetIfNull(ref _enemyHealth);
        public EnemyShooter EnemyShooter => this.GetIfNull(ref _enemyShooter);

        private EnemyHealth _enemyHealth;
        private EnemyShooter _enemyShooter;

        public override GenericHealth Health => EnemyHealth;

        public override GenericShooter Shooter => EnemyShooter;

        private Seeker seeker;

        private CharacterController characterController;

        private float Speed => enemyScriptableObject.speed;

        private int currentWaypoint;

        private bool reachedEndOfPath;

        public Path path;

        private Transform currentTarget;

        public static EnemyEntity SpawnEnemy(EnemyScriptableObject enemyScriptableObject, Vector3 spawnLocation = default, Transform targetDestination = null)
        {
            var enemyObject = Instantiate(enemyScriptableObject.prefab);

            var enemy = enemyObject.GetComponent<EnemyEntity>();

            if (!enemy)
                throw new System.Exception();

            enemy.enemyScriptableObject = enemyScriptableObject;

            enemy.Shooter.AddWeapon(enemyScriptableObject.weaponScriptableObject);

            enemy.SetPosition(spawnLocation);

            if (targetDestination)
                enemy.StartPathingTo(targetDestination);

            return null;
        }

        protected override void Awake()
        {
            base.Awake();

            seeker = GetComponent<Seeker>();
            characterController = GetComponent<CharacterController>();
        }

        public void StartPathingTo(Transform target)
        {
            currentTarget = target;
            RecalculatePath();
        }

        public void RecalculatePath()
        {
            if (!currentTarget) return;
            seeker.StartPath(transform.position, currentTarget.position, OnPathComplete);
        }

        private void OnPathComplete(Path path)
        {
            if(!path.error)
            {
                this.path = path;
                currentWaypoint = 0;
            }    
        }

        private void Update()
        {
            if (path == null)
                return;

            reachedEndOfPath = false;

            float distanceToWaypoint;

            while (true)
            {
                var distanceDifference = transform.position - path.vectorPath[currentWaypoint];
                distanceToWaypoint = (distanceDifference.x * distanceDifference.x) + (distanceDifference.y * distanceDifference.y) + (distanceDifference.z * distanceDifference.z);
                if(distanceToWaypoint < nextWaypointDistance * nextWaypointDistance)
                {
                    if (currentWaypoint + 1 < path.vectorPath.Count)
                    {
                        currentWaypoint++;
                    }
                    else
                    {
                        reachedEndOfPath = true;
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint/nextWaypointDistance) : 1;

            Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
            var velocity = dir * Speed * speedFactor;

            characterController.SimpleMove(velocity);
        }

        public void SetPosition(Vector3 position)
        {
            var charEnabled = characterController.enabled;
            characterController.enabled = false;
            transform.position = position;
            characterController.enabled = charEnabled;
        }
    }
}
