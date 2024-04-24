using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private WaitForSeconds _delayWaitForSeconds;
    private WaitUntil _waitUntilParticleDie;

    public event Action StepEnded;

    public event Action AbilityUsed;

    public event Action Died;

    public CommandExecuter CommandExecuter => _commandExecuter;

    public bool IsDead { get; private set; }

    public Sprite AbilityIcon => _abilityIcon;

    public Coroutine MoveCoroutine { get; private set; }

    public Cell CurrentCell => _mover.CurrentCell;

    public MoveCommand MoveCommand => _moveCommand;

    public ItemsInHold ItemsInHold => _itemsInHold;

    public PlayerMover Mover => _mover;

    protected Navigator Navigator => _navigator;

    protected Gameboard Gameboard => _gameboard;

    protected UpgradeSetter UpgradeSetter => _upgradeSetter;

    protected float RotationSpeed => _rotationSpeed;

    protected float MoveSpeed => _moveSpeed;

    protected int Range => _moveRange;

    public void Unsubscribe()
    {
        _mover.MoveEnded -= OnMoveEnded;
        _commandExecuter.AbilityUseFail -= OnAbilityUseFail;
        _commandExecuter.AbilityReseted -= OnAbilityReseted;
    }

    public virtual void Initialize(
        Cell startCell,
        Hourglass hourglass,
        IEnemyTurnWaiter enemyTurnHandler,
        Gameboard gameboard,
        RewardedAdHandler adHandler,
        PlayerView playerView,
        Battery battery)
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
        _moveCommand = new MoveCommand(
            this,
            _mover,
            _navigator,
            _moveSpeed,
            _rotationSpeed,
            _gameboard,
            _commandExecuter,
            _moveRange);
        _skipCommand = new SkipCommand(this, _enemyTurnWaiter, _animationHandler, _commandExecuter);
        _delayWaitForSeconds = new WaitForSeconds(_delay);
        _waitUntilParticleDie = new WaitUntil(() => !_diedParticle.isPlaying);
    }

    public void SetTargets(List<Enemy> enemies)
    {
        _enemies.AddRange(enemies);
        _attacker.Initialize(_enemies);
    }

    public bool TryPrepareAbility()
    {
        if (!_commandExecuter.TrySwitchCommand(GetAbilityCommand()))
            return false;

        if (GetAbilityCommand().IsExecuting)
            return false;

        if (GetAbilityCommand().IsUsed)
            return false;

        PrepareAbility();
        return true;
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

    public void SkipTurn()
    {
        UpdateAbilityState();
        StepEnded?.Invoke();
    }

    public bool TryMoveToCell(Cell targetCell, float moveSpeed, float rotationSpeed)
    {
        if (_turn == Turn.Enemy)
            return false;

        if (_navigator.CanMoveToCell()
            && targetCell.IsOccupied == false
            && targetCell.Content.Type != CellContentType.Wall)
        {
            MoveCoroutine = _mover.StartMoveTo(targetCell, moveSpeed, rotationSpeed);
            _startCell = _mover.CurrentCell;
            return true;
        }

        return false;
    }

    public void Die()
    {
        if (IsDead)
            return;

        StartCoroutine(MakeDeath());
    }

    public void SetTurn(Turn turn)
    {
        _turn = turn;
        _commandExecuter.SetTurn(_turn);
        _navigator.SetTurn(_turn);
        _moveCommand.SetTurn(_turn);
        TurnChanged(turn);
    }

    public void Unpause()
    {
        _mover.Unpause();
        _animationHandler.StartAnimation();
    }

    public void Pause()
    {
        _mover.Pause();
        _animationHandler.StopAnimation();
    }

    public abstract AbilityCommand GetAbilityCommand();

    protected virtual void TurnChanged(Turn turn)
    { }

    private IEnumerator MakeDeath()
    {
        IsDead = true;
        _commandExecuter.ResetCommand();
        _deathSoundSource.Play();

        while (_model.Enabled)
        {
            _model.Hide();
            yield return null;
        }

        _diedParticle.Play();
        yield return _waitUntilParticleDie;
        yield return _delayWaitForSeconds;
        Died?.Invoke();
    }

    private void OnAbilityUseFail()
        => _adHandler.Show();

    private void OnAbilityReseted()
        => _battery.Enable();

    private void OnMoveEnded()
    {
        UpdateAbilityState();
        StepEnded?.Invoke();
    }

    private void UpdateAbilityState()
    {
        if (GetAbilityCommand().IsUsed)
        {
            _battery.Disable();
            AbilityUsed?.Invoke();
        }
    }
}
