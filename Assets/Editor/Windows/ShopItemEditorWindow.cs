using OperationPlayground.ScriptableObjects;
using RicTools.Editor.Utilities;
using RicTools.Editor.Windows;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OperationPlayground.Editor.Windows
{
    public class ShopItemEditorWindow : GenericEditorWindow<ShopItemScriptableObject, AvailableShopItemsScriptableObject>
    {
        public EditorContainer<Sprite> itemSprite;
        public EditorContainer<int> supplyCost;

        public EditorContainer<bool> availableInTruckShop = new EditorContainer<bool>(true);
        public EditorContainer<bool> availableInRadioShop;

        [MenuItem("Operation Playground/Item Shop Editor")]
        public static ShopItemEditorWindow ShowWindow()
        {
            return GetWindow<ShopItemEditorWindow>("Item Shop Editor");
        }

        protected override void DrawGUI()
        {
            {
                var element = rootVisualElement.AddObjectField(itemSprite, "Item Sprite");
                RegisterLoadChange(element, itemSprite);
            }

            {
                var element = rootVisualElement.AddIntField(supplyCost, "Item Cost");
                RegisterLoadChange(element, supplyCost);
            }

            {
                var element = rootVisualElement.AddToggle(availableInTruckShop, "Available in truck");
                RegisterLoadChange(element, availableInTruckShop);
            }

            {
                var element = rootVisualElement.AddToggle(availableInRadioShop, "Available in radio");
                RegisterLoadChange(element, availableInRadioShop);
            }
        }

        protected override void LoadScriptableObject(ShopItemScriptableObject so, bool isNull)
        {
            if(isNull)
            {
                itemSprite.Reset();
                supplyCost.Reset();
                availableInRadioShop.Reset();
                availableInTruckShop.Reset();
            }
            else
            {
                itemSprite.Value = so.itemSprite;
                supplyCost.Value = so.itemCost;
                availableInTruckShop.Value = so.availableInTruckShop;
                availableInRadioShop.Value = so.availableInRadioShop;
            }
        }

        protected override void CreateAsset(ref ShopItemScriptableObject asset)
        {
            asset.itemSprite = itemSprite;
            asset.itemCost = supplyCost;
            asset.availableInRadioShop = availableInRadioShop;
            asset.availableInTruckShop = availableInTruckShop;
        }
    }
}
