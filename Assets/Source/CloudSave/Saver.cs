using System.Collections.Generic;
using UnityEngine;

public class Saver : MonoBehaviour
{
    private List<IDataReader> _dataReaders = new List<IDataReader>();
    private List<IDataWriter> _dataWriters = new List<IDataWriter>();

    private CloudSave _cloudSave = new CloudSave();
    private PlayerData _playerData = new PlayerData();

    private void OnEnable()
    {
        _cloudSave.DataLoaded += OnDataLoaded;
        _cloudSave.ErrorCallback += OnErrorCallback;
    }

    private void OnDisable()
    {
        _cloudSave.DataLoaded -= OnDataLoaded;
        _cloudSave.ErrorCallback -= OnErrorCallback;
    }

    public void AddDataReaders(IDataReader[] dataReader) => _dataReaders.AddRange(dataReader);

    public void AddDataWriters(IDataWriter[] dataWriter) => _dataWriters.AddRange(dataWriter);

    public void Save()
    {
        var playerData = new PlayerData();

        foreach (var writer in _dataWriters)
            writer.Write(playerData);

        _cloudSave.Save(playerData);
    }

    public void Load()
    {
        if (_cloudSave.TryLoadCloudSaves())
            return;

        LoadLocalSaves();
    }

    private void OnDataLoaded(string data)
    {
        PlayerData playerData = JsonUtility.FromJson<PlayerData>(data);

        if (playerData == null)
            return;

        _playerData = playerData;

        foreach (var reader in _dataReaders)
            reader.Read(_playerData);
    }

    private void OnErrorCallback(string error) => LoadLocalSaves();

    private void LoadLocalSaves()
    {
        _playerData = _cloudSave.LoadLocalSaves();

        if (_playerData == null)
            return;

        foreach (var reader in _dataReaders)
            reader.Read(_playerData);
    }
}
