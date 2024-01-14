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
    private Cell _cell;

    public BlinkCommand(Blink blink, Gameboard gameboard, Navigator navigator, CommandExecuter executer) : base(blink, executer)
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

    protected override IEnumerator WaitOfExecute()
    {
        WaitOfClickedCell waitOfClickedCell = new WaitOfClickedCell(_gameboard, _camera, _navigator);
        yield return waitOfClickedCell;
        _cell = waitOfClickedCell.Cell;
    }

    protected override IEnumerator ExecuteAction()
    {
        yield return new WaitUntil(() => _blink.Cast(_cell));
    }

    protected override void Cancel()
    {
        base.Cancel();
        _blink.Cancel();

        if (_executeCoroutine != null)
        {
            _executer.StopCoroutine(_executeCoroutine);
            _executeCoroutine = null;
        }
    }
}
