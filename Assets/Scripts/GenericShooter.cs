using OperationPlayground.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground
{
    public enum GameTeam
    {
        TeamA, // Players
        TeamB  // Enemies
    }

    public class GenericShooter : MonoBehaviour
    {
        protected Weapon currentWeapon;

        public virtual GameTeam Team => GameTeam.TeamB;

        public void SwitchWeapon(Weapon weapon)
        {
            if (currentWeapon)
            {
                currentWeapon.gameObject.SetActive(false);
                currentWeapon.SetShooter(null);
            }
            currentWeapon = weapon;
            currentWeapon.gameObject.SetActive(true);
            currentWeapon.SetShooter(this);
        }
    }
}
