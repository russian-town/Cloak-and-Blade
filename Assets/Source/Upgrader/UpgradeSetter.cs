using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New upgrade setter")]
public class UpgradeSetter : ScriptableObject
{
    [SerializeField] private List<int> _prices = new List<int>(Constants.MaxLevel);
    [SerializeField] private int _level;

    public int Level => _level;
    public IReadOnlyList<int> Prices => _prices;

    public void Upgade()
    {
        _level++;
        _level = Mathf.Clamp(_level, 0, Constants.MaxLevel);
    }
}
