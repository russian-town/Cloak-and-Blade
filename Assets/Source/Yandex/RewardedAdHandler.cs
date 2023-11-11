using UnityEngine;
using UnityEngine.UI;

public class RewardedAdHandler : MonoBehaviour
{
    [SerializeField] private Button _playAdButton;
    [SerializeField] private Button _dontPlayAdButton;

    private YandexAds _yandexAds;
    private Player _player;
    private CommandExecuter _commandExecuter;

    private void OnEnable()
    {
        _playAdButton.onClick.AddListener(OnButtonClick);
        _dontPlayAdButton.onClick.AddListener(Hide);
        _yandexAds.RewardedCallback += OnRewardedVideoCallBack;
    }

    private void OnDisable()
    {
        _playAdButton.onClick.RemoveListener(OnButtonClick);
        _dontPlayAdButton.onClick.RemoveListener(Hide);
        _yandexAds.RewardedCallback -= OnRewardedVideoCallBack;
    }

    public void Initialize(Player player, YandexAds yandexAds)
    {
        _yandexAds = yandexAds;
        _player = player;
        _commandExecuter = _player.CommandExecuter;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnButtonClick()
    {
        _yandexAds.ShowRewardedVideo();
        Hide();
    } 
    
    private void OnRewardedVideoCallBack()
    {
        Hide();
        _commandExecuter.ResetAbilityOnReward();
    }
}
