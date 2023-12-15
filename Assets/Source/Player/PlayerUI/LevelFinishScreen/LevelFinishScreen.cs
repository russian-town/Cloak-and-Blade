using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelFinishScreen : MonoBehaviour
{
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _doubleStarsForAdButton;
    [SerializeField] private ScreenAnimationHandler _animationHandler;

    private YandexAds _yandexAds;

    public event UnityAction ExitButtonClicked;

    private void OnEnable()
    {
        _exitButton.onClick.AddListener(() => ExitButtonClicked?.Invoke());
        _doubleStarsForAdButton.onClick.AddListener(OnRewardedButtonClick);
    } 

    private void OnDisable()
    {
        _exitButton.onClick.RemoveListener(() => ExitButtonClicked?.Invoke());
        _doubleStarsForAdButton.onClick.RemoveListener(OnRewardedButtonClick);
    } 

    public void Initialize(YandexAds yandexAds)
    {
        _yandexAds = yandexAds;
        gameObject.SetActive(true);
    }

    public void Unsubscribe()
    {
        _doubleStarsForAdButton.onClick.RemoveListener(OnRewardedButtonClick);
    }

    public void Show()
    {
        _animationHandler.FadeIn();
    }

    public void Hide() 
    {
        _animationHandler.FadeOut();
    } 

    private void OnRewardedButtonClick()
    {
        _yandexAds.ShowRewardedVideo();
    }
}
