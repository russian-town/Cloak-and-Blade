using Agava.YandexGames;
using Lean.Localization;
using System.Collections;
using UnityEngine;

public class YandexInit : MonoBehaviour
{
    [SerializeField] private LeanLocalization _localization;
    [SerializeField] private MainSceneLogic _mainSceneLogic;
    [SerializeField] private float _speed;
    [SerializeField] private LoadingScreen _loadingScreen;

    private void Awake()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        YandexGamesSdk.CallbackLogging = true;
#endif

        _loadingScreen.Initialize();
    }

    private IEnumerator Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        yield return YandexGamesSdk.Initialize();

        if (YandexGamesSdk.Environment.i18n.lang == "en")
            _localization.SetCurrentLanguage(Constants.English);

        if (YandexGamesSdk.Environment.i18n.lang == "ru")
            _localization.SetCurrentLanguage(Constants.Russian);

        if (YandexGamesSdk.Environment.i18n.lang == "tr")
            _localization.SetCurrentLanguage(Constants.Turkish);
#endif

        StartCoroutine(_mainSceneLogic.Initialize());

#if UNITY_WEBGL && !UNITY_EDITOR
        YandexGamesSdk.GameReady();
#endif

        _loadingScreen.StartFade();
        yield break;
    }
}
