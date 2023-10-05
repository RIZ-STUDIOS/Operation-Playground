using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace OperationPlayground
{
    public class PlayerInputData : MonoBehaviour
    {
        [System.NonSerialized]
        public ReadOnlyArray<InputDevice> devices;

        [System.NonSerialized]
        public int playerIndex;
    }
}
