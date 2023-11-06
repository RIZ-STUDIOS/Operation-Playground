using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.EntityData
{
    public enum GameTeam
    {
        // Players
        TeamA,
        // Enemies
        TeamB
    }

    public abstract class GenericEntity : MonoBehaviour
    {
        public abstract GenericHealth Health { get; }
        public abstract GenericShooter Shooter { get; }

        public virtual GameTeam Team => GameTeam.TeamB;

        protected virtual void Awake()
        {
            SetParentEntity();
        }

        protected void SetParentEntity()
        {
            Health.parentEntity = this;
            Shooter.parentEntity = this;
        }
    }
}
