using System.Collections;
using UnityEngine;

public abstract class Command
{
    private bool _isReady;

    public bool IsExecuting { get; private set; }

    public virtual IEnumerator Prepare(MonoBehaviour context)
    {
        yield return context.StartCoroutine(PrepareAction());
        _isReady = true;
    }

    public virtual IEnumerator Execute(Cell clickedCell, MonoBehaviour context)
    {
        yield return new WaitUntil(() => _isReady);
        IsExecuting = true;
        yield return context.StartCoroutine(ExecuteAction(clickedCell));
        IsExecuting = false;
    }

    protected abstract IEnumerator PrepareAction();

    protected abstract IEnumerator ExecuteAction(Cell clickedCell);

    public virtual void Cancel() => _isReady = false;
}
