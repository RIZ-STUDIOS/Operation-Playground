using OperationPlayground.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground
{
    public class AudioTracker : MonoBehaviour
    {
        private void Update()
        {
            if (PlayerSpawnManager.Instance.Players.Count == 0) return;

            Vector3 pointBetweenPlayers = Vector3.zero;

            foreach (var player in PlayerSpawnManager.Instance.Players)
            {
                pointBetweenPlayers += player.transform.position;
            }

            pointBetweenPlayers /= PlayerSpawnManager.Instance.Players.Count;

            transform.position = pointBetweenPlayers;
        }
    }
}
