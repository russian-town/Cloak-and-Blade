using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    public bool IsExecuting { get; private set; }

    public abstract void Prepare();

    public virtual IEnumerator Execute(Cell clickedCell, MonoBehaviour context)
    {
        IsExecuting = true;
        yield return context.StartCoroutine(Action(clickedCell));
        Debug.Log($"Executing {this}");
        IsExecuting = false;
    }

    protected abstract IEnumerator Action(Cell clickedCell);

    public abstract void Cancel();
}
