using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
public class TheWorld : Ability
{
    [SerializeField] private int _maxStepCount;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _timeStop;
    [SerializeField] private AudioClip _timeResume;
    [SerializeField] private float _effectSlowDuration;
    [SerializeField] private float _effectSpeedUpDuration;
    [SerializeField] private ParticleSystem _burstActionEffect;
    [SerializeField] private List<EffectChangeHanldler> _effectsToChange;

    private PlayerAttacker _attacker;
    private Player _player;
    private bool _isActive;
    private int _currentStepCount;

    public int CurrentStepCount => _currentStepCount;
    public int MaxStepCount => _maxStepCount;

    private void OnDisable()
    {
        _player.StepEnded -= IncreaseCurrentStepCount;
    }

    public override void Initialize()
    {
        _attacker = GetComponent<PlayerAttacker>();
        _player = GetComponent<Player>();
    }

    public override void Cancel() 
    {
        _isActive = false;
        _currentStepCount = 0;
    }

    public override void Prepare() { }

    protected override void Action(Cell cell)
    {
        if (_isActive)
            return;

        _isActive = true;
        _source.clip = _timeStop;
        _source.Play();
        _currentStepCount = 0;
        _player.StepEnded += IncreaseCurrentStepCount;
        _attacker.Attack(this);
        _burstActionEffect.Play();

        foreach (var effect in _effectsToChange)
            effect.ChangeEffectSpeed(0, _effectSlowDuration);
    }

    private void IncreaseCurrentStepCount()
    {
        _currentStepCount++;

        if (_currentStepCount >= _maxStepCount)
        {
            _isActive = false;
            _source.clip = _timeResume;
            _source.Play();
            _player.StepEnded -= IncreaseCurrentStepCount;

            foreach (var effect in _effectsToChange)
                effect.ChangeEffectSpeed(1, _effectSpeedUpDuration);
        }
    }
}
