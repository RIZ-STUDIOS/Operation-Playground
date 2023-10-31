using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        [System.NonSerialized]
        public new Camera camera;
        private Vector3 cForwardNorm;
        private Vector3 cRightNorm;

        private Vector3 storedPosition;
        private Quaternion storedRotation;

        private void Awake()
        {
            camera = GetComponent<Camera>();
            UpdateNormalizedCameraVectors();
        }

        private void Update()
        {
            var position = transform.position;
            var rotation = transform.rotation;
            if (position != storedPosition || rotation != storedRotation)
                UpdateNormalizedCameraVectors();
        }

        /// <summary>
        /// Should the camera move, continue to update the normalized vectors until stationary.
        /// </summary>
        private void UpdateNormalizedCameraVectors()
        {
            Vector3 cForward = transform.forward;
            Vector3 cRight = transform.right;
            cForward.y = 0;
            cRight.y = 0;
            cForwardNorm = cForward.normalized;
            cRightNorm = cRight.normalized;

            storedPosition = transform.position;
            storedRotation = transform.rotation;
        }

        public Vector3 WorldToCameraVector(Vector3 vector)
        {
            Vector3 forwardRelativeDir = vector.y * cForwardNorm;
            Vector3 rightRelativeDir = vector.x * cRightNorm;

            Vector3 relativeMoveDir = forwardRelativeDir + rightRelativeDir;
            return relativeMoveDir;
        }

        private void OnEnable()
        {
            camera.enabled = true;
        }

        private void OnDisable()
        {
            camera.enabled = false;
        }
    }
}
