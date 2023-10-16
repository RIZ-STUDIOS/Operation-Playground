using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.SupplyCreates
{
    public class SupplyCrateGraphics : MonoBehaviour
    {
        private SupplyCrate supplyCrate;

        [SerializeField]
        private GameObject crateGraphics;

        [SerializeField]
        private GameObject parachuteGraphics;

        private void Awake()
        {
            supplyCrate = GetComponent<SupplyCrate>();

            supplyCrate.onLand += DestroyParachute;
        }

        private void DestroyParachute()
        {
            Destroy(parachuteGraphics);
        }
    }
}
