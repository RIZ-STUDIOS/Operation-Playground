using OperationPlayground.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground
{
    public class FollowMouse : MonoBehaviour
    {
        private void Update()
        {
            if (Physics.Raycast(GameManager.Instance.gameCamera.camera.ScreenPointToRay(Input.mousePosition), out var hitInfo))
            {
                transform.position = hitInfo.point;
            }
        }
    }
}
