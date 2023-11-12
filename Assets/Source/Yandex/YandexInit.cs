using Agava.YandexGames;
using Lean.Localization;
using System.Collections;
using UnityEngine;

public class YandexInit : MonoBehaviour
{
    [SerializeField] private LeanLocalization _localization;
    [SerializeField] private MainSceneLogic _mainSceneLogic;
    [SerializeField] private CanvasGroup _loadingScreen;
    [SerializeField] private float _speed;

    private void Awake()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        YandexGamesSdk.CallbackLogging = true;
#endif
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

        _mainSceneLogic.Initialize();

#if UNITY_WEBGL && !UNITY_EDITOR
        YandexGamesSdk.GameReady();
#endif

        StartCoroutine(Fade());
        yield break;
    }

    private IEnumerator Fade() 
    {
        while(_loadingScreen.alpha > 0)
        {
            _loadingScreen.alpha = Mathf.MoveTowards(_loadingScreen.alpha, 0, Time.deltaTime * _speed);
            yield return null;
        }

        _loadingScreen.blocksRaycasts = false;
    }
}
