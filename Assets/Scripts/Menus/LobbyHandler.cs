using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Menus
{
    public class LobbyHandler : MonoBehaviour
    {
        public List<PlayerMenuData> players;

        private SlotHandler[] playerSlots;
        private PlayerInputManager playerInputManager;

        private void Awake()
        {
            players = new List<PlayerMenuData>();
            playerInputManager = GetComponent<PlayerInputManager>();
            playerSlots = GetComponentsInChildren<SlotHandler>();
            
            for (int i = 0; i < playerSlots.Length; i++)
            {
                if (i == 0) playerSlots[i].onPlayerExit += EndLobby;
                else playerSlots[i].onPlayerExit += UpdatePlayers;
                playerSlots[i].onPlayerReady += ReadyCheck;
            }
        }

        public void StartLobby()
        {
            playerInputManager.EnableJoining();

            if (playerSlots[0].currentPlayer) playerSlots[0].ListenForJoin();
        }

        private void EndLobby()
        {
            foreach (SlotHandler slot in playerSlots)
            {
                slot.ClearSlot();
            }
            
            playerInputManager.DisableJoining();
            MainMenu.Instance.HideLobby();
        }

        private void ReadyCheck()
        {
            int currentPlayers = 0;
            int readyPlayers = 0;
            foreach (SlotHandler slot in playerSlots)
            {
                if (slot.currentPlayer) currentPlayers++;
                if (slot.playerReady) readyPlayers++;
            }

            if (currentPlayers > 2 && currentPlayers == readyPlayers) StartGame();
        }

        private void UpdatePlayers()
        {
            players.RemoveAll(player => player == null);
        }

        private void OnPlayerJoined(PlayerInput playerInput)
        {
            PlayerMenuData pMD = playerInput.GetComponent<PlayerMenuData>();
            pMD.lobbyIndex = playerInput.playerIndex;
            pMD.gameObject.name = $"Menu Player {pMD.lobbyIndex}";
            players.Add(pMD);

            pMD.playerInput.devices = playerInput.devices;

            playerSlots[pMD.lobbyIndex].JoinSlot(pMD);
        }

        private void StartGame()
        {
            // Load the level.
        }

    }
}
