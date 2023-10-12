using Agava.YandexGames;
using Lean.Localization;
using System.Collections;
using UnityEngine;

public class YandexInit : MonoBehaviour
{
    [SerializeField] private LeanLocalization _localization;
    [SerializeField] private Shop _shop;

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

        YandexGamesSdk.GameReady();

        if (YandexGamesSdk.Environment.i18n.lang == "en")
            _localization.SetCurrentLanguage(Constants.English);

        if (YandexGamesSdk.Environment.i18n.lang == "ru")
            _localization.SetCurrentLanguage(Constants.Russian);

        if (YandexGamesSdk.Environment.i18n.lang == "tr")
            _localization.SetCurrentLanguage(Constants.Turkish);
#endif

        _shop.Initialize();
        yield break;
    }
}
