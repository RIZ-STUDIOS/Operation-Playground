using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.UI
{
    public class RadialBarUI : MonoBehaviour
    {
        [SerializeField, Range(0, 1)]
        private float percentFilled = 1;

        public float PercentFilled { get { return percentFilled; } set { percentFilled = value; UpdatePercentFilled(); } }

        private new Renderer renderer;

        private void Awake()
        {
            renderer = GetComponent<Renderer>();
            PercentFilled = percentFilled;
        }

        private void UpdatePercentFilled()
        {
            if (!renderer) return;
            renderer.material.SetFloat("_FillAmount", percentFilled);
        }

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                UpdatePercentFilled();
            }
        }
    }
}
