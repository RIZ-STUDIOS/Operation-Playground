using OperationPlayground.Player;
using OperationPlayground.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace OperationPlayground
{
    public class OptionZedButton : ZedButton
    {
        public VoteCounterUI voteCounter;

        private event System.Action<PlayerManager> OptionAction;

        public void CreateOptionAction(Option menuOption)
        {
            GetComponentInChildren<TextMeshProUGUI>().text = menuOption.text;
            OptionAction = menuOption.action;
        }

        public override void OnSubmit(PlayerManager playerManager)
        {
            OptionAction?.Invoke(playerManager);
        }
    }
}
