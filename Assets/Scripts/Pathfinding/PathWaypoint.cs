using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Pathfinding
{
    public class PathWaypoint : MonoBehaviour
    {
        [SerializeField]
        private Vector2 areaSize = new Vector2(1, 1);

        public Vector3 GetRandomPoint()
        {
            var half = areaSize / 2f;
            var min = -half;
            var max = half;

            var randomLocalPosition = new Vector3(Random.Range(min.x, max.x), 0, Random.Range(min.y, max.y));

            return transform.position + randomLocalPosition;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            Gizmos.DrawWireCube(transform.position, new Vector3(areaSize.x, 2, areaSize.y));
        }
    }
}
