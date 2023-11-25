using OperationPlayground.Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace OperationPlayground
{
    public class VoteCounterUI : MonoBehaviour
    {
        public bool isCountingRetry;

        private TextMeshProUGUI _voteText;

        private void Awake()
        {
            _voteText = GetComponentInChildren<TextMeshProUGUI>();
            UpdateVoteCount();
            PostGameManager.Instance.OnVoteValueChanged += UpdateVoteCount;
        }

        public void UpdateVoteCount()
        {
            if (isCountingRetry)
                _voteText.text = $"<color=green>{PostGameManager.Instance.VoteRetryCount}</color>/<color=blue>{PlayerSpawnManager.Instance.TotalPlayers}";

            else
                _voteText.text = $"<color=green>{PostGameManager.Instance.VoteQuitCount}</color>/<color=blue>{PlayerSpawnManager.Instance.TotalPlayers}";
        }
    }
}
