using OperationPlayground.Player;

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
                    playerManager.PlayerCanvas.OptionsUI.StopListeningToPlayer();
                    PostGameManager.Instance.VoteRetryCount++;
                }
            );

        public static Option VoteQuit = new Option
            (
                NAME_VOTE_QUIT,
                (PlayerManager playerManager) =>
                {
                    playerManager.PlayerCanvas.OptionsUI.StopListeningToPlayer();
                    PostGameManager.Instance.VoteQuitCount++;
                }
            );

        public static Option Quit = new Option
            (
                NAME_QUIT,
                (PlayerManager playerManager) =>
                {
                    PlayerSpawnManager.Instance.ReturnToMainMenu();
                }
            )
        {
            hasVote = false
        };

        #endregion Options

        public string text;
        public System.Action<PlayerManager> action;
        public bool hasVote;

        public Option(string optionText, System.Action<PlayerManager> optionAction)
        {
            text = optionText;
            action = optionAction;
            hasVote = true;
        }
    }
}
