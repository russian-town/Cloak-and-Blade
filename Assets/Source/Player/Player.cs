using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerMover))]
public class Player : Ghost
{
    [SerializeField] private Ability _ability;
    [SerializeField] private ItemsInHold _itemsInHold;

    private PlayerMover _mover;
    private Room _room;
    private Cell _startCell;
    private MoveCommand _moveCommand;
    private AbilityCommand _abilityCommand;
    private SkipCommand _skipCommand;
    private Navigator _navigator;
    private AnimationClip _hourglassAnimation;
    private Animator _hourglassAnimator;
    private CanvasGroup _hourglass;
    private PlayerAnimationHandler _playerAnimationHandler;
    private Command _currentCommand;
    private Gameboard _gameboard;

    public Cell CurrentCell => _mover.CurrentCell;
    public ItemsInHold ItemsInHold => _itemsInHold;

    public event UnityAction StepEnded;

    private void OnDisable()
    {
        _mover.MoveEnded -= OnMoveEnded;
    }

    public void Initialize(Cell startCell, AnimationClip hourglassAnimation, Animator hourglassAnimator, CanvasGroup hourglass, Room room, Gameboard gameboard)
    {
        _startCell = startCell;
        _mover = GetComponent<PlayerMover>();
        _navigator = GetComponent<Navigator>();
        _playerAnimationHandler = GetComponent<PlayerAnimationHandler>();
        _gameboard = gameboard;
        _room = room;
        _mover.Initialize(_startCell, _playerAnimationHandler);
        _ability.Initialize();
        _mover.MoveEnded += OnMoveEnded;
        _hourglass = hourglass;
        _hourglassAnimator = hourglassAnimator;
        _hourglassAnimation = hourglassAnimation;
        _navigator.RefillAvailableCells(new List<Cell> { _mover.CurrentCell.North, _mover.CurrentCell.East, _mover.CurrentCell.West, _mover.CurrentCell.South });
        _moveCommand = new MoveCommand(this, _mover, _gameboard);
        _abilityCommand = new AbilityCommand(_ability, _gameboard, this);
        _skipCommand = new SkipCommand(this, _hourglassAnimator, this, _hourglass, _room.WaitForEnemies, _playerAnimationHandler, _hourglassAnimation);
    }

    public void PrepareAbility()
    {
        SwitchCurrentCommand(_abilityCommand);
    }

    public void PrepareMove()
    {
        SwitchCurrentCommand(_moveCommand);
    }

    public void PrepareSkip()
    {
        SwitchCurrentCommand(_skipCommand);
        ExecuteCurrentCommand();
    }

    public void SkipTurn() => OnMoveEnded();

    public bool TryMoveToCell(Cell targetCell)
    {
        if (_room.Turn == Turn.Enemy)
            return false;

        if (_navigator.CanMoveToCell(targetCell))
        {
            _mover.Move(targetCell);
            _startCell = _mover.CurrentCell;
            return true;
        }

        return false;
    }

    public void ExecuteCurrentCommand()
    {
        if (_room.Turn == Turn.Enemy || _currentCommand == null)
            return;

        if (_currentCommand.IsExecuting == false)
            StartCoroutine(_currentCommand.Execute(this));
    }

    private void SwitchCurrentCommand(Command command)
    {
        if (_room.Turn == Turn.Enemy)
            return;

        if (command == _currentCommand || _currentCommand is SkipCommand)
            return;

        if (_currentCommand != null && _currentCommand.IsExecuting)
            return;

        _currentCommand?.Cancel();
        _currentCommand = command;
        _navigator.RefillAvailableCells(_mover.CurrentCell);
        StartCoroutine(_currentCommand.Prepare(this));
    }

    private void OnMoveEnded()
    {
        if (_currentCommand is not MoveCommand)
            _currentCommand = null;
        else
            StartCoroutine(_currentCommand.Prepare(this));


        _navigator.RefillAvailableCells(_mover.CurrentCell);
        StepEnded?.Invoke();
    }
}
