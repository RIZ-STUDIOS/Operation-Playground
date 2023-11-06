using OperationPlayground.Player;
using OperationPlayground.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace OperationPlayground.Menus
{
    public class SlotHandler : MonoBehaviour
    {
        //public PlayerMenuData currentPlayer;
        public PlayerManager currentPlayer;
        public GameObject playerSlot;
        public bool playerReady;
        //public CharacterScriptableObject selectedCharacter;

        public delegate void OnPlayerReady(PlayerManager playerManager);
        public OnPlayerReady onPlayerReady;

        public delegate void OnPlayerExit(PlayerManager playerManager);
        public OnPlayerExit onPlayerExit;

        [SerializeField] private GameObject characterSelect;
        [SerializeField] private TextMeshProUGUI joinPrompt;
        [SerializeField] private TextMeshProUGUI activeCharacterPrompt;
        [SerializeField] private Image activeCharacterArt;
        [SerializeField] private CharacterScriptableObject[] availableCharacters;

        private int artIndex = 0;

        private void Awake()
        {
            UpdateCharacterSelect();
        }

        private void OnDestroy()
        {
            if (!currentPlayer) return;
            currentPlayer.playerInput.Basic.Join.performed -= OnJoin;
            currentPlayer.playerInput.UI.Cancel.performed -= OnCancel;
            ToggleInput(false);
        }

        public void ListenForJoin()
        {
            currentPlayer.playerInput.Basic.Join.performed += OnJoin;
            currentPlayer.playerInput.UI.Cancel.performed += OnCancel;
        }

        public void JoinSlot(PlayerManager joiningPlayer)
        {
            currentPlayer = joiningPlayer;
            //currentPlayer.transform.parent = playerSlot.transform;

            ToggleCharacterSelect(true);
            ToggleInput(true);
        }

        public void ClearSlot()
        {
            if (currentPlayer)
            {
                ToggleInput(false);

                Destroy(currentPlayer.gameObject);
                currentPlayer = null;
            }

            ToggleCharacterSelect(false);
        }

        private void ExitSlot()
        {
            if (currentPlayer)
            {
                ToggleInput(false);

                Destroy(currentPlayer.gameObject);
                currentPlayer = null;
            }

            onPlayerExit?.Invoke(currentPlayer);

            ToggleCharacterSelect(false);
        }

        private void ReadyUp()
        {
            currentPlayer.playerInput.UI.Navigate.performed -= OnNavigate;
            //selectedCharacter = availableCharacters[artIndex];

            playerReady = true;
            onPlayerReady?.Invoke(currentPlayer);
            UpdateCharacterSelect();
        }

        private void Unready()
        {
            currentPlayer.playerInput.UI.Navigate.performed += OnNavigate;
            //selectedCharacter = null;

            playerReady = false;
            UpdateCharacterSelect();
        }

        private void ToggleInput(bool turnOnInput)
        {
            if (turnOnInput)
            {
                currentPlayer.playerInput.UI.Navigate.performed += OnNavigate;
                currentPlayer.playerInput.UI.Cancel.performed += OnCancel;
                currentPlayer.playerInput.UI.Submit.performed += OnSubmit;
            }
            else
            {
                currentPlayer.playerInput.UI.Navigate.performed -= OnNavigate;
                currentPlayer.playerInput.UI.Cancel.performed -= OnCancel;
                currentPlayer.playerInput.UI.Submit.performed -= OnSubmit;
            }
        }

        private void OnJoin(InputAction.CallbackContext value)
        {
            ToggleInput(true);
            ToggleCharacterSelect(true);
            currentPlayer.playerInput.Basic.Join.performed -= OnJoin;
        }

        private void OnNavigate(InputAction.CallbackContext value)
        {
            Vector2 input = value.ReadValue<Vector2>();

            if (input.x == 1)
            {
                artIndex++;
                if (artIndex >= availableCharacters.Length) artIndex = 0;
            }
            else if (input.x == -1)
            {
                artIndex--;
                if (artIndex < 0) artIndex = availableCharacters.Length - 1;
            }

            UpdateCharacterSelect();
        }

        private void OnCancel(InputAction.CallbackContext value)
        {
            if (playerReady)
            {
                Unready();
                return;
            }

            ExitSlot();
        }

        private void OnSubmit(InputAction.CallbackContext value)
        {
            if (!playerReady) ReadyUp();
        }

        private void ToggleCharacterSelect(bool enableCharacterSelect)
        {
            if (enableCharacterSelect)
            {
                joinPrompt.gameObject.SetActive(false);
                characterSelect.SetActive(true);

                artIndex = 0;
                UpdateCharacterSelect();
            }
            else
            {
                artIndex = 0;
                UpdateCharacterSelect();

                joinPrompt.gameObject.SetActive(true);
                characterSelect.SetActive(false);
            }
        }

        private void UpdateCharacterSelect()
        {
            if (playerReady)
            {
                activeCharacterArt.sprite = availableCharacters[artIndex].adultArt;
                activeCharacterPrompt.text = $"<color=#5C715C>{availableCharacters[artIndex].characterName} READY!</color>";
            }
            else
            {
                activeCharacterPrompt.text = availableCharacters[artIndex].characterName;
                activeCharacterArt.sprite = availableCharacters[artIndex].childArt;
            }
        }
    }
}
