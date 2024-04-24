using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelFinishScreen : MonoBehaviour
{
    [SerializeField] private Button _exitButton;
    [SerializeField] private Button _doubleStarsForAdButton;
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private ScreenAnimationHandler _animationHandler;

    private YandexAds _yandexAds;
    private ILevelFinisher _levelFinisher;

    public event Action ExitButtonClicked;

    public event Action NextLevelButtonClicked;

    private void OnEnable()
    {
        _exitButton.onClick.AddListener(() => ExitButtonClicked?.Invoke());
        _doubleStarsForAdButton.onClick.AddListener(OnRewardedButtonClick);
        _nextLevelButton.onClick.AddListener(() =>  NextLevelButtonClicked?.Invoke());
        _yandexAds.RewardedCallback += Unsubscribe;
    }

    private void OnDisable()
    {
        _exitButton.onClick.RemoveListener(() => ExitButtonClicked?.Invoke());
        _doubleStarsForAdButton.onClick.RemoveListener(OnRewardedButtonClick);
        _nextLevelButton.onClick.RemoveListener(() => NextLevelButtonClicked?.Invoke());
        _yandexAds.RewardedCallback -= Unsubscribe;
    }

    private void OnDestroy()
        => _levelFinisher.LevelPassed -= OnLevelPassed;

    public void Initialize(YandexAds yandexAds, ILevelFinisher levelFinisher)
    {
        _yandexAds = yandexAds;
        gameObject.SetActive(true);
        _levelFinisher = levelFinisher;
        _levelFinisher.LevelPassed += OnLevelPassed;
        Hide();
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

    private void OnLevelPassed()
    {
        Show();
    }
}
