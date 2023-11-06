using OperationPlayground.EntityData;
using OperationPlayground.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Enemies
{
    [RequireComponent(typeof(EnemyHealth))]
    [RequireComponent(typeof(EnemyShooter))]
    public class EnemyEntity : GenericEntity
    {
        public EnemyScriptableObject enemyScriptableObject;

        public EnemyHealth EnemyHealth => this.GetIfNull(ref _enemyHealth);
        public EnemyShooter EnemyShooter => this.GetIfNull(ref _enemyShooter);

        private EnemyHealth _enemyHealth;
        private EnemyShooter _enemyShooter;

        public override GenericHealth Health => EnemyHealth;

        public override GenericShooter Shooter => EnemyShooter;
    }
}
