using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CharacterPreset", menuName = "1543493/CharacterPreset")]
    public class CharacterScriptableObject : ScriptableObject
    {
        public string characterName;
        public Sprite childArt;
        public Sprite adultArt;
    }
}
