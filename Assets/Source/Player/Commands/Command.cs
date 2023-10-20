using System.Collections;
using UnityEngine;

public abstract class Command
{
    private bool _isReady = false;
    private Coroutine _executeActionCoroutine = null;
    private Coroutine _prepareActionCoroutine = null;

    public bool IsExecuting { get; private set; }

    public abstract IEnumerator WaitOfExecute();

    public IEnumerator Prepare(MonoBehaviour context)
    {
        _isReady = false;
        _prepareActionCoroutine = context.StartCoroutine(PrepareAction());
        yield return _prepareActionCoroutine;
        _isReady = true;
    }

    public IEnumerator Execute(Cell clickedCell, MonoBehaviour context)
    {
        yield return new WaitUntil(() => _isReady);
        IsExecuting = true;
        _executeActionCoroutine = context.StartCoroutine(ExecuteAction(clickedCell));
        yield return _executeActionCoroutine;
        IsExecuting = false;
    }

    protected abstract IEnumerator PrepareAction();

    protected abstract IEnumerator ExecuteAction(Cell clickedCell);

    public virtual void Cancel(MonoBehaviour context)
    {
        _isReady = false;

        if (_prepareActionCoroutine != null)
        {
            context.StopCoroutine(_prepareActionCoroutine);
            _prepareActionCoroutine = null;
        }

        if(_executeActionCoroutine != null)
        {
            context.StopCoroutine(_executeActionCoroutine);
            _executeActionCoroutine = null;
        }
    }
}
