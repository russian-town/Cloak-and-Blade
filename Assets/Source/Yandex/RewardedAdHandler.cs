using UnityEngine;

public class RewardedAdHandler : MonoBehaviour
{
    private YandexAds _yandexAds;
    private Player _player;
    private CommandExecuter _commandExecuter;

    private void OnDisable() => _yandexAds.RewardedCallback -= OnRewardedVideoCallBack;

    public void Initialize(Player player, YandexAds yandexAds)
    {
        _yandexAds = yandexAds;
        _player = player;
        _commandExecuter = _player.CommandExecuter;
        _yandexAds.RewardedCallback += OnRewardedVideoCallBack;
    }

    public void Show() => _yandexAds.ShowRewardedVideo();
    
    private void OnRewardedVideoCallBack() => _commandExecuter.ResetAbilityOnReward();
}
