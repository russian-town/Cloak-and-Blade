using Agava.YandexGames;
using System.Collections.Generic;
using UnityEngine;

public class Saver
{
    private List<IDataReader> _dataReaders = new List<IDataReader>();
    private List<IDataWriter> _dataWriters = new List<IDataWriter>();
    private List<IInitializable> _initializables = new List<IInitializable>();
    private ISaveLoadService _currentSaveLoadService;
    private CloudSave _cloudSave = new CloudSave();
    private LocalSave _localSave = new LocalSave();

    public void Enable()
    {
        _cloudSave.DataLoaded += OnDataLoaded;
        _cloudSave.ErrorLoadCallback += OnErrorLoadCallback;
        _cloudSave.ErrorSaveCallback += OnErrorSaveCallBack;
    }

    public void Disable()
    {
        _cloudSave.DataLoaded -= OnDataLoaded;
        _cloudSave.ErrorLoadCallback -= OnErrorLoadCallback;
        _cloudSave.ErrorSaveCallback -= OnErrorSaveCallBack;
    }

    public void Initialize()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if (PlayerAccount.IsAuthorized)
            _currentSaveLoadService = _cloudSave;
        else
            _currentSaveLoadService = _localSave;
#else
        _currentSaveLoadService = _localSave;
#endif

        _currentSaveLoadService.AddDataWriters(_dataWriters.ToArray());
        _currentSaveLoadService.AddDataReaders(_dataReaders.ToArray());
    }

    public void AddDataReaders(IDataReader[] dataReader) => _dataReaders.AddRange(dataReader);

    public void AddDataWriters(IDataWriter[] dataWriter) => _dataWriters.AddRange(dataWriter);

    public void AddInitializable(IInitializable initializable) => _initializables.Add(initializable);

    public void Save()
    {
        PlayerData playerData = new PlayerData();
        _currentSaveLoadService.Save(playerData);
    }

    public void Load()
    {
        _currentSaveLoadService.Load();

        if (_currentSaveLoadService is LocalSave)
            OnDataLoaded(null);
    }

    private void OnDataLoaded(string data)
    {
        if (_initializables.Count == 0)
            return;

        foreach (var initializable in _initializables)
        {
            initializable.Initialize();
        }
    }

    private void OnErrorLoadCallback(string error)
    {
        Debug.Log(error);
        _localSave.Load();
    }

    private void OnErrorSaveCallBack(string error)
    {
        PlayerData playerData = new PlayerData();
        _localSave.Save(playerData);
    }
}
