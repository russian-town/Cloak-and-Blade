using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FinaleCutsceneScenario : MonoBehaviour
{
    [SerializeField] private float _calmShotDuration;
    [SerializeField] private float _swordFallingDuration;
    [SerializeField] private float _swordStabingDuration;
    [SerializeField] private float _screamDuration;
    [SerializeField] private float _effectFadeUpSpeed;
    [SerializeField] private float _cameraFrequencyFadeSpeed;
    [SerializeField] private float _cameraAmplitudeFadeSpeed;
    [SerializeField] private float _cameraFrequencyFinalIntensity;
    [SerializeField] private float _cameraAmplitudeFinalIntensity;
    [SerializeField] private float _whiteScreenFadeDuration;
    [SerializeField] private AudioSource _goreSource;
    [SerializeField] private AudioSource _screamSource;
    [SerializeField] private WhooshScript _whoosh;
    [SerializeField] private EffectChangeHandler _effectChanger;
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] private Transform _sword;
    [SerializeField] private Animator _swordAnimator;
    [SerializeField] private Image _whiteScreen;

    private CinemachineBasicMultiChannelPerlin _noise;
    private WaitForSeconds _genericWait;

    public void Start()
    {
        _genericWait = new WaitForSeconds(_calmShotDuration);
        _noise = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void PlayFinalCutscene()
    {
        StartCoroutine(CutsceneCoroutine());
    }

    private IEnumerator CutsceneCoroutine()
    {
        _camera.Priority = 3;
        yield return (_genericWait);
        _camera.LookAt = _sword.transform;
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
        _genericWait = new WaitForSeconds(_screamDuration);
        yield return _genericWait;
        _whiteScreen.DOFade(1, _whiteScreenFadeDuration).SetEase(Ease.InSine);
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
