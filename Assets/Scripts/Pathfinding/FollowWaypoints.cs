using Codice.CM.Common;
using OperationPlayground.ScriptableObjects;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Pathfinding
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Seeker))]
    public class FollowWaypoints : MonoBehaviour
    {
        public event System.Action onEndPathReached;

        [SerializeField]
        private float nextWaypointDistance = 3;

        private PathWaypoint currentPathWaypoint;
        private PathWaypointList waypointList;

        private Seeker seeker;

        private CharacterController characterController;

        private Path path;

        private float speed;

        private int currentWaypoint;

        private void Awake()
        {
            seeker = GetComponent<Seeker>();

            characterController = GetComponent<CharacterController>();
        }

        public void SetWaypointList(PathWaypointList waypointList)
        {
            this.waypointList = waypointList;
            if (waypointList != null)
                currentPathWaypoint = waypointList.GetFirstWaypoint();
            else
                currentPathWaypoint = null;
            RecalculatePath();
        }

        public void NextWaypoint()
        {
            var previousWaypoint = currentPathWaypoint;
            currentPathWaypoint = waypointList.GetNextWaypoint(currentPathWaypoint);
            if(currentPathWaypoint == previousWaypoint)
            {
                onEndPathReached?.Invoke();
            }
            RecalculatePath();
        }

        public void RecalculatePath()
        {
            seeker.CancelCurrentPathRequest();
            if (currentPathWaypoint == null)
                path = null;
            seeker.StartPath(transform.position, currentPathWaypoint.GetRandomPoint(), OnPathComplete);
        }

        public void SetSpeed(float speed)
        {
            this.speed = speed;
        }

        private void OnPathComplete(Path path)
        {
            if (!path.error)
            {
                this.path = path;
                currentWaypoint = 0;
            }
        }

        private void Update()
        {
            if (path == null)
                return;

            var reachedEndOfPath = false;

            float distanceToWaypoint;

            while (true)
            {
                var distanceDifference = transform.position - path.vectorPath[currentWaypoint];
                distanceToWaypoint = (distanceDifference.x * distanceDifference.x) + (distanceDifference.y * distanceDifference.y) + (distanceDifference.z * distanceDifference.z);
                if (distanceToWaypoint < nextWaypointDistance * nextWaypointDistance)
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

            var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1;

            Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
            var velocity = dir * speed * speedFactor;

            characterController.SimpleMove(velocity);

            if (reachedEndOfPath)
            {
                NextWaypoint();
            }
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