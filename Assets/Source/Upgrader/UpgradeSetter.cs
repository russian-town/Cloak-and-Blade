using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New upgrade setter")]
public class UpgradeSetter : ScriptableObject, IDataWriter, IDataReader
{
    [SerializeField] private List<int> _prices = new List<int>(Constants.MaxLevel);
    [SerializeField] private int _level;

    public int Level => _level;
    public IReadOnlyList<int> Prices => _prices;

    public event Action Upgraded;

    public void Upgrade()
    {
        _level++;
        _level = Mathf.Clamp(_level, 0, Constants.MaxLevel);
        Upgraded?.Invoke();
    }

    public void Write(PlayerData playerData)
    {
        for (int i = 0; i < playerData.UpgradeSetters.Count; i++)
        {
            if (playerData.UpgradeSetters[i] == name)
            {
                playerData.UpgradeLevels[i] = _level;
                return;
            }
        }

        playerData.UpgradeSetters.Add(name);
        playerData.UpgradeLevels.Add(_level);
    }

    public void Read(PlayerData playerData)
    {
        if (playerData.UpgradeSetters.Count == 0)
            return;

        for (int i = 0; i < playerData.UpgradeSetters.Count; i++)
        {
            if (playerData.UpgradeSetters[i] == name)
            {
                _level = playerData.UpgradeLevels[i];
                return;
            }
        }
    }
}
