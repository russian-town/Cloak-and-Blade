using System.Collections;
using UnityEngine;

public class CommandExecuter : MonoBehaviour, ITurnHandler
{
    private Coroutine _prepare;
    private Coroutine _waitOfExecute;
    private Coroutine _switchCommand;
    private Command _currentCommand;
    private Turn _turn;

    public Command NextCommand { get; private set; }

    public void PrepareCommand(Command command)
    {
        if (_turn == Turn.Enemy)
            return;

        if (_currentCommand != null && _currentCommand.IsExecuting)
            return;

        if (_switchCommand != null)
        {
            StopCoroutine(_switchCommand);
            _switchCommand = null;
        }

        _switchCommand = StartCoroutine(SwitchCurrentCommand(command));
    }

    public void UpdateLastCommand()
    {
        if (!ResetCommand())
        {
            _prepare = StartCoroutine(_currentCommand.Prepare(this));
            _waitOfExecute = StartCoroutine(_currentCommand.WaitOfExecute());
        }
    }

    public void CancelCurrentCommand() => _currentCommand?.Cancel(this);

    public bool ResetCommand()
    {
        if (_currentCommand is not IUnmissable)
        {
            _currentCommand = null;
            return true;
        }

        return false;
    }

    public void SetTurn(Turn turn) => _turn = turn;

    private IEnumerator SwitchCurrentCommand(Command command)
    {
        if(_turn == Turn.Enemy)
            yield break;

        if (command == _currentCommand)
            yield break;

        if (_currentCommand != null && _currentCommand.IsExecuting)
            yield break;

        if (_prepare != null)
        {
            StopCoroutine(_prepare);
            _prepare = null;
        }

        if (_waitOfExecute != null)
        {
            StopCoroutine(_waitOfExecute);
            _waitOfExecute = null;
        }

        NextCommand = command;
        _currentCommand?.Cancel(this);
        _currentCommand = command;

        _prepare = StartCoroutine(_currentCommand.Prepare(this));
        yield return _prepare;

        _waitOfExecute = StartCoroutine(_currentCommand.WaitOfExecute());
        yield return _waitOfExecute;
    }
}
