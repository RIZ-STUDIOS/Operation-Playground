using OperationPlayground.Player;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace OperationPlayground
{
    public abstract class ZedButton : MonoBehaviour
    {
        private readonly Int32[] _defaultColorHex = { 0x40, 0x4C, 0x33, 0xFF };

        public Image _buttonBackground;
        public Color _backgroundColor;

        private void Awake()
        {
            _buttonBackground.color = _backgroundColor;
        }

        public virtual void SetButtonSelected()
        {
            Color selectColor = _backgroundColor;
            selectColor.r *= 0.65f;
            selectColor.g *= 0.65f;
            selectColor.b *= 0.65f;
            _buttonBackground.color = selectColor;
        }

        public virtual void SetButtonDeselected()
        {
            _buttonBackground.color = _backgroundColor;
        }

        public abstract void OnSubmit(PlayerManager playerManager);
    }
}
