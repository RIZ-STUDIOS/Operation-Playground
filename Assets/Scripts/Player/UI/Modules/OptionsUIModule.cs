using OperationPlayground.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Player.UI.Modules
{
    public class OptionsUIModule : NavigableUIModule
    {
        protected override bool IsInteractable => true;
        protected override bool CanBlockRaycasts => false;

        public bool hasVoted;

        private Option[] _startOptions =
        {
            Option.Quit
        };

        private Option[] _postGameOptions =
        {
            Option.VoteRetry,
            Option.VoteQuit
        };

        private Option[] _currentOptions;

        protected override void Awake()
        {
            base.Awake();

            _playerCanvas.playerManager.playerInput.Basic.Join.performed += OpenOptions;
        }

        public void StopListeningToPlayer()
        {
            _playerCanvas.playerManager.playerInput.UI.Submit.performed -= OnSubmit;
            _playerCanvas.playerManager.playerInput.UI.Navigate.performed -= OnNavigate;
            _playerCanvas.playerManager.playerInput.UI.Cancel.performed -= OnCancelPerformed;
        }

        protected override void PopulateMenu()
        {
            if (PostGameManager.Instance.isPostGame) _currentOptions = _postGameOptions;
            else _currentOptions = _startOptions;

            if (_currentOptions.Length <= 0)
            {
                _playerCanvas.MessageUI.DisplayMessage("<color=#EC5D5D>NO OPTIONS AVAILABLE</color>");
                return;
            }

            foreach (var option in _currentOptions)
            {
                GameObject optionButtonGO = Instantiate(_buttonPrefab);

                var optionButton = optionButtonGO.GetComponent<OptionZedButton>();
                optionButton.CreateOptionAction(option);
                optionButtonGO.transform.SetParent(_scrollPanel.transform, false);

                _buttonList.Add(optionButton);
            }
        }

        private void OpenOptions(InputAction.CallbackContext value)
        {
            OpenMenu();
        }
    }
}
