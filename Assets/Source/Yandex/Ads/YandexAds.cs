using Agava.YandexGames;
using System;

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
