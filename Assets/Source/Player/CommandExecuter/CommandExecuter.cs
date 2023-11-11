using UnityEngine;
using UnityEngine.Events;

public class CommandExecuter : MonoBehaviour, ITurnHandler
{
    private Command _currentCommand;
    private Turn _turn;
    private AbilityCommand _abilityCommandToReset;

    public Command CurrentCommand => _currentCommand;

    public event UnityAction<Command> CommandChanged;
    public event UnityAction AbilityUsed;

    public bool TrySwitchCommand(Command command)
    {
        if (command is AbilityCommand abilityCommandParameter)
        {
            if (abilityCommandParameter.IsUsed)
            {
                print("AbilityParameter is used");
                AbilityUsed?.Invoke();
                _abilityCommandToReset = abilityCommandParameter;
            }
        }

        if (_currentCommand != null)
        {
            if (_currentCommand.GetType() == command.GetType() && command is not SkipCommand)
            {
                print("Trying to use same ability twice");
                if(_currentCommand is AbilityCommand abilityCommand)
                {
                    if (_currentCommand.Enabled == false)
                    {
                        return true;
                    }
                }

                return false;
            }
        }
            
        if(CanSwith() == false)
        {
            print("Cant switch");
            return false;
        }

        _currentCommand = command;
        CommandChanged?.Invoke(command);
        return true;
    }

    public void PrepareCommand()
    {
        if (_currentCommand == null)
            return;

        StartCoroutine(_currentCommand.Execute());
    }

    public void PrepareCommand(Command command)
    {
        _currentCommand = command;
        CommandChanged?.Invoke(command);

        StartCoroutine(_currentCommand.Execute());
    }

    public void ResetCommand()
    {
        _currentCommand = null;
        CommandChanged?.Invoke(_currentCommand);
    }

    public void ResetAbilityOnReward()
    {
        if(_abilityCommandToReset != null)
            _abilityCommandToReset.Ability.ResetAbility();

        _currentCommand = null;
    }

    public void SetTurn(Turn turn) => _turn = turn;

    private bool CanSwith()
    {
        if (_turn == Turn.Enemy)
            return false;

        if (_currentCommand != null && _currentCommand.IsExecuting)
            return false;

        return true;
    }
}
