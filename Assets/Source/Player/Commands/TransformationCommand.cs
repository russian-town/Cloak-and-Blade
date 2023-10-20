using System.Collections;
using UnityEngine;

public class TransformationCommand : Command, IDeferredCommand
{
    private Transformation _transformation;
    private PlayerInput _playerInput;
    private Gameboard _gameboard;
    private Camera _camera;
    private Player _player;
    private Navigator _navigator;
    private Coroutine _executeCoroutine;

    public TransformationCommand(Transformation transformation, PlayerInput playerInput, Gameboard gameboard, Player player, Navigator navigator)
    {
        _transformation = transformation;
        _transformation.Initialize();
        _playerInput = playerInput;
        _gameboard = gameboard;
        _camera = Camera.main;
        _player = player;
        _navigator = navigator;
    }

    public override IEnumerator WaitOfExecute()
    {
        Debug.Log("Wait");
        WaitOfClickedCell waitOfClickedCell = new WaitOfClickedCell(_playerInput, _gameboard, _camera, _player, _navigator);
        yield return waitOfClickedCell;
        _executeCoroutine = _player.StartCoroutine(Execute(waitOfClickedCell.Cell, _player));
        yield return _executeCoroutine;
    }

    protected override IEnumerator ExecuteAction(Cell clickedCell)
    {
        Debug.Log("Execute");
        yield return new WaitUntil(() => _transformation.Cast(clickedCell));
        yield break;
    }

    protected override IEnumerator PrepareAction()
    {
        Debug.Log("Prepared");
        _transformation.Prepare();
        yield break;
    }

    public override void Cancel(MonoBehaviour context)
    {
        base.Cancel(context);
        _transformation.Cancel();
    }
}
