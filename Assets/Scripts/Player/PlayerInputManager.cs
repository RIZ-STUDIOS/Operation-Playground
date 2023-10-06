using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace OperationPlayground
{
    public class PlayerInputManager : MonoBehaviour
    {
        [System.NonSerialized]
        public ReadOnlyArray<InputDevice> devices;

        [System.NonSerialized]
        public int playerIndex;

        public OPPlayerInput playerInput;

        [SerializeField, Tooltip("When entering/dying")]
        private Component[] scriptsToDisable;

        [SerializeField, Tooltip("When entering/dying")]
        private GameObject[] gameObjectsToDisable;

        private void Awake()
        {
            playerInput = new OPPlayerInput();
            playerInput.Enable();
        }

        private void Start()
        {
            playerInput.devices = devices;
        }

        public void DisablePlayer()
        {
            foreach (var script in scriptsToDisable)
            {
                var property = script.GetType().GetProperty("enabled");
                if (property != null)
                {
                    property.SetValue(script, false);
                }
            }

            foreach (var gameObject in gameObjectsToDisable)
            {
                gameObject.SetActive(false);
            }
        }

        public void EnablePlayer()
        {
            foreach (var script in scriptsToDisable)
            {
                var property = script.GetType().GetProperty("enabled");
                if (property != null)
                {
                    property.SetValue(script, true);
                }
            }

            foreach (var gameObject in gameObjectsToDisable)
            {
                gameObject.SetActive(true);
            }
        }
    }
}
