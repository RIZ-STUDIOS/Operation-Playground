using OperationPlayground.EntityData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Enemies
{
    public class EnemyHealth : GenericHealth
    {
        private EnemyEntity _enemyEntity;

        public new EnemyEntity parentEntity
        {
            get
            {
                if (!_enemyEntity) _enemyEntity = base.parentEntity as EnemyEntity;
                return _enemyEntity;
            }
        }

        public override int MaxHealth => parentEntity.enemyScriptableObject.maxHealth;
    }
}