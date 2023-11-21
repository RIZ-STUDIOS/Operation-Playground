using OperationPlayground.Managers;
using OperationPlayground.Rounds;
using RicTools.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.SupplyDrop
{
    public class SupplyDropManager : MonoBehaviour
    {
        [SerializeField]
        private float dropCheckTimer = 60;

        [SerializeField, Range(0f, 1f)]
        private float dropChance;

        public float dropSpeed;

        private float dropTime;

        private bool timerStarted;

        private SupplyDropLocation[] locations;

        private void Awake()
        {
            GameManager.Instance.supplyDropManager = this;

            locations = GetComponentsInChildren<SupplyDropLocation>();
        }

        private void Start()
        {
            RoundManager.Instance.onRoundStart += () =>
            {
                timerStarted = true;
                dropTime = 0;
            };

            RoundManager.Instance.onRoundEnd += () =>
            {
                timerStarted = false;
            };
        }

        private void Update()
        {
            if (!timerStarted) return;

            dropTime += Time.deltaTime;

            if (dropTime >= dropCheckTimer)
            {
                dropTime = 0;

                var value = Random.value;

                if (value < dropChance)
                {
                    SpawnCrate();
                }
            }
        }

        [ContextMenu("Spawn Crate")]
        private void SpawnCrate()
        {
            SupplyDropLocation location;
            int tries = 0;
            do
            {
                location = locations.GetRandomElement();
                tries++;
            } while (location.HasCrate() && tries < 5);

            if (tries < 5)
            {
                location.SpawnCrate();
            }
        }
    }
}
