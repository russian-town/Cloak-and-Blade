using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneScenario : MonoBehaviour, IDataReader, IInitializable
{
    [SerializeField] private float _flyingToTableWait;
    [SerializeField] private float _candleLightWait;
    [SerializeField] private float _textAppearWait;
    [SerializeField] private float _narratorWait;
    [SerializeField] private GameObject _candle;
    [SerializeField] private AudioSource _candleSound;
    [SerializeField] private AudioSource _candleLoop;
    [SerializeField] private Light _candleLight;
    [SerializeField] private float _candleLightFadeSpeed;
    [SerializeField] private float _candleIntensity;
    [SerializeField] private LoadingScreen _loadingScreen;

    private WaitForSeconds _genericWait;
    private bool _isTutorialCompleted;

    private void Start()
    {
        _candleLight.intensity = 0;
    }

    public void Initialize()
    {
        _genericWait = new WaitForSeconds(_flyingToTableWait);
        _loadingScreen.Initialize();
        StartCoroutine(CutsceneCoroutine());
        Debug.Log(_isTutorialCompleted);
    }

    public void Read(PlayerData playerData)
    {
        _isTutorialCompleted = playerData.IsTutorialCompleted;
    }

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
        yield return _genericWait;
        print("narrator started speech");
        yield return _loadingScreen.StartFade(1);

        if (_isTutorialCompleted)
            SceneManager.LoadScene(Constants.MainMenu);
        else
            SceneManager.LoadScene(Constants.Tutorial);
    }
}
