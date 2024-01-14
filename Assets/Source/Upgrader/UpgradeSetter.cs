using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New upgrade setter")]
public class UpgradeSetter : ScriptableObject, IDataWriter, IDataReader
{
    [SerializeField] private List<int> _prices = new List<int>(Constants.MaxLevel);
    [SerializeField] private int _level;

    public int Level => _level;
    public IReadOnlyList<int> Prices => _prices;

    public void Upgrade()
    {
        _level++;
        _level = Mathf.Clamp(_level, 0, Constants.MaxLevel);
    }

    public void Write(PlayerData playerData)
    {
        if (playerData.UpgradeSetters.Contains(this))
        {
            int index = playerData.UpgradeSetters.IndexOf(this);
            playerData.Levels[index] = _level;
        }
        else
        {
            playerData.UpgradeSetters.Add(this);
            playerData.Levels.Add(_level);
        }
    }

    public void Read(PlayerData playerData)
    {
        if (playerData.UpgradeSetters.Contains(this))
        {
            int index = playerData.UpgradeSetters.IndexOf(this);
            _level = playerData.Levels[index];
        }
    }
}
