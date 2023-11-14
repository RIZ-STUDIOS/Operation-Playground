using OperationPlayground.Player;
using RicTools.Attributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Menus
{
    public class LobbyHandler : MonoBehaviour
    {
        //public List<PlayerMenuData> players;
        public System.Action onLobbyEnded;

        private SlotHandler[] playerSlots;

        [SerializeField, MinValue(1)]
        private int playersNeedForGame = 2;

        private void Awake()
        {
            //players = new List<PlayerMenuData>();
            playerSlots = GetComponentsInChildren<SlotHandler>();

            for (int i = 0; i < playerSlots.Length; i++)
            {
                //if (i == 0) playerSlots[i].onPlayerExit += EndLobby;
                playerSlots[i].onPlayerExit += UpdatePlayers;
                playerSlots[i].onPlayerReady += ReadyCheck;
            }
        }

        private void Start()
        {
            PlayerSpawnManager.Instance.onPlayerJoin += OnPlayerJoin;
        }

        public void StartLobby()
        {
            PlayerSpawnManager.Instance.EnableJoining();

            if (playerSlots[0].currentPlayer) playerSlots[0].ListenForJoin();
        }

        private void EndLobby()
        {
            foreach (SlotHandler slot in playerSlots)
            {
                if (slot.currentPlayer && slot.currentPlayer.playerIndex != 0)
                {
                    slot.ClearSlot();
                }
            }

            PlayerSpawnManager.Instance.DisableJoining();
            onLobbyEnded?.Invoke();
        }

        private void ReadyCheck(PlayerManager playerManager)
        {
            int currentPlayers = 0;
            int readyPlayers = 0;
            foreach (SlotHandler slot in playerSlots)
            {
                if (slot.currentPlayer) currentPlayers++;
                if (slot.playerReady) readyPlayers++;
            }

            if (currentPlayers >= playersNeedForGame && currentPlayers == readyPlayers) PlayerSpawnManager.Instance.StartGame();
        }

        private void UpdatePlayers(PlayerManager playerManager)
        {
            if (PlayerSpawnManager.Instance.TotalPlayers <= 1) EndLobby();
            //players.RemoveAll(player => player == null);
        }

        private void OnPlayerJoin(PlayerManager playerManager)
        {
            playerSlots[playerManager.playerIndex].JoinSlot(playerManager);
        }

        /*private void OnPlayerJoined(PlayerInput playerInput)
        {
            PlayerMenuData pMD = playerInput.GetComponent<PlayerMenuData>();
            pMD.lobbyIndex = playerInput.playerIndex;
            pMD.gameObject.name = $"Menu Player {pMD.lobbyIndex}";
            players.Add(pMD);

            pMD.playerInput.devices = playerInput.devices;

            playerSlots[pMD.lobbyIndex].JoinSlot(pMD);
        }*/

        private void StartGame()
        {
            // Load the level.
        }

    }
}
