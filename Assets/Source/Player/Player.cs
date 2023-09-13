using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerMover))]
public class Player : MonoBehaviour
{
    [SerializeField] private Ability _ability;
    [SerializeField] private ItemsInHold _itemsInHold;

    private PlayerMover _mover;
    private Cell _startCell;
    private MoveCommand _moveCommand;
    private AbilityCommand _abilityCommand;
    private SkipCommand _skipCommand;
    private Navigator _navigator;
    private AnimationClip _hourglassAnimation;
    private Animator _hourglassAnimator;
    private CanvasGroup _hourglass;

    public Command CurrentCommand { get; private set; }
    public Cell CurrentCell => _mover.CurrentCell;
    public ItemsInHold ItemsInHold => _itemsInHold;

    public event UnityAction StepEnded;

    private void OnDisable()
    {
        _mover.MoveEnded -= OnMoveEnded;
    }

    public void Initialize(Cell startCell, AnimationClip hourglassAnimation, Animator hourglassAnimator, CanvasGroup hourglass)
    {
        _startCell = startCell;
        _mover = GetComponent<PlayerMover>();
        _navigator = GetComponent<Navigator>();
        _mover.Initialize(_startCell);
        _ability.Initialize();
        _mover.MoveEnded += OnMoveEnded;
        _hourglass = hourglass;
        _hourglassAnimator = hourglassAnimator;
        _hourglassAnimation = hourglassAnimation;
        _moveCommand = new MoveCommand(this, _mover);
        _abilityCommand = new AbilityCommand(_ability);
        _skipCommand = new SkipCommand(this, _hourglassAnimation, _hourglassAnimator, this, _hourglass);
        _navigator.RefillAvailableCells(new List<Cell> { _mover.CurrentCell.North, _mover.CurrentCell.East, _mover.CurrentCell.West, _mover.CurrentCell.South });
    }

    public void PrepareAbility() => SwitchCurrentCommand(_abilityCommand);

    public void PrepareMove() => SwitchCurrentCommand(_moveCommand);

    public void PrepareSkip() => SwitchCurrentCommand(_skipCommand);

    public void SkipTurn() => OnMoveEnded();

    public bool TryMoveToCell(Cell targetCell)
    {
        if (_navigator.CanMoveToCell(targetCell))
        {
            _mover.Move(targetCell);
            _startCell = _mover.CurrentCell;
            return true;
        }

        return false;
    }

    private void SwitchCurrentCommand(Command command)
    {
        if (command == CurrentCommand)
            return;

        if (CurrentCommand != null && CurrentCommand.IsExecuting)
            return;

        if (CurrentCommand != null)
            CurrentCommand.Cancel();

        CurrentCommand = command;
        _navigator.RefillAvailableCells(_mover.CurrentCell);
        StartCoroutine(CurrentCommand.Prepare(this));
    }

    private void OnMoveEnded()
    {
        if (CurrentCommand is not MoveCommand)
            CurrentCommand = null;

        _navigator.RefillAvailableCells(_mover.CurrentCell);
        StepEnded?.Invoke();
    }
}
