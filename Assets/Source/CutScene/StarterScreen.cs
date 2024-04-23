using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup), typeof(AudioSource))]
public class StarterScreen : MonoBehaviour, IInitializable
{
    [SerializeField] private Button _pressAnyWhere;
    [SerializeField] private CanvasGroup _startText;
    [SerializeField] private CutsceneScenario _cutsceneScenario;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private AudioClip _startClickClip;
    [SerializeField] private LoadingScreen _loadingScreen;
    [SerializeField] private float _titleFadeDuration;
    [SerializeField] private RectTransform _titleTransform;
    [SerializeField] private float _pressAnywhereFadeDuration;
    [SerializeField] private AudioSource _startScreenAmbient;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private float _minParticleStrengthX;
    [SerializeField] private float _maxParticleStrengthX;
    [SerializeField] private float _minParticleStrengthY;
    [SerializeField] private float _maxParticleStrengthY;

    private AudioSource _audioSource;
    private CanvasGroup _canvasGroup;

    public void Initialize()
    {
        _loadingScreen.Initialize();
        _canvasGroup = GetComponent<CanvasGroup>();
        _audioSource = GetComponent<AudioSource>();
        _pressAnyWhere.enabled = true;
        var particleNoise = _particle.noise;
        particleNoise.strengthX = Random.Range(_minParticleStrengthX, _maxParticleStrengthX);
        particleNoise.strengthY = Random.Range(_minParticleStrengthY, _maxParticleStrengthY);
        StartCoroutine(StartScreenFade());
    }

    private void OnButtonClick()
    {
        _particle.Stop();
        _startScreenAmbient.DOFade(0, _pressAnywhereFadeDuration);
        _startScreenAmbient.Stop();
        _loadingScreen.SetFade(1);
        _pressAnyWhere.onClick.RemoveListener(OnButtonClick);
        _audioSource.PlayOneShot(_startClickClip);
        _canvasGroup.alpha = 0f;
        _cutsceneScenario.StartCutscene();
    }

    private IEnumerator StartScreenFade()
    {
        yield return _loadingScreen.StartFade(0);
        _startScreenAmbient.Play();
        _particle.Play();
        _title.DOFade(1, _titleFadeDuration).SetEase(Ease.InOutSine);
        _titleTransform.DOScale(1, _titleFadeDuration).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(_titleFadeDuration);
        _startText.DOFade(1, _pressAnywhereFadeDuration);
        yield return new WaitForSeconds(_pressAnywhereFadeDuration);
        _startText.DOFade(.7f, _pressAnywhereFadeDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
        _pressAnyWhere.onClick.AddListener(OnButtonClick);
    }
}
