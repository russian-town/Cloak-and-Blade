using System.Collections;
using UnityEngine;

public class MoveCommand : Command, IUnmissable
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

    private Coroutine _executeCoroutine;

    public MoveCommand(Player player, PlayerMover playerMover, Navigator navigator, float moveSpeed, float rotationSpeed, Gameboard gameboard, CommandExecuter executer, int range)
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

    public override void Cancel(MonoBehaviour context)
    {
        _navigator.HideAvailableCells();

        if (_executeCoroutine != null)
        {
            _executer.StopCoroutine(_executeCoroutine);
            _executeCoroutine = null;
        }
    }

    public override IEnumerator WaitOfExecute()
    {
        WaitOfClickedCell waitOfClickedCell = new WaitOfClickedCell(_gameboard, _camera, _navigator);
        yield return waitOfClickedCell;
        _executeCoroutine = _player.StartCoroutine(Execute(waitOfClickedCell.Cell, _player));
        yield return _executeCoroutine;
    }

    protected override IEnumerator PrepareAction() 
    {
        _navigator.RefillAvailableCells(_playerMover.CurrentCell, true, _range);
        _navigator.ShowAvailableCells();
        yield return null;
    }

    protected override IEnumerator ExecuteAction(Cell clickedCell)
    {
        if (_player.TryMoveToCell(clickedCell, _moveSpeed, _rotationSpeed) == false)
            _executer.UpdateLastCommand();
        else
            yield return _player.MoveCoroutine;
    }
}
