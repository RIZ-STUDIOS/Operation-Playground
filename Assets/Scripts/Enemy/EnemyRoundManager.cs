using OperationPlayground.Enemy;
using OperationPlayground.Managers;
using OperationPlayground.ScriptableObjects;
using RicTools.Attributes;
using RicTools.Managers;
using RicTools.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

namespace OperationPlayground.Enemy
{
    public class EnemyRoundManager : GenericManager<EnemyRoundManager>
    {
        protected override bool DestroyIfFound => false;

        [SerializeField]
        private List<EnemyRoundScriptableObject> rounds;

        public GameRound currentRound, nextRound;

        [SerializeField]
        private SplineContainer splineToFollow;

        public event Action onRoundStart;
        public event Action onRoundEnd;
        public event Action onCountdownStart;
        public event Action<float> onCountdownTick;
        public event Action onEnemyKilled;

        private int roundNumber = 1;

        [SerializeField, PositiveValueOnly, ReadOnly(AvailableMode.Play)]
        private float timeBetweenRounds;

        public float TimeBetweenRounds => timeBetweenRounds;

        [SerializeField, PositiveValueOnly, ReadOnly(AvailableMode.Play)]
        private float baseSpeed;

        private Coroutine countdownCoroutine;
        private Coroutine roundCoroutine;

        private void Start()
        {
            StartCountdown();
        }

        private void StartCountdown()
        {
            if (rounds.Count <= 0)
            {
                GameManager.Instance.loseWinUI.ShowWin();
                return;
            }

            countdownCoroutine = StartCoroutine(CountdownCoroutine());
        }

        private IEnumerator CountdownCoroutine()
        {
            Debug.Log("started countdown");

            float timer = timeBetweenRounds;

            UpdateNextRound();

            onCountdownStart?.Invoke();

            while (timer > 0)
            {
                onCountdownTick?.Invoke(timer);
                yield return null;
                timer -= Time.deltaTime;
            }

            StartRound();
        }

        private void UpdateNextRound()
        {
            nextRound = null;

            var round = GetNextRoundScriptableObject();
            if(round != null)
            {
                nextRound = new GameRound();
                nextRound.Populate(round);
            }
        }

        private void StartRound()
        {
            onRoundStart?.Invoke();

            if (currentRound != null)
            {
                Debug.LogWarning("Starting new round during a round");
                currentRound = null;
            }

            Debug.Log($"Starting round {roundNumber}");
            var round = GetNextRoundScriptableObject();

            currentRound = new GameRound();
            currentRound.Populate(round);

            roundCoroutine = StartCoroutine(RoundCoroutine());

            rounds.Remove(round);
        }

        private IEnumerator RoundCoroutine()
        {
            float spawnTime = 10 / baseSpeed;

            while (currentRound.queueEnemies.Count > 0)
            {
                SpawnRandomEnemy();
                yield return new WaitForSeconds(spawnTime);
            }
        }

        private void SpawnRandomEnemy()
        {
            if (currentRound.queueEnemies.Count == 0) return;

            var enemy = currentRound.queueEnemies.GetRandomElement();

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

            {
                var enemyAttack = gameObject.AddComponent<EnemyAttack>();
                enemyAttack.enemySo = enemy;
            }

            gameObject.tag = "Enemy";

            currentRound.aliveEnemies.Add(gameObject);

            currentRound.queueEnemies.Remove(enemy);
        }

        public EnemyRoundScriptableObject GetNextRoundScriptableObject()
        {
            if (rounds.Count == 0)
                return null;

            return rounds[0];
        }

        public void RemoveEnemy(GameObject gameObject)
        {
            if (currentRound != null && currentRound.aliveEnemies.Remove(gameObject))
            {
                onEnemyKilled?.Invoke();
                CheckEndOfRound();
            }
        }

        private void CheckEndOfRound()
        {
            if (currentRound == null || currentRound.queueEnemies.Count <= 0 && currentRound.aliveEnemies.Count <= 0)
            {
                onRoundEnd?.Invoke();
                roundNumber++;
                StartCountdown();
            }
        }

        public void StopRounds()
        {
            if (countdownCoroutine != null)
                StopCoroutine(countdownCoroutine);

            if (roundCoroutine != null)
                StopCoroutine(roundCoroutine);

            if(currentRound != null)
            foreach (var enemy in currentRound.aliveEnemies)
            {
                enemy.GetComponent<SplineAnimate>().Pause();
            }
        }
    }

    public class GameRound
    {
        public List<GameObject> aliveEnemies = new List<GameObject>();
        public List<EnemyScriptableObject> queueEnemies = new List<EnemyScriptableObject>();

        public void Populate(EnemyRoundScriptableObject round)
        {
            foreach (var enemyData in round.enemies)
            {
                for (int i = 0; i < enemyData.count; i++)
                {
                    queueEnemies.Add(enemyData.enemy);
                }
            }
        }

        public string EnemiesToString()
        {
            SortedDictionary<string, int> roundEnemies = new SortedDictionary<string, int>();

            foreach (var queueEnemy in queueEnemies)
            {
                if (!roundEnemies.TryGetValue(queueEnemy.id, out var value))
                {
                    roundEnemies.Add(queueEnemy.id, value);
                }

                value++;

                roundEnemies[queueEnemy.id] = value;
            }

            foreach (var enemy in aliveEnemies)
            {
                var enemySo = enemy.GetComponent<EnemyAttack>().enemySo;
                if (!roundEnemies.TryGetValue(enemySo.id, out var value))
                {
                    roundEnemies.Add(enemySo.id, value);
                }

                value++;

                roundEnemies[enemySo.id] = value;
            }

            List<string> enemyString = new List<string>();

            foreach (var enemy in roundEnemies.OrderBy(e => e.Key))
            {
                enemyString.Add($"{enemy.Value} {enemy.Key}");
            }

            /*foreach (var enemy in data.supportEnemies)
            {
                enemyString.Add($"{enemy.count} {enemy.enemy.id}");
            }*/

            return string.Join("\n", enemyString);
        }
    }
}
