using Agava.YandexGames;
using Lean.Localization;
using System.Collections;
using UnityEngine;

public class YandexInit : MonoBehaviour, IDataWriter
{
    [SerializeField] private LeanLocalization _localization;
    [SerializeField] private StarterScreen _starterScreen;
    [SerializeField] private Audio _audio;
    [SerializeField] private LevelsHandler _levelsHandler;
    
    private Saver _saver = new Saver();
    private string _currentLanguague;

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
            _currentLanguague = Constants.English;

        if (YandexGamesSdk.Environment.i18n.lang == "ru")
            _currentLanguague = Constants.Russian;

        if (YandexGamesSdk.Environment.i18n.lang == "tr")
            _currentLanguague = Constants.Turkish;

        _localization.SetCurrentLanguage(_currentLanguague);
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        YandexGamesSdk.GameReady();
#endif

        _saver.AddDataReaders(new IDataReader[] { _levelsHandler, _audio });
        _saver.AddDataWriters(new IDataWriter[] { this });
        _saver.AddInitializable(_starterScreen);
        _saver.Initialize();
        _saver.Load();
        _saver.Save();
        yield break;
    }

    public void Write(PlayerData playerData) => playerData.CurrentLanguague = _currentLanguague;
}
