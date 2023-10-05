using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Player
{
    public class PlayerSpawnManager : MonoBehaviour
    {
        [SerializeField]
        private Transform[] spawnLocations;

        void OnPlayerJoined(PlayerInput input)
        {
            Debug.Log($"Player {input.playerIndex} has joined the session!");
            
            input.gameObject.transform.position = spawnLocations[Random.Range(0, spawnLocations.Length)].position;
        }
    }
}