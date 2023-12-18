using EntroPi;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
public class TheWorld : Ability, ISceneParticlesInfluencer
{
    [SerializeField] private int _maxStepCount;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _timeStop;
    [SerializeField] private AudioClip _timeResume;
    [SerializeField] private float _effectSlowDuration;
    [SerializeField] private float _effectSpeedUpDuration;
    [SerializeField] private ParticleSystem _burstActionEffect;
    [SerializeField] private List<EffectChangeHanldler> _effectsList = new List<EffectChangeHanldler>();
    [SerializeField] private List<SoundChangeHandler> _soundList = new List<SoundChangeHandler>();
    [SerializeField] private List<SplineChangeHandler> _splineList = new List<SplineChangeHandler>();
    [SerializeField] private List<AnimationChangeHandler> _animationList = new List<AnimationChangeHandler>();
    [SerializeField] private Sprite _icon;

    private PlayerAttacker _attacker;
    private Player _player;
    private bool _isActive;
    private int _currentStepCount;
    private bool _canUse = true;
    private UpgradeSetter _upgradeSetter;
    private PlayerView _playerView;
    private CloudShadows _cloudShadows;

    private void OnDisable()
    {
        if (_player == null)
            return;

        _player.StepEnded -= OnStepEnded;
    }

    public void AddSceneEffectsToChange(List<EffectChangeHanldler> effects, List<SoundChangeHandler> sounds, List<SplineChangeHandler> splines, List<AnimationChangeHandler> animations)
    {
            _effectsList.AddRange(effects);
            _soundList.AddRange(sounds);
            _splineList.AddRange(splines);
            _animationList.AddRange(animations);
    }

    public override void Initialize(UpgradeSetter upgradeSetter, PlayerView playerView)
    {
        _attacker = GetComponent<PlayerAttacker>();
        _player = GetComponent<Player>();
        _upgradeSetter = upgradeSetter;
        _playerView = playerView;
        _maxStepCount += _upgradeSetter.Level;
    }

    public override void Cancel() { }

    public override void Prepare() => _playerView.DisableAbilityButton();

    public override bool CanUse()
    {
        return _canUse;
    }

    public override void ResetAbility()
    {
        if (_canUse == true)
            return;

        _canUse = true;
    }

    protected override void Action(Cell cell)
    {
        if (_isActive)
            return;

        _canUse = false;
        _isActive = true;
        _source.clip = _timeStop;
        _source.Play();
        _currentStepCount = 0;
        _player.StepEnded += OnStepEnded;
        _attacker.Attack(AttackType.Freeze);
        _burstActionEffect.Play();

        foreach (var effect in _effectsList)
            effect.ChangeEffectSpeed(0, _effectSlowDuration);

        foreach (var sound in _soundList)
            sound.ChangeAudioPitch(0, _effectSlowDuration);

        foreach (var spline in _splineList)
            spline.ChangeSpeed(0, _effectSlowDuration);

        foreach (var animation in _animationList)
            animation.ChangeSpeed(0, _effectSlowDuration);
    }

    private void OnStepEnded() => IncreaseCurrentStepCount();

    private void IncreaseCurrentStepCount()
    {
        _currentStepCount++;

        if (_currentStepCount >= _maxStepCount)
        {
            _isActive = false;
            _source.clip = _timeResume;
            _source.Play();
            _attacker.Attack(AttackType.UnFreeze);
            _player.StepEnded -= OnStepEnded;
            _playerView.Cancel();
            _playerView.EnableAbilityButton();

            foreach (var effect in _effectsList)
                effect.ChangeEffectSpeed(effect.InitialValue, _effectSpeedUpDuration);

            foreach (var sound in _soundList)
                sound.ChangeAudioPitch(sound.InitialPitch, _effectSpeedUpDuration);

            foreach (var spline in _splineList)
                spline.ChangeSpeed(spline.InitialSpeed, _effectSpeedUpDuration);

            foreach (var animation in _animationList)
                animation.ChangeSpeed(animation.InitialSpeed, _effectSlowDuration);
        }
    }
}
