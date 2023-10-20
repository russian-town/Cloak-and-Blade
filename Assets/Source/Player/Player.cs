using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerMover), typeof(PlayerAttacker))]
public abstract class Player : Ghost, IPauseHandler
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private ItemsInHold _itemsInHold;
    [SerializeField] private ParticleSystem _diedParticle;
    [SerializeField] private PlayerModel _model;
    [SerializeField] private Sprite _abilityIcon;

    private PlayerMover _mover;
    private PlayerAttacker _attacker;
    private IEnemyTurnHandler _enemyTurnHandler;
    private Cell _startCell;
    private MoveCommand _moveCommand;
    private SkipCommand _skipCommand;
    private Navigator _navigator;
    private Hourglass _hourglass;
    private PlayerAnimationHandler _animationHandler;
    private Command _currentCommand;
    private PlayerView _playerView;
    private List<Enemy> _enemies = new List<Enemy>();
    private Gameboard _gameboard;
    private PlayerInput _input;
    private Coroutine _prepare;
    private Coroutine _waitOfExecute;
    private Coroutine _switchCommand;
    private Command _deferredCommand;

    public Sprite AbilityIcon => _abilityIcon;
    public Coroutine MoveCoroutine { get; private set; }
    public Cell CurrentCell => _mover.CurrentCell;
    public ItemsInHold ItemsInHold => _itemsInHold;
    public MoveCommand Move => _moveCommand;
    public Command NextCommand { get; private set; }
    protected PlayerInput Input => _input;
    protected Navigator Navigator => _navigator;
    protected Gameboard Gameboard => _gameboard;
    protected PlayerView PlayerView => _playerView;
    protected PlayerMover Mover => _mover;

    public event UnityAction StepEnded;
    public event UnityAction Died;

    public void Unsubscribe() => _mover.MoveEnded -= OnMoveEnded;

    public void Initialize(Cell startCell, Hourglass hourglass, IEnemyTurnHandler enemyTurnHandler, PlayerView playerView, Gameboard gameboard, PlayerInput input)
    {
        _startCell = startCell;
        _mover = GetComponent<PlayerMover>();
        _navigator = GetComponent<Navigator>();
        _attacker = GetComponent<PlayerAttacker>();
        _playerView = playerView;
        _animationHandler = GetComponent<PlayerAnimationHandler>();
        _enemyTurnHandler = enemyTurnHandler;
        _mover.Initialize(_startCell, _animationHandler);
        _mover.MoveEnded += OnMoveEnded;
        _hourglass = hourglass;
        _gameboard = gameboard;
        _input = input;
        _moveCommand = new MoveCommand(this, _mover, _playerView, _navigator, _moveSpeed, _rotationSpeed, _input, _gameboard);
        _skipCommand = new SkipCommand(this, _enemyTurnHandler.WaitForEnemies(), _animationHandler, _hourglass, this);
    }

    public void SetTargets(List<Enemy> enemies)
    {
        _enemies.AddRange(enemies);
        _attacker.Initialize(_enemies);
    }

    public virtual void PrepareAbility()
    {
        if (_currentCommand != null && _currentCommand.IsExecuting)
            return;

        if (_switchCommand != null)
        {
            StopCoroutine(_switchCommand);
            _switchCommand = null;
        }

        _switchCommand = StartCoroutine(SwitchCurrentCommand(AbilityCommand()));
    }

    public void PrepareMove()
    {
        if (_currentCommand != null && _currentCommand.IsExecuting)
            return;

        if (_switchCommand != null)
        {
            StopCoroutine(_switchCommand);
            _switchCommand = null;
        }

        _switchCommand = StartCoroutine(SwitchCurrentCommand(_moveCommand));
    }

    public void PrepareSkip()
    {
        if (_currentCommand != null && _currentCommand.IsExecuting)
            return;

        if (_switchCommand != null)
        {
            StopCoroutine(_switchCommand);
            _switchCommand = null;
        }

        _switchCommand = StartCoroutine(SwitchCurrentCommand(_skipCommand));
    }

    public void SkipTurn() => StepEnded?.Invoke();

    public bool TryMoveToCell(Cell targetCell, float moveSpeed, float rotationSpeed)
    {
        if (_navigator.CanMoveToCell(targetCell) && targetCell.IsOccupied == false && targetCell.Content.Type != CellContentType.Wall)
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

    public bool ResetCommand()
    {
        if (_currentCommand is not IUnmissable && _deferredCommand == null)
        {
            _currentCommand = null;
            return true;
        }

        return false;
    }

    public void ResetDeferredCommand() => _deferredCommand = null;

    protected abstract Command AbilityCommand();

    private IEnumerator MakeDeath()
    {
        _currentCommand = null;
        _model.Hide();
        _diedParticle.Play();
        yield return new WaitUntil(() => !_diedParticle.isPlaying);
        Died?.Invoke();
    }

    private IEnumerator SwitchCurrentCommand(Command command)
    {
        if (command == _currentCommand)
            yield break;

        if (_currentCommand != null && _currentCommand.IsExecuting)
            yield break;

        if (_currentCommand is IDeferredCommand)
            _deferredCommand = _currentCommand;

        if (_prepare != null)
        {
            StopCoroutine(_prepare);
            _prepare = null;
        }

        if(_waitOfExecute != null)
        {
            StopCoroutine(_waitOfExecute);
            _waitOfExecute = null;
        }

        NextCommand = command;
        _currentCommand?.Cancel(this);
        _currentCommand = command;

        _prepare = StartCoroutine(_currentCommand.Prepare(this));
        yield return _prepare;

        _waitOfExecute = StartCoroutine(_currentCommand.WaitOfExecute());
        yield return _waitOfExecute;

        if (_deferredCommand != null)
        {
            _switchCommand = StartCoroutine(SwitchCurrentCommand(_deferredCommand));
            _deferredCommand = null;
        }
    }

    private void OnMoveEnded()
    {
        _deferredCommand = null;
        Debug.Log(_deferredCommand);

        if (!ResetCommand())
        {
            _prepare = StartCoroutine(_currentCommand.Prepare(this));
            _waitOfExecute = StartCoroutine(_currentCommand.WaitOfExecute());
        }

        StepEnded?.Invoke();
    }
}
