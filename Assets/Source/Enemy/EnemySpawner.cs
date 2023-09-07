using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy Get(Enemy enemyTemplate, Cell spawnCell)
    {
        Enemy enemy = Instantiate(enemyTemplate);
        enemy.transform.localPosition = spawnCell.transform.localPosition;
        return enemy;
    }
}
