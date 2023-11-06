using OperationPlayground.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Editor.CustomEditors
{
    [CustomEditor(typeof(PlayerSpawnManager))]
    public class PlayerSpawnManagerCustomEditor : UnityEditor.Editor
    {
        private PlayerInputManager playerInputManager;

        private void OnEnable()
        {
            playerInputManager = FindObjectOfType<PlayerInputManager>();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var playerSpawnManager = (target as PlayerSpawnManager);

            GUI.enabled = Application.isPlaying && playerSpawnManager.AnyPlayersJoined;
            if (GUILayout.Button("Start Game"))
            {
                playerSpawnManager.StartGame();
            }

            GUI.enabled = Application.isPlaying;

            EditorGUI.BeginChangeCheck();

            var value = GUILayout.Toggle(playerInputManager.joiningEnabled, "Allow players to join");

            if (EditorGUI.EndChangeCheck())
            {
                if (value)
                {
                    playerInputManager.EnableJoining();
                }
                else
                {
                    playerInputManager.DisableJoining();
                }
            }

            GUI.enabled = true;
        }
    }
}
