using OperationPlayground.Player.PlayerCapabilities;
using OperationPlayground.ZedExtensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Player.UI.Modules
{
    public class NavigableUIModule : UIModule
    {
        public bool InMenu => _inMenu;
        private bool _inMenu;

        [SerializeField] protected GameObject _buttonPrefab;
        [SerializeField] protected GameObject _scrollPanel;

        protected List<ZedButton> _buttonList;

        private int _navigationIndex = 0;

        protected override void Awake()
        {
            base.Awake();

            _buttonList = new List<ZedButton>();
        }

        public virtual void OpenMenu()
        {
            PopulateMenu();

            FadeRevealModule();

            _playerCanvas.playerManager.RemovePlayerState(PlayerCapabilityType.Movement);
            _playerCanvas.playerManager.RemovePlayerState(PlayerCapabilityType.Interaction);
            _playerCanvas.playerManager.RemovePlayerState(PlayerCapabilityType.Building);
            _playerCanvas.playerManager.RemovePlayerState(PlayerCapabilityType.Shooter);
            _playerCanvas.playerManager.RemovePlayerState(PlayerCapabilityType.ToggleBuilding);

            _buttonList[_navigationIndex].SetButtonSelected();

            EnableMenuInput();

            _inMenu = true;
        }

        public virtual void CloseMenu()
        {
            FadeHideModule();

            DePopulateMenu();

            _playerCanvas.playerManager.AddPlayerState(PlayerCapabilityType.Movement);
            _playerCanvas.playerManager.AddPlayerState(PlayerCapabilityType.Interaction);
            _playerCanvas.playerManager.AddPlayerState(PlayerCapabilityType.Shooter);
            _playerCanvas.playerManager.AddPlayerState(PlayerCapabilityType.ToggleBuilding);

            _buttonList.Clear();

            DisableMenuInput();

            _inMenu = false;
        }

        protected virtual void PopulateMenu() { }

        protected virtual void DePopulateMenu()
        {
            foreach (Transform child in _scrollPanel.transform)
            {
                Destroy(child.gameObject);
            }
        }

        private void OnNavigate(InputAction.CallbackContext value)
        {
            int previousNavigationIndex = _navigationIndex;
            Vector2 input = value.ReadValue<Vector2>();

            switch (input.y)
            {
                // Go up shop buttons.
                case 1:
                    {
                        _navigationIndex--;
                        if (_navigationIndex < 0) _navigationIndex = _buttonList.Count - 1;
                    }
                    break;

                // Go down shop buttons.
                case -1:
                    {
                        _navigationIndex++;
                        if (_navigationIndex >= _buttonList.Count) _navigationIndex = 0;
                    }
                    break;

                default:
                    return;
            }

            _buttonList[previousNavigationIndex].SetButtonDeselected();
            _buttonList[_navigationIndex].SetButtonSelected();
        }

        private void OnCancelPerformed(InputAction.CallbackContext value)
        {
            CloseMenu();
        }

        private void OnSubmit(InputAction.CallbackContext value)
        {
            _buttonList[_navigationIndex].OnSubmit(_playerCanvas.playerManager);
        }

        private void EnableMenuInput()
        {
            _playerCanvas.playerManager.playerInput.Basic.Disable();
            _playerCanvas.playerManager.playerInput.UI.Cancel.performed += OnCancelPerformed;
            _playerCanvas.playerManager.playerInput.UI.Navigate.performed += OnNavigate;
            _playerCanvas.playerManager.playerInput.UI.Submit.performed += OnSubmit;
        }

        private void DisableMenuInput()
        {
            _playerCanvas.playerManager.playerInput.UI.Submit.performed -= OnSubmit;
            _playerCanvas.playerManager.playerInput.UI.Navigate.performed -= OnNavigate;
            _playerCanvas.playerManager.playerInput.UI.Cancel.performed -= OnCancelPerformed;
            _playerCanvas.playerManager.playerInput.Basic.Enable();
        }
    }
}
