using DG.Tweening;
using PSXShaderKit;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneScenario : MonoBehaviour, IDataReader, IInitializable
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
    [SerializeField] private Light _candleLight;
    [SerializeField] private float _candleIntensity;
    [SerializeField] private float _candleLightFadeSpeed;
    [SerializeField] private float _musicLowVolume;
    [SerializeField] private LoadingScreen _loadingScreen;
    [SerializeField] private ProgressBarFiller _progressBar;

    private WaitForSeconds _genericWait;
    private bool _isTutorialCompleted;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _progressBar.ChangeFilling();
        }
        else if (Input.GetMouseButtonUp(0) && _progressBar.WasFilling)
        {
            _progressBar.ChangeFilling();
        }
    }

    private void OnDisable()
    {
        _progressBar.ProgressBarFilled -= OnProgressBarFilled;
    }

    public void Initialize()
    {
        _candleLight.intensity = 0;
        _genericWait = new WaitForSeconds(_flyingToTableWait);
        _loadingScreen.Initialize();
        StartCoroutine(CutsceneCoroutine());
        _progressBar.ProgressBarFilled += OnProgressBarFilled;
        Debug.Log(_isTutorialCompleted);
    }

    public void Read(PlayerData playerData)
    {
        _isTutorialCompleted = playerData.IsTutorialCompleted;
    }

    private void OnProgressBarFilled() => SceneManager.LoadScene(Constants.MainMenu);

    private IEnumerator CutsceneCoroutine()
    {
        yield return _loadingScreen.StartFade(0);
        yield return _genericWait;
        print("im at table");
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
        print("text started");
        _genericWait = new WaitForSeconds(_narratorWait);
        print("narrator started speech");
        _backgroundMusic.DOFade(_musicLowVolume, 1);
        yield return _genericWait;
        _genericWait = new WaitForSeconds(_narratorSpeechWait);
        _narratorSpeech.Play();
        yield return _genericWait;
        yield return _loadingScreen.StartFade(1);

        if (_isTutorialCompleted)
            SceneManager.LoadScene(Constants.MainMenu);
        else
            SceneManager.LoadScene(Constants.Tutorial);
    }
}
