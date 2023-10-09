using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground
{
    public enum TargetType
    {
        Building,
        Player
    }

    public class EnemyTarget : MonoBehaviour
    {
        public bool visible = true;
        public TargetType type;
    }
}
