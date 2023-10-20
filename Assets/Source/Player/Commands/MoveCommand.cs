using System.Collections;
using System.Linq;
using UnityEngine;

public class MoveCommand : Command, IUnmissable
{
    private float _moveSpeed;
    private float _rotationSpeed;
    private Player _player;
    private PlayerMover _playerMover;
    private PlayerView _playerView;
    private Navigator _navigator;
    private PlayerInput _playerInput;
    private Gameboard _gameboard;
    private Camera _camera;
    private Coroutine _executeCoroutine;

    public MoveCommand(Player player, PlayerMover playerMover, PlayerView playerView, Navigator navigator, float moveSpeed, float rotationSpeed, PlayerInput playerInput, Gameboard gameboard)
    {
        _player = player;
        _playerMover = playerMover;
        _playerView = playerView;
        _navigator = navigator;
        _moveSpeed = moveSpeed;
        _rotationSpeed = rotationSpeed;
        _playerInput = playerInput;
        _gameboard = gameboard;
        _camera = Camera.main;
    }

    protected override IEnumerator PrepareAction() 
    {
        _navigator.RefillAvailableCells(_playerMover.CurrentCell);
        _playerView.ShowAvailableCells(_navigator.AvailableCells.ToList());
        yield return null;
    }

    public override void Cancel(MonoBehaviour context)
    {
        _playerView.HideAvailableCells();

        if(_executeCoroutine != null)
        {
            _player.StopCoroutine(_executeCoroutine);
            _executeCoroutine = null;
        }
    }

    protected override IEnumerator ExecuteAction(Cell clickedCell)
    {
        if (_player.TryMoveToCell(clickedCell, _moveSpeed, _rotationSpeed))
            yield return _player.MoveCoroutine;
    }

    public override IEnumerator WaitOfExecute()
    {
        WaitOfClickedCell waitOfClickedCell = new WaitOfClickedCell(_playerInput, _gameboard, _camera, _player, _navigator);
        yield return waitOfClickedCell;
        _executeCoroutine = _player.StartCoroutine(Execute(waitOfClickedCell.Cell, _player));
        yield return _executeCoroutine;
    }
}
