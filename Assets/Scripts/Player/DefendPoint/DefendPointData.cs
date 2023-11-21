using OperationPlayground.EntityData;
using OperationPlayground.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.DefendPoint
{
    [RequireComponent(typeof(DefendPointHealth))]
    public class DefendPointData : GenericEntity
    {
        public override GenericHealth Health => DefendPointHealth;

        public override GenericShooter Shooter => null;

        private DefendPointHealth _defendPointHealth;

        public DefendPointHealth DefendPointHealth => this.GetIfNull(ref _defendPointHealth);

        public override GameTeam Team => GameTeam.TeamA;

        protected override void Awake()
        {
            base.Awake();
            GameManager.Instance.defendPointData = this;
        }
    }
}
