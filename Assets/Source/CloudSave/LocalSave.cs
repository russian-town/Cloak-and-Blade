using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalSave : ISaveLoadService
{
    private List<IDataReader> _dataReaders = new List<IDataReader>();
    private List<IDataWriter> _dataWriters = new List<IDataWriter>();

    public void AddDataWriters(IDataWriter[] dataWriters) => _dataWriters.AddRange(dataWriters);

    public void AddDataReaders(IDataReader[] dataReaders) => _dataReaders.AddRange(dataReaders);

    public void Save(PlayerData playerData)
    {
        if (playerData == null)
            return;

        foreach (var writer in _dataWriters)
        {
            writer.Write(playerData);
        }

        string saveData = JsonUtility.ToJson(playerData);
        PlayerPrefs.SetString(Constants.PlayerProgress, saveData);
        PlayerPrefs.Save();
    }

    public PlayerData Load()
    {
        if (PlayerPrefs.HasKey(Constants.PlayerProgress) == false)
            return null;

        string data = PlayerPrefs.GetString(Constants.PlayerProgress);

        if (string.IsNullOrEmpty(data))
            return null;

        PlayerData playerData = JsonUtility.FromJson<PlayerData>(data);

        if (playerData != null)
        {
            foreach (var dataReader in _dataReaders)
            {
                dataReader.Read(playerData);
            }
        }

        return playerData;
    }
}
