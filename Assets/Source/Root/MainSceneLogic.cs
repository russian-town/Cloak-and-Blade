using UnityEngine;

public class MainSceneLogic : MonoBehaviour
{
    [SerializeField] private Shop _shop;
    [SerializeField] private PlayersHandler _playersHandler;
    [SerializeField] private Character[] _characters;
    [SerializeField] private UpgradeSetter[] _upgradeSetters;
    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private Audio _audio;
    [SerializeField] private AudioView _audioView;
    [SerializeField] private WalletView _walletView;

    private Saver _saver = new Saver();
    private Wallet _wallet = new Wallet();

    private void OnEnable()
    {
        _saver.Enable();
        _shop.CharacterSold += OnCharacterSold;
        _shop.CharacterSelected += OnCharacterSelected;
    }

    private void OnDisable()
    {
        _saver.Disable();
        _shop.CharacterSold -= OnCharacterSold;
        _shop.CharacterSelected -= OnCharacterSelected;
        _saver.Save();
    }

    public void Initialize()
    {
        _saver.AddInitializable(_shop);
        _saver.AddInitializable(_wallet);
        _saver.AddDataReaders(new IDataReader[] {_shop, _playersHandler, _wallet, _levelLoader, _audioView, _audio});
        _saver.AddDataReaders(_characters);
        _saver.AddDataReaders(_upgradeSetters);
        _saver.AddDataWriters(new IDataWriter[] { _shop, _playersHandler, _wallet, _audioView});
        _saver.AddDataWriters(_characters);
        _saver.AddDataWriters(_upgradeSetters);
        _saver.Initialize();
        _saver.Load();
        _walletView.Initialize(_wallet);
        _wallet.Initialize();
        _shop.SetWallet(_wallet);
    }

    private void OnCharacterSold() => _saver.Save();

    private void OnCharacterSelected() => _saver.Save();
}
