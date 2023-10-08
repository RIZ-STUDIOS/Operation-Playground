using OperationPlayground.Player;
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
        public System.Action lobbyFinished;

        [SerializeField]
        private TextMeshProUGUI timerText;

        [SerializeField]
        private TextMeshProUGUI[] playerPrompts;

        [SerializeField]
        private PlayerSpawnManager playerSpawn;

        [SerializeField]
        private int lobbyDuration;

        private Coroutine timerCoroutine;

        private int totalPlayers;
        private int readyPlayers;

        private void Start()
        {
            playerSpawn.playerJoined += RegisterPlayer;
            timerText.text = lobbyDuration.ToString();
        }

        private void RegisterPlayer()
        {
            int index = playerSpawn.players.Count - 1;
            GameObject player = playerSpawn.players[index];
            Debug.Log(index);
            playerPrompts[index].text = $"Player {index + 1}\n(X/A) Ready";

            var playerInput = player.GetComponent<PlayerInput>();
            playerInput.SwitchCurrentActionMap("UI");

            playerInput.actions.FindAction("Submit").performed += (InputAction.CallbackContext value) =>
            { 
                if (value.ReadValue<float>() == 1)
                {
                    PlayerReadied(player);
                }
            };

            totalPlayers++;

            if (playerSpawn.players.Count == 2)
            {
                timerCoroutine = StartCoroutine(StartTimer());
            }
        }

        private void PlayerReadied(GameObject player)
        {
            int playerIndex = player.GetComponent<PlayerInputManager>().playerIndex;
            playerPrompts[playerIndex].text = $"Player {playerIndex + 1}\nReady";
            playerPrompts[playerIndex].color = Color.green;

            readyPlayers++;

            ReadyCheck();
        }

        private void ReadyCheck()
        {
            if (totalPlayers > 1 && readyPlayers == totalPlayers)
            {
                timerCoroutine = null;

                foreach (var player in playerSpawn.players)
                {
                    player.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
                }

                gameObject.SetActive(false);
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

            foreach (var player in playerSpawn.players)
            {
                player.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
            }

            lobbyFinished.Invoke();
            gameObject.SetActive(false);
        }
    }
}
