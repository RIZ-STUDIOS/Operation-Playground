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
        private void OnSceneGUI()
        {
            var crateManager = target as SupplyCrateManager;

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
