using System.Collections;
using UnityEngine;

public class TransformationCommand : AbilityCommand, IDeferredCommand
{
    private Transformation _transformation;
    private Gameboard _gameboard;
    private Camera _camera;
    private Navigator _navigator;
    private Coroutine _executeCoroutine;
    private CommandExecuter _executer;

    public TransformationCommand(Transformation transformation, Gameboard gameboard, Navigator navigator, CommandExecuter executer) : base(transformation)
    {
        _transformation = transformation;
        _gameboard = gameboard;
        _camera = Camera.main;
        _navigator = navigator;
        _executer = executer;
    }

    public override IEnumerator WaitOfExecute()
    {
        Debug.Log("Wait");
        WaitOfClickedCell waitOfClickedCell = new WaitOfClickedCell(_gameboard, _camera, _navigator);
        yield return waitOfClickedCell;
        _executeCoroutine = _executer.StartCoroutine(Execute(waitOfClickedCell.Cell, _executer));
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
