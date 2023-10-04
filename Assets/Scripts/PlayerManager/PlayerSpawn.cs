using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace OperationPlayground.PlayerManager
{
    public class PlayerSpawn : MonoBehaviour
    {
        [SerializeField]
        private Transform[] spawnLocations;

        void OnPlayerJoined(PlayerInput input)
        {
            Debug.Log($"Player {input.playerIndex} has joined the session!");
            
        }
    }
}
