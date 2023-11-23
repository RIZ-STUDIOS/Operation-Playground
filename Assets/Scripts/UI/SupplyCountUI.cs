using OperationPlayground.Managers;
using OperationPlayground.Resources;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace OperationPlayground.UI
{
    public class SupplyCountUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI text;

        public void SubscribeEvent()
        {
            ResourceManager.Instance.onSupplyChange += OnSupplyChange;
            OnSupplyChange();
        }

        private void OnSupplyChange()
        {
            text.text = ResourceManager.Instance.Supplies.ToString();
        }
    }
}
