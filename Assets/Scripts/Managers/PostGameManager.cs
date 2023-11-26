using OperationPlayground.Loading;
using OperationPlayground.Managers;
using OperationPlayground.Player;
using RicTools.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace OperationPlayground
{
    public class PostGameManager : GenericManager<PostGameManager>
    {
        public bool isPostGame;
        public event System.Action OnVoteValueChanged;

        public AudioSource audioSource;
        public AudioClip winClip;
        public AudioClip loseClip;

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

        protected override void Awake()
        {
            base.Awake();
            audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            GameStateManager.Instance.OnGameOver += SetupPlayers;
            OnVoteValueChanged += EndGameVoteQuery;
        }

        private void SetupPlayers()
        {
            isPostGame = true;

            if (GameStateManager.Instance.IsVictory) audioSource.clip = winClip;
            else audioSource.clip = loseClip;

            audioSource.Play();

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

            audioSource.Stop();
        }
    }
}
