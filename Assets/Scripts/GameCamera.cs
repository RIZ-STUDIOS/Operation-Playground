using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground
{
    public class GameCamera : MonoBehaviour
    {
        private void Awake()
        {
            GameManager.Instance.gameCamera = GetComponent<Camera>();
        }
    }
}
