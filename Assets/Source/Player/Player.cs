using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerMover), typeof(PlayerAttacker), typeof(CommandExecuter))]
[RequireComponent (typeof(Navigator))]
public abstract class Player : Ghost, IPauseHandler, ITurnHandler
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private ItemsInHold _itemsInHold;
    [SerializeField] private ParticleSystem _diedParticle;
    [SerializeField] private PlayerModel _model;
    [SerializeField] private Sprite _abilityIcon;
    [SerializeField] private UpgradeSetter _upgradeSetter;

    private PlayerMover _mover;
    private PlayerAttacker _attacker;
    private IEnemyTurnWaiter _enemyTurnWaiter;
    private Cell _startCell;
    private MoveCommand _moveCommand;
    private SkipCommand _skipCommand;
    private Navigator _navigator;
    private Hourglass _hourglass;
    private PlayerAnimationHandler _animationHandler;
    private List<Enemy> _enemies = new List<Enemy>();
    private Gameboard _gameboard;
    private CommandExecuter _commandExecuter;
    private Turn _turn;

    public Sprite AbilityIcon => _abilityIcon;
    public Coroutine MoveCoroutine { get; private set; }
    public Cell CurrentCell => _mover.CurrentCell;
    public ItemsInHold ItemsInHold => _itemsInHold;
    public MoveCommand Move => _moveCommand;
    protected Navigator Navigator => _navigator;
    protected Gameboard Gameboard => _gameboard;
    protected CommandExecuter CommandExecuter => _commandExecuter;
    protected UpgradeSetter UpgradeSetter => _upgradeSetter;

    public event UnityAction StepEnded;
    public event UnityAction Died;

    public void Unsubscribe() => _mover.MoveEnded -= OnMoveEnded;

    public virtual void Initialize(Cell startCell, Hourglass hourglass, IEnemyTurnWaiter enemyTurnHandler, Gameboard gameboard)
    {
        _startCell = startCell;
        _mover = GetComponent<PlayerMover>();
        _navigator = GetComponent<Navigator>();
        _attacker = GetComponent<PlayerAttacker>();
        _commandExecuter = GetComponent<CommandExecuter>();
        _animationHandler = GetComponent<PlayerAnimationHandler>();
        _enemyTurnWaiter = enemyTurnHandler;
        _mover.Initialize(_startCell, _animationHandler);
        _mover.MoveEnded += OnMoveEnded;
        _hourglass = hourglass;
        _hourglass.Initialaze(_commandExecuter);
        _gameboard = gameboard;
        _moveCommand = new MoveCommand(this, _mover, _navigator, _moveSpeed, _rotationSpeed, _gameboard, _commandExecuter);
        _skipCommand = new SkipCommand(this, _enemyTurnWaiter.WaitForEnemies(), _animationHandler, _hourglass, _commandExecuter);
    }

    public void SetTargets(List<Enemy> enemies)
    {
        _enemies.AddRange(enemies);
        _attacker.Initialize(_enemies);
    }

    public void PrepareAbility()
    {
        if (AbilityCommand().IsUsed)
            return;

        _commandExecuter.PrepareCommand(AbilityCommand());
    }

    public void PrepareMove() => _commandExecuter.PrepareCommand(_moveCommand);

    public void PrepareSkip() => _commandExecuter.PrepareCommand(_skipCommand);

    public void SkipTurn() => StepEnded?.Invoke();

    public bool TryMoveToCell(Cell targetCell, float moveSpeed, float rotationSpeed)
    {
        if (_turn == Turn.Enemy)
            return false;

        if (_navigator.CanMoveToCell(ref targetCell) && targetCell.IsOccupied == false && targetCell.Content.Type != CellContentType.Wall)
        {
            _mover.StartMoveTo(targetCell, moveSpeed, rotationSpeed);
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
        _commandExecuter?.SetTurn(_turn);
    }

    protected abstract AbilityCommand AbilityCommand();

    private IEnumerator MakeDeath()
    {
        _commandExecuter.ResetCommand();
        _model.Hide();
        _diedParticle.Play();
        yield return new WaitUntil(() => !_diedParticle.isPlaying);
        Died?.Invoke();
    }

    private void OnMoveEnded()
    {
        _commandExecuter.UpdateLastCommand();
        StepEnded?.Invoke();
    }
}
