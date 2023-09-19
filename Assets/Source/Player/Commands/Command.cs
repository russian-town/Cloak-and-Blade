using System.Collections;
using UnityEngine;

public abstract class Command
{
    private bool _isReady = false;

    public bool IsExecuting { get; private set; }

    public IEnumerator Prepare(MonoBehaviour context)
    {
        Debug.Log($"{this} prepare...");
        yield return context.StartCoroutine(PrepareAction());
        _isReady = true;
        Debug.Log($"{this} ready!");
    }

    public IEnumerator Execute(MonoBehaviour context)
    {
        yield return new WaitUntil(() => _isReady);
        Debug.Log($"{this} executing...");
        IsExecuting = true;
        yield return context.StartCoroutine(ExecuteAction());
        IsExecuting = false;
        Debug.Log($"{this} executed!");
    }

    protected abstract IEnumerator PrepareAction();

    protected abstract IEnumerator ExecuteAction();

    public virtual void Cancel() => _isReady = false;
}
