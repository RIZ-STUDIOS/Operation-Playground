using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using static UnityEngine.GraphicsBuffer;

namespace RicTools
{
    public class Billboard : MonoBehaviour
    {
        [SerializeField, FormerlySerializedAs("lookAtTarget")]
        private Transform _lookAtTarget;

        [SerializeField, FormerlySerializedAs("inverseLookAt")]
        private bool _inverseLookAt;


        public Transform LookAtTarget { get { return _lookAtTarget; } set { _lookAtTarget = value; UpdateLook(); } }
        public bool InverseLookAt { get { return _inverseLookAt; } set { _inverseLookAt = value; UpdateLook(); } }

        private static Camera activeCamera;

        private void OnEnable()
        {
            RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
        }

        private void OnDisable()
        {
            RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
        }

        void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
        {
            // Put the code that you want to execute before the camera renders here
            // If you are using URP or HDRP, Unity calls this method automatically
            // If you are writing a custom SRP, you must call RenderPipeline.BeginCameraRendering

            var target = camera.transform;

            var position = target.position;

            if (InverseLookAt)
            {
                position = transform.position * 2 - target.position;
            }

            transform.LookAt(position);
        }

        /*private void Awake()
        {
            UpdateLook();
        }

        private void Update()
        {
            UpdateLook();
        }*/

        private void UpdateLook()
        {
            Transform target = LookAtTarget;

            if (target == null && activeCamera == null) activeCamera = FindObjectOfType<Camera>();

            if (target == null && activeCamera != null) target = activeCamera.transform;

            if (target == null) return;

            var position = target.position;

            if (InverseLookAt)
            {
                position = transform.position * 2 - target.position;
            }

            transform.LookAt(position);
        }
    }
}
