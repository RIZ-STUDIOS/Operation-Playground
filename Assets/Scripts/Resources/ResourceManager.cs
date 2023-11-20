using RicTools.Attributes;
using RicTools.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Resources
{
    public class ResourceManager : GenericManager<ResourceManager>
    {
        private int _supplies;

        public int Supplies
        {
            get { return _supplies; }
            set
            {
                _supplies = value;
                onSupplyChange?.Invoke();
            }
        }

        public event System.Action onSupplyChange;

        [ContextMenu("Add 100 supplies")]
        private void AddResource()
        {
            Supplies += 100;
        }

        [ContextMenu("Add 100 supplies", true)]
        private bool AddResourceValidate()
        {
            return Application.isPlaying;
        }
    }
}
