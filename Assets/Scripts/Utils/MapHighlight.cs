using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace OperationPlayground
{
    public class MapHighlight : MonoBehaviour
    {
        private List<Outline> outlines = new List<Outline>();

        public Color HighLightColor { get { return _highLightColor; } set { _highLightColor = value; UpdateOutlineColor(); } }

        [SerializeField]
        private Color _highLightColor = Color.green;

        public LayerMask visibleLayerMask;

        private void Awake()
        {
            var renderers = GetComponentsInChildren<Renderer>();

            foreach (var renderer in renderers)
            {
                var outline = renderer.gameObject.AddComponent<Outline>();
                if (!outline) continue;
                outline.enabled = false;
                outline.OutlineWidth = 7;
                outline.OutlineMode = Outline.Mode.OutlineVisible;
                outlines.Add(outline);
            }

            HighLightColor = _highLightColor;
        }

        private void UpdateOutlineColor()
        {
            EnableOutlines();
            foreach (var outline in outlines)
            {
                outline.OutlineColor = HighLightColor;
            }
            DisableOutlines();
        }

        private void OnEnable()
        {
            RenderPipelineManager.beginCameraRendering += RenderPipelineManager_beginCameraRendering;
            RenderPipelineManager.endCameraRendering += RenderPipelineManager_endCameraRendering;
        }

        private void OnDisable()
        {
            RenderPipelineManager.beginCameraRendering -= RenderPipelineManager_beginCameraRendering;
            RenderPipelineManager.endCameraRendering -= RenderPipelineManager_endCameraRendering;
            DisableOutlines();
        }

        private void RenderPipelineManager_beginCameraRendering(ScriptableRenderContext renderContext, Camera currentCamera)
        {
            if (visibleLayerMask.value != (visibleLayerMask.value | (1 << currentCamera.gameObject.layer))) return;
            if (currentCamera.transform.parent) return;

            EnableOutlines();
        }

        private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext renderContext, Camera currentCamera)
        {
            if (visibleLayerMask.value != (visibleLayerMask.value | (1 << currentCamera.gameObject.layer))) return;
            if (currentCamera.transform.parent) return;

            DisableOutlines();
        }

        private void EnableOutlines()
        {
            foreach (var outline in outlines)
            {
                outline.enabled = true;
                outline.UpdateMaterialProperties();
            }
        }

        private void DisableOutlines()
        {
            foreach (var outline in outlines)
            {
                outline.enabled = false;
            }
        }
    }
}
