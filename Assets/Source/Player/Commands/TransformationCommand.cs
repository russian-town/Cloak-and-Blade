using System.Collections;
using UnityEngine;

public class TransformationCommand : AbilityCommand
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
        WaitOfClickedCell waitOfClickedCell = new WaitOfClickedCell(_gameboard, _camera, _navigator);
        yield return waitOfClickedCell;
        _executeCoroutine = _executer.StartCoroutine(Execute(waitOfClickedCell.Cell, _executer));
        yield return _executeCoroutine;
    }

    protected override IEnumerator ExecuteAction(Cell clickedCell)
    {
        _transformation.Cast(clickedCell);
        yield break;
    }

    protected override IEnumerator PrepareAction()
    {
        _transformation.Prepare();
        yield break;
    }

    public override void Cancel(MonoBehaviour context)
    {
        if (_executer.NextCommand is not SkipCommand)
        {
            base.Cancel(context);
            _transformation.Cancel();
        }
        else
        {
            _transformation.Prepare();
            _executer.StartCoroutine(WaitOfExecute());
        }
    }
}
