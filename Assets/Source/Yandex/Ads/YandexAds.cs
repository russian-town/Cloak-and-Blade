using System;
using Agava.YandexGames;
using UnityEngine.SceneManagement;

public class YandexAds
{
    public Action OpenCallback;
    public Action RewardedCallback;
    public Action CloseCallback;
    public Action<string> ErrorCallback;
    public Action OpenInterstitialCallback;
    public Action<bool> CloseInterstitialCallback;
    public Action<string> ErrorInterstitialCallback;
    public Action OfflineCallback;

    public void ShowRewardedVideo()
    {
        if (SceneManager.GetActiveScene().name == Constants.Tutorial)
            return;

#if UNITY_WEBGL && !UNITY_EDITOR
        if (YandexGamesSdk.IsInitialized == true)
            VideoAd.Show(OpenCallback, RewardedCallback, CloseCallback, ErrorCallback);
#endif
    }

    public void ShowInterstitial()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if (YandexGamesSdk.IsInitialized == true)
            InterstitialAd.Show(OpenInterstitialCallback, CloseInterstitialCallback, ErrorInterstitialCallback, OfflineCallback);
#endif
    }
}
