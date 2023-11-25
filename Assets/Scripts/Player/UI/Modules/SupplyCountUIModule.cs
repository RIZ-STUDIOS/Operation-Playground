using OperationPlayground.Resources;
using TMPro;
using UnityEngine;

namespace OperationPlayground.Player.UI.Modules
{
    public class SupplyCountUIModule : UIModule
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
