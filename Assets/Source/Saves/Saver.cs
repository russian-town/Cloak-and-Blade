using System.Collections.Generic;

namespace Source.Saves
{
    public class Saver : IDataReader
    {
        private List<IDataReader> _dataReaders = new List<IDataReader>();
        private List<IDataWriter> _dataWriters = new List<IDataWriter>();
        private List<IInitializable> _initializables = new List<IInitializable>();
        private ISaveLoadService _currentSaveLoadService;
        private CloudSave _cloudSave = new CloudSave();
        private LocalSave _localSave = new LocalSave();
        private PlayerData _playerData = new PlayerData();

        public bool DataLoaded { get; private set; }

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

            _dataReaders.Add(this);
            _currentSaveLoadService.AddDataWriters(_dataWriters.ToArray());
            _currentSaveLoadService.AddDataReaders(_dataReaders.ToArray());
        }

        public void AddDataReaders(IDataReader[] dataReader) => _dataReaders.AddRange(dataReader);

        public void AddDataWriters(IDataWriter[] dataWriter) => _dataWriters.AddRange(dataWriter);

        public void AddInitializable(IInitializable initializable) => _initializables.Add(initializable);

        public void Save()
        {
            _currentSaveLoadService.Save(_playerData);
        }

        public void Load()
        {
            _currentSaveLoadService.Load();

            if (_currentSaveLoadService is LocalSave)
                OnDataLoaded(null);
        }

        public void Read(PlayerData playerData) => _playerData = playerData;

        private void OnDataLoaded(string data)
        {
            if (_initializables.Count == 0)
                return;

            foreach (var initializable in _initializables)
                initializable.Initialize();

            DataLoaded = true;
        }

        private void OnErrorLoadCallback(string error)
        {
            _localSave.Load();
        }

        private void OnErrorSaveCallBack(string error)
        {
            _localSave.Save(_playerData);
        }
    }
}
