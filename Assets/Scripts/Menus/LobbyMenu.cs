using OperationPlayground.Managers;
using OperationPlayground.Player;
using RicTools.Attributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

namespace OperationPlayground
{
    public class LobbyMenu : MonoBehaviour
    {
        public event System.Action onLobbyFinished;

        [SerializeField]
        private TextMeshProUGUI timerText;

        [SerializeField]
        private TextMeshProUGUI[] playerPrompts;

        [SerializeField, MinValue(1)]
        private int minPlayers = 2;

        private PlayerSpawnManager playerSpawn;

        [SerializeField]
        private int lobbyDuration;

        private Coroutine timerCoroutine;

        private int totalPlayers;
        private int readyPlayers;

        private void Awake()
        {
            GameManager.Instance.lobbyMenu = this;
        }

        private void Start()
        {
            playerSpawn = PlayerSpawnManager.Instance;
            playerSpawn.onPlayerJoined += RegisterPlayer;
            timerText.text = lobbyDuration.ToString();
        }

        private void RegisterPlayer(PlayerManager playerManager)
        {
            var index = playerManager.playerIndex;
            GameObject playerGameObject = playerManager.gameObject;
            Debug.Log(index);
            playerPrompts[index].text = $"Player {index + 1}\n(X/A) Ready";

            playerManager.playerInput.Player.Disable();
            playerManager.playerInput.UI.Enable();

            playerManager.playerInput.UI.Submit.performed += (InputAction.CallbackContext context) =>
            {
                if (context.ReadValue<float>() == 1)
                {
                    PlayerReadied(playerManager);
                }
            };

            totalPlayers++;

            if (playerSpawn.players.Count == minPlayers)
            {
                timerCoroutine = StartCoroutine(StartTimer());
            }
        }

        private void PlayerReadied(PlayerManager playerManager)
        {
            int playerIndex = playerManager.playerIndex;
            playerPrompts[playerIndex].text = $"Player {playerIndex + 1}\nReady";
            playerPrompts[playerIndex].color = Color.green;

            readyPlayers++;

            ReadyCheck();
        }

        private void ReadyCheck()
        {
            if (totalPlayers >= minPlayers && readyPlayers == totalPlayers)
            {
                if (timerCoroutine != null)
                    StopCoroutine(timerCoroutine);
                timerCoroutine = null;

                OnTimerEnd();
            }
        }

        private IEnumerator StartTimer()
        {
            int timer = lobbyDuration;
            timerText.text = timer.ToString();

            while (timer > 0)
            {
                yield return new WaitForSeconds(1);
                timer--;
                timerText.text = timer.ToString();
            }

            OnTimerEnd();
        }

        private void OnTimerEnd()
        {
            foreach (var player in playerSpawn.players)
            {
                player.playerInput.Player.Enable();
                player.playerInput.UI.Disable();
            }

            onLobbyFinished?.Invoke();
            Destroy(gameObject);
        }
    }
}
