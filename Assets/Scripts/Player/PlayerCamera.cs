using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        public Camera Camera => this.GetIfNull(ref _camera);
        private Camera _camera;

        private Vector3 cForwardNorm;
        private Vector3 cRightNorm;

        private Vector3 storedPosition;
        private Quaternion storedRotation;

        public CinemachineBrain CameraBrain => this.GetIfNull(ref _brain);

        private CinemachineBrain _brain;

        private void Awake()
        {
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
            return new Vector3(vector.x, 0, vector.y);
        }

        private void OnEnable()
        {
            CameraBrain.enabled = true;
            Camera.enabled = true;
        }

        private void OnDisable()
        {
            CameraBrain.enabled = false;
            Camera.enabled = false;
        }
    }
}
