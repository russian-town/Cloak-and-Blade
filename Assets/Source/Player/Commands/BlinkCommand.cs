using System.Collections;
using UnityEngine;

public class BlinkCommand : Command
{
    private Blink _blink;
    private PlayerInput _playerInput;
    private Gameboard _gameboard;
    private Camera _camera;
    private Player _player;
    private Navigator _navigator;
    private Coroutine _executeCoroutine;

    public BlinkCommand(Blink blink, PlayerInput playerInput, Gameboard gameboard, Player player, Navigator navigator)
    {
        _blink = blink;
        _blink.Initialize();
        _playerInput = playerInput;
        _gameboard = gameboard;
        _player = player;
        _camera = Camera.main;
        _navigator = navigator;
    }

    protected override IEnumerator PrepareAction()
    {
        _blink.Prepare();
        yield break;
    }

    public override IEnumerator WaitOfExecute()
    {
        WaitOfClickedCell waitOfClickedCell = new WaitOfClickedCell(_playerInput, _gameboard, _camera, _player, _navigator);
        yield return waitOfClickedCell;
        _executeCoroutine = _player.StartCoroutine(Execute(waitOfClickedCell.Cell, _player));
        yield return _executeCoroutine;
    }

    protected override IEnumerator ExecuteAction(Cell clickedCell)
    {
        yield return new WaitUntil(() => _blink.Cast(clickedCell));
    }

    public override void Cancel(MonoBehaviour context)
    {
        base.Cancel(context);
        _blink.Cancel();

        if (_executeCoroutine != null)
        {
            _player.StopCoroutine(_executeCoroutine);
            _executeCoroutine = null;
        }
    }
}
