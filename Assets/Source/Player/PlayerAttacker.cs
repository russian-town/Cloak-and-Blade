using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour
{
    private List<Enemy> _enemies = new List<Enemy>();

    public void Initialize(List<Enemy> enemies)
    {
        _enemies.AddRange(enemies);
    }

    public void Attack(AttackType type)
    {
        switch (type)
        {
            case AttackType.Freeze:
                FreezeEnemies();
                break;
            case AttackType.Blind:
                BlindEnemies();
                break;
            case AttackType.UnFreeze:
                UnFreezeEnemies();
                break;
            case AttackType.UnBlind:
                UnBlindEnemies();
                break;
            default:
                break;
        }
    }

    private void FreezeEnemies()
    {
        foreach (var enemy in _enemies)
            enemy.Freeze();
    }

    private void UnFreezeEnemies()
    {
        foreach (var enemy in _enemies)
            enemy.UnFreeze();
    }

    private void BlindEnemies()
    {
        foreach (var enemy in _enemies)
            enemy.Blind();
    }

    private void UnBlindEnemies()
    {
        foreach (var enemy in _enemies)
            enemy.UnBlind();
    }
}

public enum AttackType
{
    Freeze,
    Blind,
    UnFreeze,
    UnBlind
}
