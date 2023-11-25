using OperationPlayground.Loading;
using OperationPlayground.Managers;
using OperationPlayground.Player;
using RicTools.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground
{
    public class PostGameManager : GenericManager<PostGameManager>
    {
        public bool isPostGame;
        public event System.Action OnVoteValueChanged;

        public int VoteRetryCount
        {
            get { return _voteRetryCount; }
            set
            {
                _voteRetryCount = value;
                if (value < 0) value = 0;
                OnVoteValueChanged?.Invoke();
            }
        }
        private int _voteRetryCount;

        public int VoteQuitCount
        {
            get { return _voteQuitCount; }
            set
            {

                _voteQuitCount = value;
                if (value < 0) value = 0;
                OnVoteValueChanged?.Invoke();
            }
        }
        private int _voteQuitCount;

        private void Start()
        {
            GameStateManager.Instance.OnGameOver += SetupPlayers;
            OnVoteValueChanged += EndGameVoteQuery;
        }

        private void SetupPlayers()
        {
            isPostGame = true;

            foreach (var player in PlayerSpawnManager.Instance.Players)
            {
                player.PlayerCanvas.OptionsUI.OpenMenu();
            }
        }

        private void EndGameVoteQuery()
        {
            if (VoteRetryCount >= PlayerSpawnManager.Instance.Players.Count * 0.5f)
            {
                foreach (var player in PlayerSpawnManager.Instance.Players)
                {
                    player.PlayerCanvas.ResetPlayerUI();
                }
                isPostGame = false;
                PlayerSpawnManager.Instance.LoadIntoLevel();
            }
            else if (VoteQuitCount >= PlayerSpawnManager.Instance.Players.Count * 0.5f)
            {
                isPostGame = false;
                PlayerSpawnManager.Instance.ReturnToMainMenu();
            }
        }
    }
}
