using OperationPlayground.Enemies;
using OperationPlayground.Pathfinding;
using OperationPlayground.ScriptableObjects;
using RicTools.Attributes;
using RicTools.Managers;
using RicTools.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

namespace OperationPlayground.Rounds
{
    public class RoundManager : GenericManager<RoundManager>
    {
        [SerializeField, PositiveValueOnly]
        private float preRoundTimer;

        [SerializeField]
        private PathWaypointList waypointList;

        public float PreRoundTimer => preRoundTimer;

        public float PreRoundTime => preRoundTime;

        [SerializeField, ReadOnly(AvailableMode.Play)]
        private RoundScriptableObject[] rounds;

        private List<RoundScriptableObject> roundList;

        public event System.Action onRoundStart;
        public event System.Action onRoundEnd;
        public event System.Action onPreRoundStart;
        public event System.Action onPreRoundEnd;
        public event System.Action onAllRoundsFinish;

        private float preRoundTime;

        public RoundData CurrentRound { get; private set; }

        private RoundStatus roundStatus;

        protected override void Awake()
        {
            base.Awake();
            roundList = new List<RoundScriptableObject>(rounds);
        }

        private void Start()
        {
            StartRounds();
        }

        private void Update()
        {
            if (roundStatus == RoundStatus.PreRound)
            {
                preRoundTime += Time.deltaTime;

                if(preRoundTime >= preRoundTimer)
                {
                    Debug.Log("Ended Pre Round");
                    onPreRoundEnd?.Invoke();
                    StartRound();
                }
            }
        }

        [ContextMenu("Start Rounds")]
        public void StartRounds()
        {
            StartPreRound();
        }

        private void StartPreRound()
        {
            if (CheckFinishRounds())
            {
                return;
            }

            roundStatus = RoundStatus.PreRound;
            preRoundTime = 0;
            Debug.Log("Started Pre Round");
            onPreRoundStart?.Invoke();
        }

        private void StartRound()
        {
            roundStatus = RoundStatus.Round;

            Debug.Log("Started Round");
            onRoundStart?.Invoke();

            var roundData = roundList[0];

            var enemyCount = Random.Range(roundData.minEnemies, roundData.maxEnemies);
            var enemies = new List<RoundEnemyData>(roundData.enemies);

            CurrentRound = new RoundData(enemyCount, roundData.spawnDelay, enemies);

            roundList.RemoveAt(0);

            StartCoroutine(SpawnEnemiesRoundCoroutine());
        }

        private bool CheckFinishRounds()
        {
            return roundList.Count <= 0;
        }

        public void EnemyKilled()
        {
            CurrentRound.killedEnemies++;

            if(CurrentRound.killedEnemies == CurrentRound.enemyCount)
            {
                Debug.Log("Ended Round");
                onRoundEnd?.Invoke();
                if (CheckFinishRounds())
                {
                    roundStatus = RoundStatus.Finished;
                    Debug.Log("Finished all round");
                    onAllRoundsFinish?.Invoke();
                    return;
                }
                StartPreRound();
            }
        }

        private IEnumerator SpawnEnemiesRoundCoroutine()
        {
            while(CurrentRound.spawnEnemies < CurrentRound.enemyCount)
            {
                float timer = 0;

                while(timer < CurrentRound.spawnDelay)
                {
                    timer += Time.deltaTime;
                    yield return null;
                }

                SpawnRandomEnemy();
            }
        }

        private void SpawnRandomEnemy()
        {
            var enemySo = CurrentRound.enemies.GetRandomElement();

            var position = waypointList.GetFirstWaypoint().GetRandomPoint();

            if (Physics.Raycast(new Vector3(position.x, 100, position.z), Vector3.down, out var hitInfo, 1000f, LayerMask.GetMask("Ground"), QueryTriggerInteraction.Ignore))
            {
                position.y = hitInfo.point.y;
            }

            var enemy = EnemyEntity.SpawnEnemy(enemySo.enemySo, position);

            var followPath = enemy.GetComponent<FollowWaypoints>();

            if (followPath)
            {
                followPath.SetWaypointList(waypointList);
                followPath.NextWaypoint();
            }

            CurrentRound.spawnEnemies++;
        }
    }

    public enum RoundStatus
    {
        None,
        PreRound,
        Round,
        Finished,
    }

    public class RoundData
    {
        public int enemyCount;
        public float spawnDelay;
        public List<RoundEnemyData> enemies;
        public int spawnEnemies;
        public int killedEnemies;

        public RoundData(int maxEnemies, float spawnDelay, List<RoundEnemyData> enemies)
        {
            this.enemyCount = maxEnemies;
            this.enemies = enemies;
            this.spawnDelay = spawnDelay;
            spawnEnemies = 0;
            killedEnemies = 0;
        }
    }
}
