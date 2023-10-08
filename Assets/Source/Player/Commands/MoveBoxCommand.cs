using System.Collections;
using UnityEngine;

public class MoveBoxCommand : Command
{
    private MoveCommand _moveCommand;
    private MonoBehaviour _context;

    public MoveBoxCommand(MoveCommand moveCommand, MonoBehaviour context)
    {
        _moveCommand = moveCommand;
        _context = context;
    }

    protected override IEnumerator ExecuteAction(Cell clickedCell)
    {
        yield return _context.StartCoroutine(_moveCommand.Execute(clickedCell, _context));
    }

    protected override IEnumerator PrepareAction()
    {
        yield return _context.StartCoroutine(_moveCommand.Prepare(_context));
    }

    public override void Cancel()
    {
        _moveCommand.Cancel();
    }
}
