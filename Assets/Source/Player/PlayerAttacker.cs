using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    private List<Enemy> _enemies = new List<Enemy>();

    public void Initialize(List<Enemy> enemies)
    {
        _enemies.AddRange(enemies);
    }

    public void Attack(Ability ability)
    {
        if (_enemies.Count == 0)
            return;

        foreach (var enemy in _enemies)
            enemy.TakeAbility(ability);
    }
}
