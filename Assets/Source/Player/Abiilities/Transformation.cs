using System;
using UnityEngine;

[RequireComponent(typeof(PlayerAttacker))]
public class Transformation : Ability
{
    [SerializeField] private int _useLimit = 1;
    [SerializeField] private PlayerModel _model;
    [SerializeField] private AudioSource _whisperSound;
    [SerializeField] private AudioSource _laughterSound;
    [SerializeField] private AudioSource _castSound;
    [SerializeField] private AudioSource _switchBackSound;

    private PlayerAttacker _attacker;
    private Player _player;
    private Cell _currentCell;
    private int _maxUseLimit;
    private UpgradeSetter _upgradeSetter;
    private PlayerView _playerView;

    public bool Prepared { get; private set; }

    public override void Initialize(UpgradeSetter upgradeSetter, PlayerView playerView)
    {
        _attacker = GetComponent<PlayerAttacker>();
        _player = GetComponent<Player>();
        _upgradeSetter = upgradeSetter;
        _playerView = playerView;
        _useLimit += _upgradeSetter.Level;
        _maxUseLimit = _useLimit;
    }

    public override void Prepare()
    {
        if (Prepared == true)
            return;

        _playerView.DisableAbilityButton();
        _model.TransformToDecoy();
        _whisperSound.Play();
        _laughterSound.Play();
        _castSound.Play();
        _attacker.Attack(AttackType.Blind);
        _currentCell = _player.CurrentCell;
        _currentCell.Content.BecomeWall();
        _player.SkipTurn();
        _player.StepEnded += OnStepEnded;
        _playerView.HideMoveButton();
        Prepared = true;
    }

    public override void Cancel()
    {
        _playerView.Cancel();
        _playerView.EnableAbilityButton();
        _model.SwitchBack();
        _whisperSound.Stop();
        _laughterSound.Stop();
        _castSound.Stop();
        _switchBackSound.Play();
        _currentCell.Content.BecomeEmpty();
        _attacker.Attack(AttackType.UnBlind);
        _player.StepEnded -= OnStepEnded;
        Prepared = false;
    }

    public override bool CanUse()
    {
        return _useLimit > 0;
    }

    public override void ResetAbility() => _useLimit++;

    protected override void Action(Cell cell)
    {
        _useLimit--;
        _useLimit = Mathf.Clamp(_useLimit, 0, _maxUseLimit);
    }

    private void OnStepEnded() => Action(_currentCell);
}
