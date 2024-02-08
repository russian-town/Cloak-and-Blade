using System.Collections;
using UnityEngine;

public class MainSceneLogic : MonoBehaviour, IDataReader
{
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

    private Saver _saver = new Saver();
    private Wallet _wallet = new Wallet();
    private string _currentLanguage;

    private void OnEnable()
    {
        _saver.Enable();
        _shop.CharacterSold += OnCharacterSold;
        _shop.CharacterSelected += OnCharacterSelected;
    }

    private void Start()
    {
        StartCoroutine(Initialize());
    }

    private void OnDestroy()
    {
        _saver.Save();
    }

    private void OnDisable()
    {
        _saver.Disable();
        _shop.CharacterSold -= OnCharacterSold;
        _shop.CharacterSelected -= OnCharacterSelected;
    }

    public IEnumerator Initialize()
    {
        _saver.AddInitializable(_shop);
        _saver.AddInitializable(_wallet);
        _saver.AddDataReaders(new IDataReader[] {_shop, _playersHandler, _wallet, _levelLoader, _audioView, _audio});
        _saver.AddDataReaders(_characters);
        _saver.AddDataReaders(_upgradeSetters);
        _saver.AddDataWriters(new IDataWriter[] { _shop, _playersHandler, _wallet, _levelLoader, _audioView });
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
            if (YandexGamesSdk.Environment.i18n.lang == "en")
                _localization.SetCurrentLanguage(Constants.English);

            if (YandexGamesSdk.Environment.i18n.lang == "ru")
                _localization.SetCurrentLanguage(Constants.Russian);

            if (YandexGamesSdk.Environment.i18n.lang == "tr")
                _localization.SetCurrentLanguage(Constants.Turkish);
        }
#endif

        _loadingScreen.Initialize();
        _walletView.Initialize(_wallet);
        _wallet.Initialize();
        _shop.SetWallet(_wallet);
        _levelLoader.Initialize();
        _loadingScreen.StartFade(0);
        _mainMenu.Initialize();
        yield return null;
    }

    private void OnCharacterSold() => _saver.Save();

    private void OnCharacterSelected() => _saver.Save();

    public void Read(PlayerData playerData) => _currentLanguage = playerData.CurrentLanguague;
}
