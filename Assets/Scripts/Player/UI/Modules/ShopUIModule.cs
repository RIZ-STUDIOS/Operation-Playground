using OperationPlayground.ScriptableObjects;
using OperationPlayground.Shop;
using UnityEngine;

namespace OperationPlayground.Player.UI.Modules
{
    public class ShopUIModule : NavigableUIModule
    {
        public ShopItemScriptableObject[] ShopItems
        {
            get { return _shopItems; }
            set
            {
                if (value == null) return;
                _shopItems = value;
            }
        }
        private ShopItemScriptableObject[] _shopItems;

        protected override void PopulateMenu()
        {
            if (ShopItems.Length <= 0)
            {
                _playerCanvas.MessageUI.DisplayMessage("<color=#EC5D5D>NO ITEMS IN SHOP</color>");
                return;
            }

            foreach (var shopItem in ShopItems)
            {
                GameObject buyButton = Instantiate(_buttonPrefab);

                ShopButton shopButton = buyButton.GetComponent<ShopButton>();
                shopButton.AssignShopItem(shopItem, _playerCanvas.playerManager);
                shopButton.transform.SetParent(_scrollPanel.transform, false);

                _buttonList.Add(shopButton);
            }
        }
    }
}
