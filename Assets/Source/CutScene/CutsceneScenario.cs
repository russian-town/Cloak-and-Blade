using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class CutsceneScenario : MonoBehaviour, IPauseHandler
{
    [SerializeField] private float _flyingToTableWait;
    [SerializeField] private float _candleLightWait;
    [SerializeField] private float _textAppearWait;
    [SerializeField] private float _narratorWait;
    [SerializeField] private float _narratorSpeechWait;
    [SerializeField] private GameObject _candle;
    [SerializeField] private AudioSource _candleSound;
    [SerializeField] private AudioSource _candleLoop;
    [SerializeField] private AudioSource _narratorSpeech;
    [SerializeField] private AudioSource _backgroundMusic;
    [SerializeField] private AudioSource _rainMusic;
    [SerializeField] private Light _candleLight;
    [SerializeField] private float _candleIntensity;
    [SerializeField] private float _candleLightFadeSpeed;
    [SerializeField] private float _musicLowVolume;
    [SerializeField] private LoadingScreen _loadingScreen;
    [SerializeField] private ProgressBarFiller _progressBar;
    [SerializeField] private SimpleTextTyper _textTyper;
    [SerializeField] private CanvasGroup _letter;
    [SerializeField] private LevelsHandler _levelsHandler;
    [SerializeField] private SplineAnimate _cameraSpline;
    [SerializeField] private ThunderMaker _thunderMaker;

    private WaitForSeconds _genericWait;
    private bool _isInitialized;
    private Coroutine _cutSceneCoroutine;

    private void Update()
    {
        if (_isInitialized == false)
            return;

        if (Input.GetMouseButtonDown(0))
            _progressBar.ChangeFilling();
        else if (Input.GetMouseButtonUp(0) && _progressBar.WasFilling)
            _progressBar.ChangeFilling();
    }

    private void OnDisable()
    {
        _progressBar.ProgressBarFilled -= OnProgressBarFilled;
    }

    public void SetPause(bool isPause)
    {
        if (isPause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void StartCutscene()
    {
        if (_cutSceneCoroutine != null)
            StopCoroutine(_cutSceneCoroutine);

        _cutSceneCoroutine = StartCoroutine(CutsceneCoroutine());
    }

    private void OnProgressBarFilled() => _levelsHandler.TryLoadTutorial();

    private IEnumerator CutsceneCoroutine()
    {
        _cameraSpline.Play();
        _backgroundMusic.Play();
        _rainMusic.Play();
        _thunderMaker.enabled = true;
        _candleLight.intensity = 0;
        _genericWait = new WaitForSeconds(_flyingToTableWait);
        _progressBar.Initialize();
        _progressBar.ProgressBarFilled += OnProgressBarFilled;
        _isInitialized = true;
        yield return _loadingScreen.StartFade(0);
        yield return _genericWait;
        _genericWait = new WaitForSeconds(_candleLightWait);
        yield return _genericWait;
        _candle.SetActive(true);
        _candleSound.Play();

        while (_candleLight.intensity != _candleIntensity)
        {
            _candleLight.intensity = Mathf.MoveTowards(_candleLight.intensity, _candleIntensity, _candleLightFadeSpeed * Time.deltaTime);
            yield return null;
        }

        _genericWait = new WaitForSeconds(_textAppearWait);
        yield return _genericWait;
        _candleLoop.Play();
        _genericWait = new WaitForSeconds(_narratorWait);
        _backgroundMusic.DOFade(_musicLowVolume, 1);
        _textTyper.TypeText();
        yield return _genericWait;
        _genericWait = new WaitForSeconds(_narratorSpeechWait);
        _narratorSpeech.Play();
        yield return _genericWait;
        _genericWait = new WaitForSeconds(_textAppearWait);
        yield return _genericWait;
        _letter.DOFade(0, 1f).SetEase(Ease.OutSine);
        yield return _genericWait;
        yield return _loadingScreen.StartFade(1);
        _cutSceneCoroutine = null;
        _levelsHandler.TryLoadTutorial();
    }
}
