using OperationPlayground.EntityData;
using OperationPlayground.Managers;
using OperationPlayground.Rounds;
using OperationPlayground.UI;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

        [SerializeField]
        private Vector3 roundTimerOffset;

        private RadialBarUI timerRadialBar;

        protected override void Awake()
        {
            base.Awake();
            GameManager.Instance.defendPointData = this;

            var timerRadialBarObject = Instantiate(PrefabsManager.Instance.data.radialBarUIPrefab, transform);
            timerRadialBarObject.transform.localScale = Vector3.one * 3.5f;
            timerRadialBarObject.transform.localPosition = roundTimerOffset;
            timerRadialBar = timerRadialBarObject.GetComponent<RadialBarUI>();
            Destroy(timerRadialBarObject.GetComponent<BillboardObject>());
        }

        private void Start()
        {
            RoundManager.Instance.onPreRoundStart += () => timerRadialBar.PercentFilled = 1;
            RoundManager.Instance.onPreRoundEnd += () => timerRadialBar.PercentFilled = 0;
        }

        private void Update()
        {
            if (RoundManager.Instance.RoundStatus == RoundStatus.PreRound)
            {
                timerRadialBar.PercentFilled = 1 - (RoundManager.Instance.PreRoundTime / RoundManager.Instance.PreRoundTimer);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawCube(transform.position + roundTimerOffset, Vector3.one);
        }
    }
}
