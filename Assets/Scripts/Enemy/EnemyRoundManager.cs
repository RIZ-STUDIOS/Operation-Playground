using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RicTools.Managers;
using System;
using UnityEngine.Splines;
using OperationPlayground.Enemy;

namespace OperationPlayground
{
    public class EnemyRoundManager : GenericManager<EnemyRoundManager>
    {
        protected override bool DestroyIfFound => false;

        [SerializeField]
        private List<EnemyRoundScriptableObject> rounds;

        private List<GameObject> aliveGameObjects = new List<GameObject>();

        [SerializeField]
        private SplineContainer splineToFollow;

        private void Start()
        {
            StartRound();
        }

        public void StartRound()
        {
            if(rounds.Count == 0)
            {
                Debug.Log("end of game");
                return; 
            }

            var round = rounds[0];

            StartCoroutine(SpawnEnemiesCoroutine(round));

            rounds.RemoveAt(0);
        }

        private IEnumerator SpawnEnemiesCoroutine(EnemyRoundScriptableObject round)
        {
            var enemies = round.enemies;

            for (int i = 0; i < enemies.Length; i++)
            {
                var enemy = enemies[i];
                for (int j = 0; j < enemy.count; j++)
                {
                    GameObject gameObject = GameObject.Instantiate(enemy.enemy.prefab);
                    var enemyHealth = gameObject.AddComponent<EnemyHealth>();
                    enemyHealth.enemySo = enemy.enemy;

                    {
                        var splineAnimator = gameObject.AddComponent<SplineAnimate>();
                        splineAnimator.Container = splineToFollow;
                        splineAnimator.AnimationMethod = SplineAnimate.Method.Speed;
                        splineAnimator.MaxSpeed = 2.5f;
                    }

                    aliveGameObjects.Add(gameObject);

                    yield return new WaitForSeconds(1.5f);
                }
            }
        }

        public void RemoveEnemy(GameObject gameObject)
        {
            aliveGameObjects.Remove(gameObject);
            CheckEndOfRound();
        }

        private void CheckEndOfRound()
        {
            if(aliveGameObjects.Count <= 0)
            {
                StartRound();
            }
        }
    }
}
