using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Enemy Get(Cell spawnCell, Enemy template)
    {
        Enemy enemy = Instantiate(template);
        enemy.transform.localPosition = spawnCell.transform.localPosition;
        return enemy;
    }
}
