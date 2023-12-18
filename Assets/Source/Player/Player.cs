using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerMover), typeof(PlayerAttacker), typeof(CommandExecuter))]
[RequireComponent (typeof(Navigator))]
public abstract class Player : Ghost, IPauseHandler, ITurnHandler
{
    [SerializeField][Range(1, 5)] private int _moveRange = 2;
    [SerializeField] private float _delay;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private ItemsInHold _itemsInHold;
    [SerializeField] private ParticleSystem _diedParticle;
    [SerializeField] private PlayerModel _model;
    [SerializeField] private Sprite _abilityIcon;
    [SerializeField] private UpgradeSetter _upgradeSetter;
    [SerializeField] private AudioSource _deathSoundSource;

    private RewardedAdHandler _adHandler;
    private PlayerMover _mover;
    private PlayerAttacker _attacker;
    private IEnemyTurnWaiter _enemyTurnWaiter;
    private Cell _startCell;
    private MoveCommand _moveCommand;
    private SkipCommand _skipCommand;
    private Navigator _navigator;
    private PlayerAnimationHandler _animationHandler;
    private List<Enemy> _enemies = new List<Enemy>();
    private Gameboard _gameboard;
    private CommandExecuter _commandExecuter;
    private Turn _turn;
    private Battery _battery;

    public CommandExecuter CommandExecuter => _commandExecuter;
    public bool IsDied { get; private set; }
    public Sprite AbilityIcon => _abilityIcon;
    public Coroutine MoveCoroutine { get; private set; }
    public Cell CurrentCell => _mover.CurrentCell;
    public MoveCommand MoveCommand => _moveCommand;
    public ItemsInHold ItemsInHold => _itemsInHold;
    protected Navigator Navigator => _navigator;
    protected Gameboard Gameboard => _gameboard;
    protected UpgradeSetter UpgradeSetter => _upgradeSetter;
    protected PlayerMover Mover => _mover;
    protected float RotationSpeed => _rotationSpeed;
    protected float MoveSpeed => _moveSpeed;
    protected int Range => _moveRange;

    public event UnityAction StepEnded;
    public event UnityAction AbilityUsed;
    public event UnityAction Died;

    public void Unsubscribe()
    {
        _mover.MoveEnded -= OnMoveEnded;
        _commandExecuter.AbilityUseFail -= OnAbilityUseFail;
        _commandExecuter.AbilityReseted -= OnAbilityReseted;
    }

    public virtual void Initialize(Cell startCell, Hourglass hourglass, IEnemyTurnWaiter enemyTurnHandler, Gameboard gameboard, RewardedAdHandler adHandler, PlayerView playerView, Battery battery)
    {
        _startCell = startCell;
        _mover = GetComponent<PlayerMover>();
        _navigator = GetComponent<Navigator>();
        _attacker = GetComponent<PlayerAttacker>();
        _commandExecuter = GetComponent<CommandExecuter>();
        _animationHandler = GetComponent<PlayerAnimationHandler>();
        _navigator.Initialize(this);
        _enemyTurnWaiter = enemyTurnHandler;
        _mover.Initialize(_startCell, _animationHandler);
        _mover.MoveEnded += OnMoveEnded;
        _commandExecuter.AbilityUseFail += OnAbilityUseFail;
        _commandExecuter.AbilityReseted += OnAbilityReseted;
        _gameboard = gameboard;
        _adHandler = adHandler;
        _battery = battery;
        _moveCommand = new MoveCommand(this, _mover, _navigator, _moveSpeed, _rotationSpeed, _gameboard, _commandExecuter, _moveRange);
        _skipCommand = new SkipCommand(this, _enemyTurnWaiter.WaitForEnemies(), _animationHandler, _commandExecuter);
    }

    public void SetTargets(List<Enemy> enemies)
    {
        _enemies.AddRange(enemies);
        _attacker.Initialize(_enemies);
    }

    public void TryPrepareAbility()
    {
        if (!_commandExecuter.TrySwitchCommand(AbilityCommand()))
            return;

        if (AbilityCommand().IsExecuting)
            return;

        if (AbilityCommand().IsUsed)
            return;

        PrepareAbility();
    }

    public void PrepareAbility()
    {
        _commandExecuter.PrepareCommand();
    }

    public void PrepareMove()
    {
        if (_commandExecuter.TrySwitchCommand(_moveCommand))
            _commandExecuter.PrepareCommand();
    }

    public void PrepareSkip()
    {
        if (_commandExecuter.TrySwitchCommand(_skipCommand))
            _commandExecuter.PrepareCommand();
    }

    public void SkipTurn() => StepEnded?.Invoke();

    public bool TryMoveToCell(Cell targetCell, float moveSpeed, float rotationSpeed)
    {
        if (_turn == Turn.Enemy)
            return false;

        if (_navigator.CanMoveToCell(ref targetCell) && targetCell.IsOccupied == false && targetCell.Content.Type != CellContentType.Wall)
        {
            MoveCoroutine = _mover.StartMoveTo(targetCell, moveSpeed, rotationSpeed);
            _startCell = _mover.CurrentCell;
            return true;
        }

        return false;
    }

    public void Die() => StartCoroutine(MakeDeath());

    public void SetPause(bool isPause)
    {
        _mover.SetPause(isPause);

        if (isPause == true)
            _animationHandler.StopAnimation();
        else
            _animationHandler.StartAnimation();
    }

    public void SetTurn(Turn turn)
    {
        _turn = turn;
        _commandExecuter.SetTurn(_turn);
        _navigator.SetTurn(_turn);
        _moveCommand.SetTurn(_turn);
        TurnChanged(turn);
    }

    protected virtual void TurnChanged(Turn turn) { }

    protected abstract AbilityCommand AbilityCommand();

    private IEnumerator MakeDeath()
    {
        IsDied = true;
        _commandExecuter.ResetCommand();
        _deathSoundSource.Play();

        while (_model.Enabled)
        {
            _model.Hide();
            yield return null;
        }

        _diedParticle.Play();
        yield return new WaitUntil(() => !_diedParticle.isPlaying);
        yield return new WaitForSeconds(_delay);
        Died?.Invoke();
    }

    private void OnAbilityUseFail() => _adHandler.Show();

    private void OnAbilityReseted() => _battery.Enable();

    private void OnMoveEnded()
    {
        UpdateAbilityState();
        StepEnded?.Invoke();
    }

    private void UpdateAbilityState()
    {
        if (AbilityCommand().IsUsed)
        {
            _battery.Disable();
            AbilityUsed?.Invoke();
        }
    }
}
