using System.Collections;
using UnityEngine;

public abstract class Command
{
    private CommandExecuter _executer;
    private Coroutine _executeActionCoroutine = null;
    private Coroutine _prepareActionCoroutine = null;
    private Coroutine _waitOfExecute = null;
    private Coroutine _prepare = null;

    public Command(CommandExecuter executer)
    {
        _executer = executer;
    }

    public bool Enabled { get; private set; } = false;
    public bool IsExecuting { get; private set; }

    public IEnumerator Execute()
    {
        _prepare = _executer.StartCoroutine(Prepare());
        yield return _prepare;
        yield return new WaitUntil(() => Enabled);
        _executer.CommandChanged += OnCommandChanged;
        _waitOfExecute = _executer.StartCoroutine(WaitOfExecute());
        yield return _waitOfExecute;
        IsExecuting = true;
        _executeActionCoroutine = _executer.StartCoroutine(ExecuteAction());
        yield return _executeActionCoroutine;
        IsExecuting = false;
    }

    protected virtual void Cancel()
    {
        if (IsExecuting)
            return;

        Enabled = false;
        StopAction(ref _prepare);
        StopAction(ref _waitOfExecute);
        StopAction(ref _prepareActionCoroutine);
        StopAction(ref _executeActionCoroutine);
        _executer.CommandChanged -= OnCommandChanged;
    }

    protected IEnumerator Prepare()
    {
        Enabled = false;
        _prepareActionCoroutine = _executer.StartCoroutine(PrepareAction());
        yield return _prepareActionCoroutine;
        Enabled = true;
    }

    protected abstract IEnumerator WaitOfExecute();

    protected abstract IEnumerator PrepareAction();

    protected abstract IEnumerator ExecuteAction();

    protected abstract void OnCommandChanged(Command command);

    private void StopAction(ref Coroutine action) 
    {
        if(action != null) 
        {
            _executer.StopCoroutine(action);
            action = null;
        }
    }
}
