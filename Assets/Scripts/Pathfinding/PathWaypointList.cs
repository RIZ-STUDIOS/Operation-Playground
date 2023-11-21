using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Pathfinding
{
    public class PathWaypointList : MonoBehaviour
    {
        private List<PathWaypoint> waypoints;

        private void Awake()
        {
            waypoints = new List<PathWaypoint>(GetComponentsInChildren<PathWaypoint>());
        }

        public PathWaypoint GetFirstWaypoint()
        {
            return waypoints[0];
        }

        public PathWaypoint GetLastWaypoint()
        {
            return waypoints[(waypoints.Count - 1)];
        }

        public PathWaypoint GetNextWaypoint(PathWaypoint waypoint)
        {
            if (waypoint == null) return GetFirstWaypoint();

            var index = waypoints.IndexOf(waypoint);

            if (index < 0) return GetFirstWaypoint();

            index++;

            if (index >= waypoints.Count) return GetLastWaypoint();

            return waypoints[index];
        }
    }
}
