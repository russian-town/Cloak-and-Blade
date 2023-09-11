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
    private List<Cell> _availableCells = new List<Cell>();

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
        _mover.Initialize(_startCell);
        _ability.Initialize(_mover);
        _mover.MoveEnded += OnMoveEnded;
        _moveCommand = new MoveCommand(this);
        _abilityCommand = new AbilityCommand(_ability);
        AddAvailableCells();
    }

    public void PrepareAbility() => SwitchCurrentCommand(_abilityCommand);

    public void PrepareMove() => SwitchCurrentCommand(_moveCommand);

    public void SkipTurn() => OnMoveEnded();

    public void TryMoveToCell(Cell targetCell)
    {
        if (_availableCells.Contains(targetCell))
            _mover.Move(targetCell);

        _startCell = targetCell;
        AddAvailableCells();
    }

    private void AddAvailableCells()
    {
        if (_availableCells.Count > 0)
            _availableCells.Clear();

        _availableCells.Add(_startCell.East);
        _availableCells.Add(_startCell.North);
        _availableCells.Add(_startCell.South);
        _availableCells.Add(_startCell.West);
    }

    private void SwitchCurrentCommand(Command command)
    {
        if (command == CurrentCommand)
            return;

        if (CurrentCommand != null)
            CurrentCommand.Cancel();

        CurrentCommand = command;
        CurrentCommand.Prepare();
    }

    private void OnMoveEnded()
    {
        CurrentCommand = null;
        StepEnded?.Invoke();
    }
}
