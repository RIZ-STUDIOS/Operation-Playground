using log4net.Util;
using OperationPlayground.Managers;
using OperationPlayground.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.UI
{
    public struct Option
    {
        #region Constant Names

        public const string NAME_QUIT = "QUIT";
        public const string NAME_VOTE_RETRY = "VOTE RETRY";
        public const string NAME_VOTE_QUIT = "VOTE QUIT";

        #endregion Constant Names

        #region Options

        public static Option VoteRetry = new Option
            (
                NAME_VOTE_RETRY,
                (PlayerManager playerManager) =>
                {
                    PostGameManager.Instance.VoteRetryCount++;

                    foreach (var button in playerManager.PlayerCanvas.OptionsUI.ButtonList)
                    {
                        var optionButton = (OptionZedButton)button;
                        optionButton.voteCounter.gameObject.SetActive(true);
                    }
                }
            );

        public static Option VoteQuit = new Option
            (
                NAME_VOTE_QUIT,
                (PlayerManager playerManager) =>
                {
                    PostGameManager.Instance.VoteQuitCount++;

                    foreach (var button in playerManager.PlayerCanvas.OptionsUI.ButtonList)
                    {

                    }
                }
            );

        public static Option Quit = new Option
            (
                NAME_QUIT,
                (PlayerManager playerManager) =>
                {
                    PlayerSpawnManager.Instance.ReturnToMainMenu();
                }
            );

        #endregion Options

        public string text;
        public System.Action<PlayerManager> action;

        public Option(string optionText, System.Action<PlayerManager> optionAction)
        {
            text = optionText;
            action = optionAction;
        }
    }
}
