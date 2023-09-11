using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerMover), typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    [SerializeField] private Ability _ability; 

    private PlayerMover _mover;
    private PlayerInput _input;
    private Gameboard _gameboard;
    private Cell _startCell;
    private ParticleSystem _mouseOverCell;
    private MoveCommand _moveCommand;
    private AbilityCommand _abilityCommand;

    public Command CurrentCommand { get; private set; }
    public Cell CurrentCell => _mover.CurrentCell;

    public PlayerInput Input => _input;

    public event UnityAction StepEnded;

    private void OnDisable()
    {
        _mover.MoveEnded -= OnMoveEnded;
    }

    public void Initialize(Gameboard gameboard, Cell startCell, ParticleSystem mouseOverCell)
    {
        _mouseOverCell = mouseOverCell;
        _gameboard = gameboard;
        _startCell = startCell;
        _mover = GetComponent<PlayerMover>();
        _input = GetComponent<PlayerInput>();
        _mover.Initialize(_startCell);
        _input.Initialize(_gameboard, _mover, _mouseOverCell, this);
        _ability.Initialize(_mover, _input);
        _mover.MoveEnded += OnMoveEnded;
        _moveCommand = new MoveCommand();
        _abilityCommand = new AbilityCommand();
        _abilityCommand.Initialize(_ability);
    }

    public void PrepareAbility() => SwitchCurrentCommand(_abilityCommand);

    public void PrepareMove() => SwitchCurrentCommand(_moveCommand);

    public void SkipTurn() => OnMoveEnded();

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
        StepEnded?.Invoke();
    }
}
