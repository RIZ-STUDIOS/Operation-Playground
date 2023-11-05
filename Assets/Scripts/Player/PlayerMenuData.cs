using OperationPlayground.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground
{
    public class PlayerMenuData : MonoBehaviour
    {
        public int lobbyIndex;
        public OPPlayerInput playerInput;
        public CharacterScriptableObject character;

        private void Awake()
        {
            if (playerInput != null) return;
            playerInput = new OPPlayerInput();
            playerInput.Enable();
        }
    }
}
