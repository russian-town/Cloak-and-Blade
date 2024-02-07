using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class FinaleCutsceneScenario : MonoBehaviour
{
    [SerializeField] private float _calmShotDuration;
    [SerializeField] private float _swordFallingDuration;
    [SerializeField] private float _swordStabingDuration;
    [SerializeField] private float _screamDuration;
    [SerializeField] private float _whiteScreenFadeDuration;
    [SerializeField] private float _whitescreenBlankDuration;
    [SerializeField] private float _effectFadeUpSpeed;
    [SerializeField] private float _cameraFrequencyFadeSpeed;
    [SerializeField] private float _cameraAmplitudeFadeSpeed;
    [SerializeField] private float _soundFadeDuration;
    [SerializeField] private float _cameraFrequencyFinalIntensity;
    [SerializeField] private float _cameraAmplitudeFinalIntensity;
    [SerializeField] private AudioSource _goreSource;
    [SerializeField] private AudioSource _screamSource;
    [SerializeField] private AudioSource _music;
    [SerializeField] private AudioSource _whiteNoise;
    [SerializeField] private AudioSource _heartBeat;
    [SerializeField] private AudioSource _whooshSource1;
    [SerializeField] private AudioSource _whooshSource2;
    [SerializeField] private WhooshScript _whoosh;
    [SerializeField] private EffectChangeHandler _effectChanger;
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private CinemachineVirtualCamera _staticCamera;
    [SerializeField] private CinemachineVirtualCamera _swordCamera;
    [SerializeField] private Transform _sword;
    [SerializeField] private Animator _swordAnimator;
    [SerializeField] private Image _whiteScreen;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private CanvasGroup _hud;

    private CinemachineBasicMultiChannelPerlin _noise;
    private WaitForSeconds _genericWait;

    public void Start()
    {
        _genericWait = new WaitForSeconds(_calmShotDuration);
        _noise = _swordCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void PlayFinalCutscene()
    {
        StartCoroutine(CutsceneCoroutine());
    }

    private IEnumerator CutsceneCoroutine()
    {
        _hud.DOFade(0, .5f);
        _staticCamera.Priority = 3;
        yield return (_genericWait);
        _swordCamera.Priority = _staticCamera.Priority + 1;
        yield return (_genericWait);
        _swordAnimator.SetBool(Constants.FallParameter, true);
        _genericWait = new WaitForSeconds(_swordFallingDuration);
        yield return _genericWait;
        _effect.Play();
        _goreSource.Play();
        _whoosh.PlayWhoosh();
        _effectChanger.ChangeEffectSpeed(1, _effectFadeUpSpeed);
        StartCoroutine(ShakeCameraWithFade(_cameraFrequencyFinalIntensity, _cameraAmplitudeFinalIntensity));
        _genericWait = new WaitForSeconds(_swordStabingDuration);
        yield return _genericWait;
        _screamSource.Play();
        _whiteNoise.Play();
        _music.DOFade(0, _soundFadeDuration);
        _heartBeat.DOFade(0, _soundFadeDuration);
        _whooshSource1.DOFade(0, _soundFadeDuration);
        _whooshSource2.DOFade(0, _soundFadeDuration);
        _whiteNoise.DOFade(.2f, _soundFadeDuration).SetEase(Ease.InSine);
        _genericWait = new WaitForSeconds(_screamDuration);
        yield return _genericWait;
        _whiteScreen.DOFade(1, _whiteScreenFadeDuration).SetEase(Ease.InSine);
        _genericWait = new WaitForSeconds(_whiteScreenFadeDuration);
        yield return _genericWait;
        _text.DOFade(1, _whiteScreenFadeDuration).SetEase(Ease.OutSine);
        _genericWait = new WaitForSeconds(_soundFadeDuration);
        yield return _genericWait;
        _whiteNoise.DOFade(0, _soundFadeDuration);
        yield return null;  
    }

    private IEnumerator ShakeCameraWithFade(float frequencyTarget, float amplitudeTarget)
    {
        while(_noise.m_FrequencyGain != frequencyTarget)
        {
            _noise.m_FrequencyGain = Mathf.MoveTowards(_noise.m_FrequencyGain, frequencyTarget, _cameraFrequencyFadeSpeed * Time.deltaTime);
            _noise.m_AmplitudeGain = Mathf.MoveTowards(_noise.m_AmplitudeGain, amplitudeTarget, _cameraAmplitudeFadeSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
