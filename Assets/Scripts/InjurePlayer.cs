using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player
{
    public class InjurePlayer : MonoBehaviour
    {
        public PlayerSpawnManager ps;

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                ps.players[0].GetComponent<PlayerHealth>().Damage(1);
            }
        }
    }
}
