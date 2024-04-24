using UnityEngine;
using UnityEngine.UI;

public class RewardedAdHandler : MonoBehaviour
{
    [SerializeField] private Button _doubleStarsButton;
    [SerializeField] private ScoreDefiner _scoreDefiner;

    private YandexAds _yandexAds;
    private Player _player;
    private CommandExecuter _commandExecuter;
    private Wallet _wallet;
    private AdHandler _adHandler;

    private void OnDisable()
    {
        _yandexAds.RewardedCallback -= OnRewardedVideoCallBack;
        _yandexAds.OpenCallback -= OnRewardedOpenClallback;
        _yandexAds.CloseCallback -= OnRewardedCloseClallback;
    }

    public void Initialize(
        Player player,
        YandexAds yandexAds,
        Wallet wallet,
        AdHandler adHandler)
    {
        _player = player;
        _yandexAds = yandexAds;
        _wallet = wallet;
        _adHandler = adHandler;
        _commandExecuter = _player.CommandExecuter;
        _yandexAds.RewardedCallback += OnRewardedVideoCallBack;
        _yandexAds.OpenCallback += OnRewardedOpenClallback;
        _yandexAds.CloseCallback += OnRewardedCloseClallback;
    }

    public void Show()
        => _yandexAds.ShowRewardedVideo();

    private void OnRewardedVideoCallBack()
    {
        _wallet.AddStars(_scoreDefiner.StarsCount);
        _doubleStarsButton.gameObject.SetActive(false);
        _commandExecuter.ResetAbilityOnReward();
    }

    private void OnRewardedOpenClallback()
        => _adHandler.OpenAd();

    private void OnRewardedCloseClallback()
        => _adHandler.CloseAd();
}
