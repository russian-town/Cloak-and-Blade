using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerMover))]
public class Player : Ghost, IPauseHandler
{
    [SerializeField] private Ability _ability;
    [SerializeField] private ItemsInHold _itemsInHold;

    private PlayerMover _mover;
    private IEnemyTurnHandler _enemyTurnHandler;
    private Cell _startCell;
    private MoveCommand _moveCommand;
    private AbilityCommand _abilityCommand;
    private SkipCommand _skipCommand;
    private Navigator _navigator;
    private AnimationClip _hourglassAnimation;
    private Animator _hourglassAnimator;
    private CanvasGroup _hourglass;
    private PlayerAnimationHandler _animationHandler;
    private Command _currentCommand;
    private PlayerView _playerView;

    public Cell CurrentCell => _mover.CurrentCell;
    public ItemsInHold ItemsInHold => _itemsInHold;

    public event UnityAction StepEnded;
    public event UnityAction Died;

    private void OnDisable()
    {
        _mover.MoveEnded -= OnMoveEnded;
    }

    public void Initialize(Cell startCell, AnimationClip hourglassAnimation, Animator hourglassAnimator, CanvasGroup hourglass, IEnemyTurnHandler enemyTurnHandler, PlayerView playerView)
    {
        _startCell = startCell;
        _mover = GetComponent<PlayerMover>();
        _navigator = GetComponent<Navigator>();
        _playerView = playerView;
        _animationHandler = GetComponent<PlayerAnimationHandler>();
        _enemyTurnHandler = enemyTurnHandler;
        _mover.Initialize(_startCell, _animationHandler);
        _ability.Initialize();
        _mover.MoveEnded += OnMoveEnded;
        _hourglass = hourglass;
        _hourglassAnimator = hourglassAnimator;
        _hourglassAnimation = hourglassAnimation;
        _moveCommand = new MoveCommand(this, _mover, _playerView, _navigator);
        _abilityCommand = new AbilityCommand(_ability);
        _skipCommand = new SkipCommand(this, _hourglassAnimator, this, _hourglass, _enemyTurnHandler.WaitForEnemies(), _animationHandler, _hourglassAnimation);
    }

    public void PrepareAbility() => SwitchCurrentCommand(_abilityCommand);

    public void PrepareMove() => SwitchCurrentCommand(_moveCommand);

    public void PrepareSkip()
    {
        SwitchCurrentCommand(_skipCommand);
        ExecuteCurrentCommand(CurrentCell);
    }

    public void SkipTurn() => OnMoveEnded();

    public bool TryMoveToCell(Cell targetCell)
    {
        if (_navigator.CanMoveToCell(targetCell) && targetCell.IsOccupied == false)
        {
            _mover.Move(targetCell);
            _startCell = _mover.CurrentCell;
            return true;
        }

        return false;
    }

    public void ExecuteCurrentCommand(Cell cell)
    {
        if (_currentCommand == null)
            return;

        if (_currentCommand.IsExecuting == false)
            StartCoroutine(_currentCommand.Execute(cell, this));
    }

    public void Die() => Died?.Invoke();

    public void SetPause(bool isPause)
    {
        _mover.SetPause(isPause);

        if (isPause == true)
        {
            _animationHandler.StopAnimation();
        }
        else
        {
            _animationHandler.StartAnimation();
        }
    }

    private void SwitchCurrentCommand(Command command)
    {
        if (command == _currentCommand || _currentCommand is SkipCommand)
            return;

        if (_currentCommand != null && _currentCommand.IsExecuting)
        {
            Debug.Log(_currentCommand);
            return;
        }

        _currentCommand?.Cancel();
        _currentCommand = command;
        StartCoroutine(_currentCommand.Prepare(this));
    }

    private void OnMoveEnded()
    {
        if (_currentCommand is not MoveCommand)
            _currentCommand = null;
        else
            StartCoroutine(_currentCommand.Prepare(this));

        StepEnded?.Invoke();
    }
}
