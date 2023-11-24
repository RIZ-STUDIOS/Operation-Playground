using OperationPlayground.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OperationPlayground
{
    public abstract class ZedButton : MonoBehaviour
    {
        public Image _buttonBackground;
        public Color _normalColor;

        public virtual void SetButtonSelected()
        {
            Color selectColor = _normalColor;
            selectColor.r *= 0.65f;
            selectColor.g *= 0.65f;
            selectColor.b *= 0.65f;
            _buttonBackground.color = selectColor;
        }

        public virtual void SetButtonDeselected()
        {
            _buttonBackground.color = _normalColor;
        }

        public abstract void OnSubmit(PlayerManager playerManager);
    }
}
