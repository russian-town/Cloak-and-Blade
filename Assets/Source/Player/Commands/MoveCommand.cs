using System.Collections;
using UnityEngine;

public class MoveCommand : Command, ITurnHandler
{
    private readonly int _range;
    private readonly float _moveSpeed;
    private readonly float _rotationSpeed;
    private readonly Player _player;
    private readonly PlayerMover _playerMover;
    private readonly Navigator _navigator;
    private readonly Gameboard _gameboard;
    private readonly Camera _camera;
    private readonly CommandExecuter _executer;
    private Cell _cell;

    public MoveCommand(Player player, PlayerMover playerMover, Navigator navigator, float moveSpeed, float rotationSpeed, Gameboard gameboard, CommandExecuter executer, int range) : base(executer)
    {
        _player = player;
        _playerMover = playerMover;
        _navigator = navigator;
        _moveSpeed = moveSpeed;
        _rotationSpeed = rotationSpeed;
        _gameboard = gameboard;
        _camera = Camera.main;
        _executer = executer;
        _range = range;
    }

    protected override void Cancel()
    {
        base.Cancel();
        _navigator.HideAvailableCells();
    }

    public void SetTurn(Turn turn)
    {
        if (Enabled == false)
            return;

        if (turn == Turn.Enemy)
            Cancel();

        if (turn == Turn.Player && _executer.CurrentCommand == this)
            _executer.PrepareCommand();
    }

    protected override IEnumerator WaitOfExecute()
    {
        WaitOfClickedCell waitOfClickedCell = new WaitOfClickedCell(_gameboard, _camera, _navigator);
        yield return waitOfClickedCell;
        _cell = waitOfClickedCell.Cell;
    }

    protected override IEnumerator PrepareAction() 
    {
        _navigator.RefillAvailableCells(_playerMover.CurrentCell, true, _range);
        _navigator.ShowAvailableCells();
        yield return null;
    }

    protected override IEnumerator ExecuteAction()
    {
        if (_player.TryMoveToCell(_cell, _moveSpeed, _rotationSpeed))
            yield return _player.MoveCoroutine;
    }

    protected override void OnCommandChanged(Command command) => Cancel();
}
