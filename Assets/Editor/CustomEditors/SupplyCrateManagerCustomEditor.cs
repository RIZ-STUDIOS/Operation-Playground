using OperationPlayground.SupplyCreates;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OperationPlayground.Editor.CustomEditors
{
    [CustomEditor(typeof(SupplyCrateManager))]
    public class SupplyCrateManagerCustomEditor : UnityEditor.Editor
    {
        private SupplyCrateManager crateManager;

        private void OnEnable()
        {
            crateManager = (SupplyCrateManager)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Get All Dropdown Locations"))
            {
                crateManager.locations = new SupplyCreateSpawnLocation[] { };

                crateManager.locations = GameObject.FindObjectsByType<SupplyCreateSpawnLocation>(FindObjectsSortMode.None);

                serializedObject.ApplyModifiedProperties();
            }
        }

        private void OnSceneGUI()
        {
            var color = new Color(0.2f, 1f, 0.2f);

            Handles.color = color;

            foreach (var location in crateManager.locations)
            {
                if (location == null) continue;

                var transform = location.transform;
                var position = transform.position;

                var heightPosition = position + new Vector3(0, location.spawnHeight, 0);

                Handles.color = color;
                Handles.DrawWireDisc(position, transform.up, location.range, 3);

                Handles.DrawLine(position, heightPosition, 3);

                Handles.DrawWireDisc(heightPosition, transform.up, location.range, 3);
            }
        }
    }
}
