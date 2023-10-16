using OperationPlayground.SupplyCreates;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OperationPlayground.Editor.CustomEditors
{
    [CustomEditor(typeof(SupplyCreateSpawnLocation))]
    public class SupplyCrateSpawnLocationCustomEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            var supplyLocation = target as SupplyCreateSpawnLocation;

            var color = new Color(0.2f, 1f, 0.2f);

            var transform = supplyLocation.transform;
            var position = transform.position;

            var heightPosition = position + new Vector3(0, supplyLocation.spawnHeight, 0);

            Handles.color = color;
            Handles.DrawWireDisc(position, transform.up, supplyLocation.range, 3);

            Handles.DrawLine(position, heightPosition, 3);

            Handles.DrawWireDisc(heightPosition, transform.up, supplyLocation.range, 3);
        }
    }
}
