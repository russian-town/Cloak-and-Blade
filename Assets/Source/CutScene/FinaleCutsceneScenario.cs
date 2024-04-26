using System.Collections;
using Cinemachine;
using DG.Tweening;
using Source.Player.Abiilities.SceneEffectChangers;
using Source.Root;
using Source.Sound_and_music;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Source.CutScene
{
    public class FinaleCutsceneScenario : MonoBehaviour
    {
        private readonly float _hudFadeRation = .5f;
        private readonly float _whiteNoiseFadeStep = .2f;
        private readonly int _staticCameraPriority = 3;

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
        [SerializeField] private AudioSource _gore;
        [SerializeField] private AudioSource _scream;
        [SerializeField] private AudioSource _music;
        [SerializeField] private AudioSource _whiteNoise;
        [SerializeField] private AudioSource _heartBeat;
        [SerializeField] private AudioSource _leftWhoosh;
        [SerializeField] private AudioSource _rifghtWhoosh;
        [SerializeField] private WhooshScript _whoosh;
        [SerializeField] private EffectChangeHandler _effectChanger;
        [SerializeField] private ParticleSystem _effect;
        [SerializeField] private CinemachineVirtualCamera _staticCamera;
        [SerializeField] private CinemachineVirtualCamera _swordCamera;
        [SerializeField] private Transform _sword;
        [SerializeField] private Animator _swordAnimator;
        [SerializeField] private Image _whiteScreen;
        [SerializeField] private TMP_Text _finaleCredits;
        [SerializeField] private CanvasGroup _hud;
        [SerializeField] private ParticleSystem[] _mistParticles;
        [SerializeField] private Gameboard.Gameboard _gameboard;

        private CinemachineBasicMultiChannelPerlin _cameraShakeNoise;
        private WaitForSeconds _genericWait;

        public void Start()
        {
            _genericWait = new WaitForSeconds(_calmShotDuration);
            _cameraShakeNoise = _swordCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        public void PlayFinalCutscene()
            => StartCoroutine(CutsceneCoroutine());

        private IEnumerator CutsceneCoroutine()
        {
            _gameboard.Disable();

            foreach (var particle in _mistParticles)
                particle.Stop();

            _hud.DOFade(0, _hudFadeRation);
            _staticCamera.Priority = _staticCameraPriority;
            yield return _genericWait;
            _swordCamera.Priority = _staticCamera.Priority + 1;
            yield return _genericWait;
            _swordAnimator.SetBool(Constants.FallParameter, true);
            _genericWait = new WaitForSeconds(_swordFallingDuration);
            yield return _genericWait;
            _effect.Play();
            _gore.Play();
            _whoosh.StartWhooshCoroutine();
            _effectChanger.ChangeEffectSpeed(1, _effectFadeUpSpeed);
            StartCoroutine(ShakeCameraWithFade(_cameraFrequencyFinalIntensity, _cameraAmplitudeFinalIntensity));
            _genericWait = new WaitForSeconds(_swordStabingDuration);
            yield return _genericWait;
            _scream.Play();
            _whiteNoise.Play();
            _music.DOFade(0, _soundFadeDuration);
            _heartBeat.DOFade(0, _soundFadeDuration);
            _leftWhoosh.DOFade(0, _soundFadeDuration);
            _rifghtWhoosh.DOFade(0, _soundFadeDuration);
            _whiteNoise.DOFade(_whiteNoiseFadeStep, _soundFadeDuration).SetEase(Ease.InSine);
            _genericWait = new WaitForSeconds(_screamDuration);
            yield return _genericWait;
            _whiteScreen.DOFade(1, _whiteScreenFadeDuration).SetEase(Ease.InSine);
            _genericWait = new WaitForSeconds(_whiteScreenFadeDuration);
            yield return _genericWait;
            _finaleCredits.DOFade(1, _whiteScreenFadeDuration).SetEase(Ease.OutSine);
            _genericWait = new WaitForSeconds(_soundFadeDuration);
            _whiteNoise.DOFade(0, _cameraAmplitudeFadeSpeed);
            yield return _genericWait;
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(Constants.MainMenu);
        }

        private IEnumerator ShakeCameraWithFade(float frequencyTarget, float amplitudeTarget)
        {
            while (_cameraShakeNoise.m_FrequencyGain != frequencyTarget)
            {
                _cameraShakeNoise.m_FrequencyGain = Mathf.MoveTowards(
                    _cameraShakeNoise.m_FrequencyGain,
                    frequencyTarget,
                    _cameraFrequencyFadeSpeed * Time.deltaTime);
                _cameraShakeNoise.m_AmplitudeGain = Mathf.MoveTowards(
                    _cameraShakeNoise.m_AmplitudeGain,
                    amplitudeTarget,
                    _cameraAmplitudeFadeSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }
}
