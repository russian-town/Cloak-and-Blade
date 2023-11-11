using System.Collections.Generic;
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
    [SerializeField] private List<EffectChangeHanldler> _effectsToChange = new();
    [SerializeField] private Sprite _icon;

    private PlayerAttacker _attacker;
    private Player _player;
    private bool _isActive;
    private int _currentStepCount;
    private bool _canUse = true;
    private UpgradeSetter _upgradeSetter;

    private void OnDisable()
    {
        if (_player == null)
            return;

        _player.StepEnded -= OnStepEnded;
    }

    public void AddSceneParticles(List<EffectChangeHanldler> effects)
    {
        if (effects.Count == 0)
            return;

        _effectsToChange.AddRange(effects);
    }

    public override void Initialize(UpgradeSetter upgradeSetter)
    {
        _attacker = GetComponent<PlayerAttacker>();
        _player = GetComponent<Player>();
        _upgradeSetter = upgradeSetter;
        _maxStepCount += _upgradeSetter.Level;
    }

    public override void Cancel() { }

    public override void Prepare() { }

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

        foreach (var effect in _effectsToChange)
            effect.ChangeEffectSpeed(0, _effectSlowDuration);
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

            foreach (var effect in _effectsToChange)
                effect.ChangeEffectSpeed(1, _effectSpeedUpDuration);
        }
    }
}
