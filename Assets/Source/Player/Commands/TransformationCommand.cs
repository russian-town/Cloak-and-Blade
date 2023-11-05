using System.Collections;
using UnityEngine;

public class TransformationCommand : AbilityCommand, ITurnHandler
{
    private readonly int _range;
    private readonly float _moveSpeed;
    private readonly float _rotationSpeed;
    private readonly PlayerMover _playerMover;
    private readonly Transformation _transformation;
    private readonly Gameboard _gameboard;
    private readonly Camera _camera;
    private readonly Navigator _navigator;
    private readonly Player _player;
    private readonly CommandExecuter _executer;

    private Cell _cell;

    public TransformationCommand(Transformation transformation, Gameboard gameboard, Navigator navigator, CommandExecuter executer, Player player, PlayerMover playerMover, float moveSpeed, float rotationSpeed, int range) : base(transformation, executer)
    {
        _transformation = transformation;
        _gameboard = gameboard;
        _camera = Camera.main;
        _navigator = navigator;
        _player = player;
        _playerMover = playerMover;
        _range = range;
        _moveSpeed = moveSpeed;
        _rotationSpeed = rotationSpeed;
        _executer = executer;
    }

    public void SetTurn(Turn turn)
    {
        if (Enabled == false)
            return;

        if(turn == Turn.Enemy)
        {
            _navigator.HideAvailableCells();
        }
        else
        {
            _navigator.RefillAvailableCellsIgnoredWalls(_player.CurrentCell, _range);
            _navigator.ShowAvailableCells();
            _executer.StartCoroutine(WaitOfExecute());
        }
    }

    protected override void Cancel()
    {
        base.Cancel();
        _playerMover.MoveEnded -= Cancel;
        _transformation.Cancel();
        _navigator.HideAvailableCells();
        Debug.Log("Cancel");
    }

    protected override IEnumerator WaitOfExecute()
    {
        WaitOfClickedCell waitOfClickedCell = new WaitOfClickedCell(_gameboard, _camera, _navigator);
        yield return waitOfClickedCell;
        _cell = waitOfClickedCell.Cell;
    }

    protected override IEnumerator ExecuteAction()
    {
        _playerMover.MoveEnded += Cancel;
        yield return new WaitUntil(() => _transformation.Cast(_cell));

        if (_player.TryMoveToCell(_cell, _moveSpeed, _rotationSpeed))
            yield return _player.MoveCoroutine;
    }

    protected override IEnumerator PrepareAction()
    {
        _navigator.RefillAvailableCellsIgnoredWalls(_playerMover.CurrentCell, _range);
        _navigator.ShowAvailableCells();
        _transformation.Prepare();
        yield return null;
    }

    protected override void OnCommandChanged(Command command)
    {
        if (command is SkipCommand || command == this)
            return;

        Cancel();
    }
}
