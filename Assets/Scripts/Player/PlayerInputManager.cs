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

        private void Awake()
        {
            playerInput = new OPPlayerInput();
            playerInput.Enable();
        }

        private void Start()
        {
            playerInput.devices = devices;
        }
    }
}
