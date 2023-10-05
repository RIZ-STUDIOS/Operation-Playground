using OperationPlayground.Managers;
using OperationPlayground.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground
{
    public abstract class Building : MonoBehaviour
    {
        [System.NonSerialized]
        public BuildingScriptableObject buildingSo;

        [SerializeField]
        private Component[] scriptsToEnable;

        public void StartPlacement()
        {
            foreach (var script in scriptsToEnable)
            {
                var property = script.GetType().GetProperty("enabled");
                if (property != null)
                {
                    property.SetValue(script, false);
                }
            }
        }

        public void Place()
        {
            foreach(var script in scriptsToEnable)
            {
                var property = script.GetType().GetProperty("enabled");
                if (property != null)
                {
                    property.SetValue(script, true);
                }
            }
            OnPlaced();
        }

        protected abstract void OnPlaced();
    }
}
