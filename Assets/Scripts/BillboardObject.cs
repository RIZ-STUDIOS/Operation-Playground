using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace OperationPlayground
{
    public class BillboardObject : MonoBehaviour
    {
        [SerializeField]
        private bool inverseLookAt = true;

        private void OnEnable()
        {
            RenderPipelineManager.beginCameraRendering += RenderPipelineManager_beginCameraRendering;
        }

        private void RenderPipelineManager_beginCameraRendering(ScriptableRenderContext renderContext, Camera currentCamera)
        {
            var target = currentCamera.transform;
            var position = target.position;

            if (inverseLookAt)
            {
                position = transform.position * 2 - target.position;
            }

            transform.LookAt(position);
        }

        private void OnDisable()
        {
            RenderPipelineManager.beginCameraRendering -= RenderPipelineManager_beginCameraRendering;
        }
    }
}
