using System.Collections;
using UnityEngine;

public class BlinkCommand : AbilityCommand
{
    private Blink _blink;
    private Gameboard _gameboard;
    private Camera _camera;
    private Navigator _navigator;
    private Coroutine _executeCoroutine;
    private CommandExecuter _executer;

    public BlinkCommand(Blink blink, Gameboard gameboard, Navigator navigator, CommandExecuter executer) : base(blink)
    {
        _blink = blink;
        _gameboard = gameboard;
        _camera = Camera.main;
        _navigator = navigator;
        _executer = executer;
    }

    protected override IEnumerator PrepareAction()
    {
        _blink.Prepare();
        yield break;
    }

    public override IEnumerator WaitOfExecute()
    {
        WaitOfClickedCell waitOfClickedCell = new WaitOfClickedCell(_gameboard, _camera, _navigator);
        yield return waitOfClickedCell;
        _executeCoroutine = _executer.StartCoroutine(Execute(waitOfClickedCell.Cell, _executer));
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
            _executer.StopCoroutine(_executeCoroutine);
            _executeCoroutine = null;
        }
    }
}
