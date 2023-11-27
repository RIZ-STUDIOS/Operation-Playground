using OperationPlayground.Managers;
using OperationPlayground.Player;
using OperationPlayground.Rounds;
using RicTools.Utilities;
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

        private AudioSource supplyDropAudio;

        private void Awake()
        {
            GameManager.Instance.supplyDropManager = this;

            locations = GetComponentsInChildren<SupplyDropLocation>();

            supplyDropAudio = GetComponent<AudioSource>();
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

            supplyDropAudio.Play();

            foreach (var player in PlayerSpawnManager.Instance.Players)
            {
                player.PlayerCanvas.MessageUI.DisplayMessage("<color=#A0C16D>SUPPLY CRATE INCOMING\nLOOK TO THE SKY</color>", 3);
            }
        }
    }
}
