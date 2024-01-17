using Agava.YandexGames;
using Lean.Localization;
using System.Collections;
using UnityEngine;

public class YandexInit : MonoBehaviour
{
    [SerializeField] private LeanLocalization _localization;
    [SerializeField] private CutsceneScenario _cutsceneScenario;
    
    private Saver _saver = new Saver();

    private void OnEnable()
    {
        _saver.Enable();
    }

    private void Awake()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        YandexGamesSdk.CallbackLogging = true;
#endif
    }

    private void OnDisable()
    {
        _saver.Disable();
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
         
#if UNITY_WEBGL && !UNITY_EDITOR
        YandexGamesSdk.GameReady();
#endif

        _saver.AddDataReaders(new IDataReader[] { _cutsceneScenario });
        _saver.AddInitializable(_cutsceneScenario);
        _saver.Initialize();
        _saver.Load();
        yield break;
    }
}
