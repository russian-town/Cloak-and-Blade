using System;
using Source.Gameboard.Cell;
using UnityEngine;

namespace Source.Root
{
    [Serializable]
    public class EnemySetter
    {
        [SerializeField] private Enemy.Enemy _enemyTemplate;
        [SerializeField] private Cell[] _destinations;

        public Enemy.Enemy EnemyTemplate => _enemyTemplate;

        public Cell[] Destinations => _destinations;
    }
}