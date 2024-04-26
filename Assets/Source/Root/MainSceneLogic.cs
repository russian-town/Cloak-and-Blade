using System.Collections;
using Agava.YandexGames;
using Lean.Localization;
using UnityEngine;

public class MainSceneLogic : MonoBehaviour, IDataReader
{
    private readonly Saver _saver = new ();
    private readonly Wallet _wallet = new ();

    [SerializeField] private Shop _shop;
    [SerializeField] private PlayersHandler _playersHandler;
    [SerializeField] private Character[] _characters;
    [SerializeField] private UpgradeSetter[] _upgradeSetters;
    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private Audio _audio;
    [SerializeField] private AudioView _audioView;
    [SerializeField] private WalletView _walletView;
    [SerializeField] private LoadingScreen _loadingScreen;
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private LeanLocalization _localization;
    [SerializeField] private LevelsHandler _levelsHandler;

    private string _currentLanguage;

    private void OnEnable()
    {
        _saver.Enable();
        _shop.CharacterSold += OnCharacterSold;
        _shop.CharacterSelected += OnCharacterSelected;
        _audio.AudioValueChanged += OnAudioValueChanged;

        foreach (var setter in _upgradeSetters)
            setter.Upgraded += OnUpgrade;
    }

    private void Start()
    {
        StartCoroutine(Initialize());
    }

    private void OnDisable()
    {
        _saver.Disable();
        _shop.CharacterSold -= OnCharacterSold;
        _shop.CharacterSelected -= OnCharacterSelected;
        _audio.AudioValueChanged -= OnAudioValueChanged;

        foreach (var setter in _upgradeSetters)
            setter.Upgraded -= OnUpgrade;
    }

    public IEnumerator Initialize()
    {
        _saver.AddInitializable(_shop);
        _saver.AddInitializable(_wallet);
        _saver.AddDataReaders(new IDataReader[] { _playersHandler, _wallet, _levelLoader, _audioView, _audio });
        _saver.AddDataReaders(_characters);
        _saver.AddDataReaders(_upgradeSetters);
        _saver.AddDataWriters(new IDataWriter[] { _playersHandler, _wallet, _audioView });
        _saver.AddDataWriters(_characters);
        _saver.AddDataWriters(_upgradeSetters);
        _saver.Initialize();
        _saver.Load();
        yield return new WaitUntil(() => _saver.DataLoaded);

#if UNITY_WEBGL && !UNITY_EDITOR
        if (string.IsNullOrEmpty(_currentLanguage) == false)
        {
            _localization.SetCurrentLanguage(_currentLanguage);
        } 
        else
        {
            switch (YandexGamesSdk.Environment.i18n.lang)
            {
                case Constants.En:
                    _localization.SetCurrentLanguage(Constants.English);
                    break;
                case Constants.Ru:
                    _localization.SetCurrentLanguage(Constants.Russian);
                    break;
                case Constants.Tr:
                    _localization.SetCurrentLanguage(Constants.Turkish);
                    break;
                default:
                    _localization.SetCurrentLanguage(Constants.English);
                    break;
            }
        }
#endif

        _loadingScreen.Initialize();
        _walletView.Initialize(_wallet);
        _wallet.Initialize();
        _shop.SetWallet(_wallet);
        _levelLoader.Initialize(_levelsHandler);
        _loadingScreen.StartFade(0);
        _mainMenu.Initialize();
        yield return null;
    }

    public void Read(PlayerData playerData) => _currentLanguage = playerData.CurrentLanguague;

    private void OnCharacterSold() => _saver.Save();

    private void OnCharacterSelected() => _saver.Save();

    private void OnUpgrade() => _saver.Save();

    private void OnAudioValueChanged() => _saver.Save();
}
