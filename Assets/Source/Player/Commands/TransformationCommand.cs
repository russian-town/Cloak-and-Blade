using System.Collections;
using UnityEngine;

public class TransformationCommand : AbilityCommand
{
    private readonly int _range;
    private readonly float _moveSpeed;
    private readonly float _rotationSpeed;
    private readonly PlayerMover _playerMover;

    private Transformation _transformation;
    private Gameboard _gameboard;
    private Camera _camera;
    private Navigator _navigator;
    private CommandExecuter _executer;
    private Player _player;
    private Cell _cell;

    public TransformationCommand(Transformation transformation, Gameboard gameboard, Navigator navigator, CommandExecuter executer, Player player, PlayerMover playerMover, float moveSpeed, float rotationSpeed, int range) : base(transformation, executer)
    {
        _transformation = transformation;
        _gameboard = gameboard;
        _camera = Camera.main;
        _navigator = navigator;
        _executer = executer;
        _player = player;
        _playerMover = playerMover;
        _range = range;
        _moveSpeed = moveSpeed;
        _rotationSpeed = rotationSpeed;
    }

    protected override void Cancel()
    {
        _playerMover.MoveEnded -= Cancel;
        _transformation.Cancel();
        base.Cancel();
        _navigator.HideAvailableCells();
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
        _navigator.RefillAvailableCells(_playerMover.CurrentCell, true, _range);
        _navigator.ShowAvailableCells();
        _transformation.Prepare();
        yield return null;
    }

    protected override void OnCommandChanged(Command command)
    {
        if (command is SkipCommand)
            return;

        Cancel();
    }
}
