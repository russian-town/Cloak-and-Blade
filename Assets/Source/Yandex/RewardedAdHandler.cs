using UnityEngine;
using UnityEngine.UI;

public class RewardedAdHandler : MonoBehaviour
{
    [SerializeField] private Button _playAdButton;
    [SerializeField] private Button _dontPlayAdButton;

    private YandexAds _yandexAds;
    private Player _player;

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
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnButtonClick()
    {
        /*_yandexAds.ShowRewardedVideo();*/
        _player.ResetAbilityOnReward();
        Hide();
    } 
    
    private void OnRewardedVideoCallBack()
    {
        _player.ResetAbilityOnReward();
        Hide();
    }
}
