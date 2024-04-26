using System.Collections;
using Lean.Localization;
using Source.CutScene;
using Source.LevelLoader;
using Source.Saves;
using Source.Sound_and_music;
using UnityEngine;

namespace Source.Yandex
{
    public class YandexInit : MonoBehaviour, IDataWriter
    {
        [SerializeField] private LeanLocalization _localization;
        [SerializeField] private StarterScreen _starterScreen;
        [SerializeField] private Audio _audio;
        [SerializeField] private LevelsHandler _levelsHandler;
        [SerializeField] private SimpleTextTyper _simpleTextTyper;

        private Saver _saver = new ();
        private string _currentLanguague;

        private void OnEnable()
            => _saver.Enable();

        private void Awake()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        YandexGamesSdk.CallbackLogging = true;
#endif
        }

        private void OnDisable()
            => _saver.Disable();

        private IEnumerator Start()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        yield return YandexGamesSdk.Initialize();
        YandexGamesSdk.CallbackLogging = true;

        switch (YandexGamesSdk.Environment.i18n.lang)
        {
            case Constants.En:
                _localization.SetCurrentLanguage(Constants.English);
                break;
            case Constants.Ru:
                _localization.SetCurrentLanguage(Constants.Russian);
                break;
            case Constants.Tr:
                _localization.SetCurrentLanguage(Constants.Turkish);
                break;
            default:
                _localization.SetCurrentLanguage(Constants.English);
                break;
        }

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
            yield return new WaitUntil(() => _saver.DataLoaded);
            _saver.Save();
            _simpleTextTyper.Initialize();
            yield break;
        }

        public void Write(PlayerData playerData) => playerData.CurrentLanguague = _currentLanguague;
    }
}
