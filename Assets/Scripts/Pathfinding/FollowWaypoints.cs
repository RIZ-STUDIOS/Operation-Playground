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

        [SerializeField]
        private bool faceWaypoint = true;

        private PathWaypoint currentPathWaypoint;
        private PathWaypointList waypointList;

        private Seeker seeker;

        private CharacterController characterController;

        private Path path;

        private float speed;

        private int currentWaypoint;

        private Vector3 point;

        private Vector3 previousWaypointPosition;

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
            CalculateNewPath();
        }

        public void NextWaypoint()
        {
            var previousWaypoint = currentPathWaypoint;
            currentPathWaypoint = waypointList.GetNextWaypoint(currentPathWaypoint);
            if (currentPathWaypoint == previousWaypoint)
            {
                onEndPathReached?.Invoke();
            }
            CalculateNewPath();
        }

        public void CalculateNewPath()
        {
            point = currentPathWaypoint.GetRandomPoint();
            CalculatePath();
        }

        public void CalculatePath()
        {
            seeker.CancelCurrentPathRequest();
            if (currentPathWaypoint == null)
                path = null;
            seeker.StartPath(transform.position, point, OnPathComplete);
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
                distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
                if (distanceToWaypoint < nextWaypointDistance)
                {
                    if (currentWaypoint + 1 < path.vectorPath.Count)
                    {
                        currentWaypoint++;
                        previousWaypointPosition = path.vectorPath[currentWaypoint];
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

            Vector3 dir = GetMovementDirection();
            var velocity = dir * speed * speedFactor;

            characterController.SimpleMove(velocity);
            if (faceWaypoint)
            {
                transform.LookAt(path.vectorPath[currentWaypoint], Vector3.up);
            }

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

        public Vector3 GetCurrentWaypointPosition()
        {
            if (path == null) return previousWaypointPosition;
            return path.vectorPath[currentWaypoint];
        }

        public Vector3 GetMovementDirection()
        {
            return (GetCurrentWaypointPosition() - transform.position).normalized;
        }
    }
}
