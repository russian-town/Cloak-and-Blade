using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Agava.YandexGames;
using Agava.WebUtility;
using System;

public class CloudSave : ISaveLoadService
{
    private List<IDataWriter> _dataWriters = new List<IDataWriter>();
    private List<IDataReader> _dataReaders = new List<IDataReader>();

    public event Action<string> DataLoaded;
    public event Action<string> ErrorLoadCallback;
    public event Action<string> ErrorSaveCallback;

    public void AddDataWriters(IDataWriter[] dataWriters) => _dataWriters.AddRange(dataWriters);

    public void AddDataReaders(IDataReader[] dataReaders) => _dataReaders.AddRange(dataReaders);

    public void Save(PlayerData data)
    {
        if (data == null)
            return;

        foreach (var writer in _dataWriters)
            writer.Write(data);

        string saveData = JsonUtility.ToJson(data);

        Debug.Log(saveData);

#if UNITY_WEBGL && !UNITY_EDITOR
        if (PlayerAccount.IsAuthorized)
            PlayerAccount.SetCloudSaveData(saveData, null, ErrorSaveCallback);
#endif
    }

    public void Load()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if (YandexGamesSdk.IsInitialized == false)
            return;

        if (PlayerAccount.IsAuthorized)
            PlayerAccount.GetCloudSaveData(OnDataLoaded, OnErrorLoad);
#endif
    }

    public void OnDataLoaded(string data)
    {
        PlayerData playerData = JsonUtility.FromJson<PlayerData>(data);

        if (playerData == null)
        {
            Debug.Log("PlayerData is null.");
            return;
        }

        foreach (var reader in _dataReaders)
            reader.Read(playerData);

        DataLoaded?.Invoke(data);
    }

    public void OnErrorLoad(string error) => ErrorLoadCallback?.Invoke(error);
}
