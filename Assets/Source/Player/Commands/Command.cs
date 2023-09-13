using System.Collections;
using UnityEngine;

public abstract class Command
{
    public bool IsExecuting { get; private set; }
    public bool IsReady { get; private set; }

    public virtual IEnumerator Prepare(MonoBehaviour context)
    {
        yield return context.StartCoroutine(PrepareAction());
        IsReady = true;
    }

    public virtual IEnumerator Execute(Cell clickedCell, MonoBehaviour context)
    {
        IsExecuting = true;
        yield return context.StartCoroutine(ExecuteAction(clickedCell));
        IsExecuting = false;
    }

    protected abstract IEnumerator PrepareAction();

    protected abstract IEnumerator ExecuteAction(Cell clickedCell);

    public virtual void Cancel() => IsReady = false;
}
