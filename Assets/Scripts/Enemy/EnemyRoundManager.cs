using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RicTools.Managers;
using System;
using UnityEngine.Splines;
using OperationPlayground.Enemy;
using RicTools.Attributes;
using RicTools.Utilities;
using OperationPlayground.ScriptableObjects;

namespace OperationPlayground.Enemy
{
    public class EnemyRoundManager : GenericManager<EnemyRoundManager>
    {
        protected override bool DestroyIfFound => false;

        [SerializeField]
        private List<EnemyRoundScriptableObject> rounds;

        private List<GameObject> aliveEnemies = new List<GameObject>();

        private List<EnemyScriptableObject> queueEnemies = new List<EnemyScriptableObject>();

        [SerializeField]
        private SplineContainer splineToFollow;

        public event Action onRoundStart;
        public event Action onRoundEnd;
        public event Action onCountdownStart;
        public event Action<float> onCountdownTick;

        private int roundNumber = 1;

        [SerializeField, PositiveValueOnly, ReadOnly(AvailableMode.Play)]
        private float timeBetweenRounds;

        [SerializeField, PositiveValueOnly, ReadOnly(AvailableMode.Play)]
        private float baseSpeed;

        private void Start()
        {
            StartCountdown();
        }

        private void StartCountdown()
        {
            if(rounds.Count <= 0)
            {
                Debug.Log("End of game");
                return;
            }

            StartCoroutine(CountdownCoroutine());
        }

        private IEnumerator CountdownCoroutine()
        {
            Debug.Log("started countdown");

            float timer = timeBetweenRounds;

            onCountdownStart?.Invoke();

            while (timer > 0)
            {
                onCountdownTick?.Invoke(timer);
                yield return null;
                timer -= Time.deltaTime;
            }

            StartRound();
        }

        private void StartRound()
        {
            onRoundStart?.Invoke();

            if(queueEnemies.Count > 0)
            {
                Debug.Log("Starting new round with queued enemies");
                queueEnemies.Clear();
            }

            Debug.Log($"Starting round {roundNumber}");
            var round = GetNextRound();


            foreach (var enemyData in round.enemies)
            {
                for (int i = 0; i < enemyData.count; i++)
                {
                    queueEnemies.Add(enemyData.enemy);
                }
            }

            StartCoroutine(RoundCoroutine(round));

            rounds.Remove(round);
        }

        private IEnumerator RoundCoroutine(EnemyRoundScriptableObject round)
        {
            float spawnTime = 10 / baseSpeed;

            while(queueEnemies.Count > 0)
            {
                SpawnRandomEnemy();
                yield return new WaitForSeconds(spawnTime);
            }
        }

        private void SpawnRandomEnemy()
        {
            if (queueEnemies.Count == 0) return;

            var enemy = queueEnemies.GetRandomElement();

            var gameObject = Instantiate(enemy.prefab);
            gameObject.GetComponent<EnemyHealth>().enemySo = enemy;

            {
                var splineAnimate = gameObject.AddComponent<SplineAnimate>();
                splineAnimate.Loop = SplineAnimate.LoopMode.Once;
                splineAnimate.AnimationMethod = SplineAnimate.Method.Speed;
                splineAnimate.MaxSpeed = baseSpeed;
                splineAnimate.Container = splineToFollow;
            }

            gameObject.AddComponent<EnemyPathing>();

            aliveEnemies.Add(gameObject);

            queueEnemies.Remove(enemy);
        }

        public EnemyRoundScriptableObject GetNextRound()
        {
            if (rounds.Count == 0)
                return null;

            return rounds[0];
        }

        public void RemoveEnemy(GameObject gameObject)
        {
            if (aliveEnemies.Remove(gameObject))
            {
                CheckEndOfRound();
            }
        }

        private void CheckEndOfRound()
        {
            if (queueEnemies.Count > 0)
            {
                onRoundEnd?.Invoke();
                roundNumber++;
                StartCountdown();
            }
        }
    }
}
