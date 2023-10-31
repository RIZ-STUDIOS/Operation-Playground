using OperationPlayground.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OperationPlayground.Editor.CustomEditors
{
    [CustomEditor(typeof(PlayerSpawnManager))]
    public class PlayerSpawnManagerCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var playerSpawnManager = (target as PlayerSpawnManager);

            GUI.enabled = Application.isPlaying && playerSpawnManager.AnyPlayersJoined;
            if(GUILayout.Button("Start Game"))
            {
                playerSpawnManager.StartGame();
            }
            GUI.enabled = true;
        }
    }
}
