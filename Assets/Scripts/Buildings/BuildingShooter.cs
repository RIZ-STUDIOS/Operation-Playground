using OperationPlayground.EntityData;
using OperationPlayground.ScriptableObjects;
using OperationPlayground.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Buildings
{
    public class BuildingShooter : GenericShooter
    {
        private Weapon weapon;

        private BuildingData _buildingData;

        public new BuildingData parentEntity
        {
            get
            {
                if (!_buildingData) _buildingData = base.parentEntity as BuildingData;
                return _buildingData;
            }
        }

        private bool triggerDown;

        private void Awake()
        {
            weapon = GetComponent<Weapon>();

            SetWeapon(weapon);

            parentEntity.onFirePerformed += OnFirePerformed;
            parentEntity.onFireCanceled += OnFireCanceled;
        }

        private void OnFirePerformed(InputAction.CallbackContext context)
        {
            triggerDown = true;
        }

        private void OnFireCanceled(InputAction.CallbackContext context)
        {
            triggerDown = false;
        }

        private void LateUpdate()
        {
            if (triggerDown)
            {
                if (!_currentWeapon) return;
                if (_currentWeapon.Shoot())
                {
                    if (!_currentWeapon.HasAmmo())
                    {
                        //var newWeapon = FindWeaponWithAmmo();
                    }
                }
            }
        }
    }
}
