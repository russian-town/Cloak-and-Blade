using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerMover))]
public class Player : MonoBehaviour
{
    [SerializeField] private Ability _ability; 

    private PlayerMover _mover;
    private Cell _startCell;
    private MoveCommand _moveCommand;
    private AbilityCommand _abilityCommand;
    private SkipCommand _skipCommand;
    private Navigator _navigator;

    public Command CurrentCommand { get; private set; }
    public Cell CurrentCell => _mover.CurrentCell;

    public event UnityAction StepEnded;

    private void OnDisable()
    {
        _mover.MoveEnded -= OnMoveEnded;
    }

    public void Initialize(Cell startCell)
    {
        _startCell = startCell;
        _mover = GetComponent<PlayerMover>();
        _navigator = GetComponent<Navigator>();
        _mover.Initialize(_startCell);
        _ability.Initialize();
        _mover.MoveEnded += OnMoveEnded;
        _moveCommand = new MoveCommand(this, _mover);
        _abilityCommand = new AbilityCommand(_ability);
        _skipCommand = new SkipCommand(this);
        _navigator.RefillAvailableCells(new List<Cell> { _mover.CurrentCell.North, _mover.CurrentCell.East, _mover.CurrentCell.West, _mover.CurrentCell.South });
    }

    public void PrepareAbility() => SwitchCurrentCommand(_abilityCommand);

    public void PrepareMove() => SwitchCurrentCommand(_moveCommand);

    public void PrepareSkip() => SwitchCurrentCommand(_skipCommand);

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

    public void OnMoveEnded()
    {
        if (CurrentCommand is not MoveCommand)
            CurrentCommand = null;
        else
            SwitchCurrentCommand(_moveCommand);

        _navigator.RefillAvailableCells(_mover.CurrentCell);
        StepEnded?.Invoke();
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
        CurrentCommand.Prepare();
        Debug.Log(CurrentCommand);
    }
}
